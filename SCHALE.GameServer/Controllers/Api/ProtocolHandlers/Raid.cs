using SCHALE.Common.Database;
using SCHALE.Common.FlatData;
using SCHALE.Common.NetworkProtocol;
using SCHALE.GameServer.Services;

namespace SCHALE.GameServer.Controllers.Api.ProtocolHandlers
{
    public class Raid : ProtocolHandlerBase
    {
        private readonly ISessionKeyService sessionKeyService;
        private readonly SCHALEContext context;
        private readonly ExcelTableService excelTableService;

        public Raid(IProtocolHandlerFactory protocolHandlerFactory, ISessionKeyService _sessionKeyService, SCHALEContext _context, ExcelTableService _excelTableService) : base(protocolHandlerFactory)
        {
            sessionKeyService = _sessionKeyService;
            context = _context;
            excelTableService = _excelTableService;
        }

        [ProtocolHandler(Protocol.Raid_Lobby)]
        public ResponsePacket LobbyHandler(RaidLobbyRequest req)
        {
            var account = sessionKeyService.GetAccount(req.SessionKey);

            var raidSeasonInfo = excelTableService.GetTable<RaidSeasonManageExcelTable>().UnPack().DataList;
            var targetSeason = raidSeasonInfo.FirstOrDefault(x => x.SeasonId == account.RaidSeasonId);

            return new RaidLobbyResponse()
            {
                SeasonType = RaidSeasonType.Open,
                RaidLobbyInfoDB = new()
                {
                    SeasonId = account.RaidSeasonId,
                    Tier = 2,
                    Ranking = 1,
                    BestRankingPoint = 1_000_000,
                    TotalRankingPoint = 10_000_000,
                    ReceiveRewardIds = targetSeason.SeasonRewardId,
                    PlayableHighestDifficulty = new() {
                        { targetSeason.OpenRaidBossGroup.FirstOrDefault(), Difficulty.Torment }
                    }
                }
            };
        }

        [ProtocolHandler(Protocol.Raid_OpponentList)]
        public ResponsePacket OpponentListHandler(RaidOpponentListRequest req)
        {
            return new RaidOpponentListResponse()
            {
                OpponentUserDBs = []
            };
        }

        [ProtocolHandler(Protocol.Raid_CreateBattle)]
        public ResponsePacket CreateBattleHandller(RaidCreateBattleRequest req)
        {
            var account = sessionKeyService.GetAccount(req.SessionKey);

            var raidStageTable = excelTableService.GetTable<RaidStageExcelTable>().UnPack().DataList;
            var currentRaid = raidStageTable.FirstOrDefault(x => x.Id == req.RaidUniqueId);

            return new RaidCreateBattleResponse()
            {
                RaidDB = new()
                {
                    Owner = new()
                    {
                        AccountId = account.ServerId,
                        AccountName = account.Nickname,
                    },
                    ContentType = ContentType.Raid,
                    UniqueId = req.RaidUniqueId,
                    SeasonId = account.RaidSeasonId,
                    PlayerCount = 1,
                    RaidState = RaidStatus.Playing,
                    IsPractice = req.IsPractice,
                    RaidBossDBs = [
                        new() {
                            ContentType = ContentType.Raid,
                            BossCurrentHP = long.MaxValue
                        }
                    ],
                    AccountLevelWhenCreateDB = account.Level,
                },

                RaidBattleDB = new()
                {
                    ContentType = ContentType.Raid,
                    RaidUniqueId = req.RaidUniqueId,
                    CurrentBossHP = long.MaxValue,
                    RaidMembers = [
                        new() {
                            AccountId = account.ServerId,
                            AccountName = account.Nickname,
                        }    
                    ]
                },

                AssistCharacterDB = new () { }
            };
        }

        [ProtocolHandler(Protocol.Raid_EnterBattle)] // clicked restart
        public ResponsePacket EnterBattleHandler(RaidEnterBattleRequest req)
        {
            return new RaidEnterBattleResponse()
            {

            };
        }

        [ProtocolHandler(Protocol.Raid_EndBattle)]
        public ResponsePacket EndBattleHandler(RaidEndBattleRequest req)
        {
            return new RaidEndBattleResponse()
            {
            };
        }
    }
}
