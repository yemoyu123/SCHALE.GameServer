using Microsoft.AspNetCore.Http.Connections;
using SCHALE.Common.Database;
using SCHALE.Common.Database.ModelExtensions;
using SCHALE.Common.FlatData;
using SCHALE.GameServer.Controllers.Api.ProtocolHandlers;
using SCHALE.GameServer.Services;
using SCHALE.GameServer.Services.Irc;

namespace SCHALE.GameServer.Commands
{
    [CommandHandler("character", "Command to interact with user's characters", "/character <add|clear> [all|characterId]")]
    internal class CharacterCommand : Command
    {
        public CharacterCommand(IrcConnection connection, string[] args, bool validate = true) : base(connection, args, validate) { }

        [Argument(0, @"^add$|^clear$", "The operation selected (add, clear)", ArgumentFlags.IgnoreCase)]
        public string Op { get; set; } = string.Empty;

        [Argument(1, @"^[0-9]+$|^all$", "The target character, value is item id or 'all'", ArgumentFlags.Optional)]
        public string Target { get; set; } = string.Empty;

        public override void Execute()
        {
            var characterDB = connection.Context.Characters;

            switch (Op.ToLower())
            {
                case "add":
                    if (Target == "all")
                    {
                        AddAllCharacters(connection);

                        connection.SendChatMessage("All Characters Added!");
                    }
                    else if (uint.TryParse(Target, out uint characterId))
                    {
                        var newChar = CreateMaxCharacterFromId(characterId);
                        
                        if (characterDB.Any(x => x.UniqueId == newChar.UniqueId))
                        {
                            connection.SendChatMessage($"{newChar.UniqueId} already exists!");
                            return;
                        }

                        connection.Account.AddCharacters(connection.Context, [newChar]);
                        connection.SendChatMessage($"{newChar.UniqueId} added!");
                    }
                    else
                    {
                        throw new ArgumentException("Invalid Target / Amount!");
                    }

                    break;
                case "clear":
                    var defaultCharacters = connection.ExcelTableService.GetTable<DefaultCharacterExcelTable>().UnPack().DataList.Select(x => x.CharacterId).ToList();

                    var removed = characterDB.Where(x => x.AccountServerId == connection.AccountServerId && !defaultCharacters.Contains(x.UniqueId));

                    characterDB.RemoveRange(removed);
                    connection.SendChatMessage($"Removed {removed.Count()} characters!");
                    break;
                default:
                    connection.SendChatMessage($"Usage: /character unlock=<all|clear|characterId>");
                    throw new InvalidOperationException("Invalid operation!");
            }

            connection.Context.SaveChanges();
        }

        private void AddAllCharacters(IrcConnection connection)
        {
            var account = connection.Account;
            var context = connection.Context;

            var characterExcel = connection.ExcelTableService.GetTable<CharacterExcelTable>().UnPack().DataList;
            var allCharacters = characterExcel.Where(x => x.IsPlayable && x.IsPlayableCharacter && x.CollectionVisible && !account.Characters.Any(c => c.UniqueId == x.Id)).Select(x =>
            {
                return CreateMaxCharacterFromId(x.Id);
            }).ToList();

            account.AddCharacters(context, [.. allCharacters]);
            connection.Context.SaveChanges();
        }

        private CharacterDB CreateMaxCharacterFromId(long characterId)
        {
            return new CharacterDB()
            {
                UniqueId = characterId,
                StarGrade = 5,
                Level = 90,
                Exp = 0,
                PublicSkillLevel = 10,
                ExSkillLevel = 5,
                PassiveSkillLevel = 10,
                ExtraPassiveSkillLevel = 10,
                LeaderSkillLevel = 1,
                FavorRank = 500,
                IsNew = true,
                IsLocked = true,
                PotentialStats = { { 1, 0 }, { 2, 0 }, { 3, 0 } },
                EquipmentServerIds = [0, 0, 0]
            };
        }

    }
}
