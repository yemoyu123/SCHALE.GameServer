using SCHALE.Common.Database;
using SCHALE.Common.Database.ModelExtensions;
using SCHALE.Common.FlatData;
using SCHALE.GameServer.Services.Irc;

namespace SCHALE.GameServer.Commands
{
    [CommandHandler("character", "Unlock a character or all characters", "character unlock=all")]
    public class CharacterCommand : Command
    {
        [Argument("unlock")]
        public string? Unlock { get; set; }

        public override void Execute(Dictionary<string, string> args, IrcConnection connection)
        {
            base.Execute(args);

            // TODO: finish this
            if (Unlock is null)
            {
                connection.SendChatMessage($"Usage: /character unlock=<all|clear|shipId>");
                return;
            }

            if (Unlock.Equals("all", StringComparison.CurrentCultureIgnoreCase))
            {
                AddAllCharacters(connection);

                connection.SendChatMessage("All Characters Added!");
            } else if (Unlock.Equals("clear", StringComparison.CurrentCultureIgnoreCase))
            {

            } else if (uint.TryParse(Unlock, out uint characterId))
            {

            } else
            {
                connection.SendChatMessage($"Invalid Character Id: {characterId}");
                return;
            }

            connection.Context.SaveChanges();
            base.NotifySuccess(connection);
        }

        private void AddAllCharacters(IrcConnection connection)
        {
            var account = connection.Account;
            var context = connection.Context;

            var characterExcel = connection.ExcelTableService.GetTable<CharacterExcelTable>().UnPack().DataList;
            var allCharacters = characterExcel.Where(x => x.IsPlayable && x.IsPlayableCharacter && x.CollectionVisible && !account.Characters.Any(c => c.UniqueId == x.Id)).Select(x =>
            {
                return new CharacterDB()
                {
                    UniqueId = x.Id,
                    StarGrade = x.MaxStarGrade,
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
            }).ToList();

            account.AddCharacters(context, [.. allCharacters]);
            connection.Context.SaveChanges();
        }

    }
}
