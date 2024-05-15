using SCHALE.Common.Database;
using SCHALE.Common.Database.ModelExtensions;
using SCHALE.Common.FlatData;
using SCHALE.GameServer.Services;
using SCHALE.GameServer.Services.Irc;
using System;
using System.Data;

namespace SCHALE.Common.Utils
{
    public static class InventoryUtils
    {
        public static void AddAllCharacters(IrcConnection connection)
        {
            var account = connection.Account;
            var context = connection.Context;

            var characterExcel = connection.ExcelTableService.GetTable<CharacterExcelTable>().UnPack().DataList;
            var allCharacters = characterExcel.Where(x => x.IsPlayable && x.IsPlayableCharacter && x.CollectionVisible && !account.Characters.Any(c => c.UniqueId == x.Id)).Select(x =>
            {
                return CreateMaxCharacterFromId(x.Id);
            }).ToList();

            account.AddCharacters(context, [.. allCharacters]);
            context.SaveChanges();
        }

        public static void AddAllEquipment(IrcConnection connection)
        {
            var equipmentExcel = connection.ExcelTableService.GetTable<EquipmentExcelTable>().UnPack().DataList;
            var allEquipment = equipmentExcel.Select(x =>
            {
                return new EquipmentDB()
                {
                    UniqueId = x.Id,
                    Level = 1,
                    StackCount = 77777, // ~ 90,000 cap, auto converted if over
                };
            }).ToList();

            connection.Account.AddEquipment(connection.Context, [.. allEquipment]);
            connection.Context.SaveChanges();
        }

        public static void AddAllItems(IrcConnection connection)
        {
            var itemExcel = connection.ExcelTableService.GetTable<ItemExcelTable>().UnPack().DataList;
            var allItems = itemExcel.Select(x =>
            {
                return new ItemDB()
                {
                    IsNew = true,
                    UniqueId = x.Id,
                    StackCount = 5555,
                };
            }).ToList();

            connection.Account.AddItems(connection.Context, [.. allItems]);
            connection.Context.SaveChanges();
        }

        public static void AddAllWeapons(IrcConnection connection)
        {
            var account = connection.Account;
            var context = connection.Context;

            // only for current characters
            var allWeapons = account.Characters.Select(x =>
            {
                return new WeaponDB()
                {
                    UniqueId = x.UniqueId,
                    BoundCharacterServerId = x.ServerId,
                    IsLocked = false,
                    StarGrade = 5,
                    Level = 200
                };
            });

            account.AddWeapons(context, [.. allWeapons]);
            context.SaveChanges();
        }

        public static void RemoveAllCharacters(IrcConnection connection) // removing default characters breaks game
        {
            var characterDB = connection.Context.Characters;
            var defaultCharacters = connection.ExcelTableService.GetTable<DefaultCharacterExcelTable>().UnPack().DataList.Select(x => x.CharacterId).ToList();

            var removed = characterDB.Where(x => x.AccountServerId == connection.AccountServerId && !defaultCharacters.Contains(x.UniqueId));

            characterDB.RemoveRange(removed);
        }

        public static CharacterDB CreateMaxCharacterFromId(long characterId)
        {
            return new CharacterDB()
            {
                UniqueId = characterId,
                StarGrade = 5,
                Level = 200,
                Exp = 0,
                PublicSkillLevel = 10,
                ExSkillLevel = 5,
                PassiveSkillLevel = 10,
                ExtraPassiveSkillLevel = 10,
                LeaderSkillLevel = 1,
                FavorRank = 500,
                IsNew = true,
                IsLocked = true,
                PotentialStats = { { 1, 25 }, { 2, 25 }, { 3, 25 } },
                EquipmentServerIds = [0, 0, 0]
            };
        }
    }
}
