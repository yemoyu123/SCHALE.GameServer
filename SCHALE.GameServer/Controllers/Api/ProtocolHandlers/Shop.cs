using SCHALE.Common.Database;
using SCHALE.Common.NetworkProtocol;
using SCHALE.GameServer.Services;

namespace SCHALE.GameServer.Controllers.Api.ProtocolHandlers
{
    public class Shop : ProtocolHandlerBase
    {
        private readonly ISessionKeyService sessionKeyService;
        private readonly SCHALEContext context;

        // TODO: temp storage until gacha management
        public List<long> SavedGachaResults { get; set; } = [];

        public Shop(IProtocolHandlerFactory protocolHandlerFactory, ISessionKeyService _sessionKeyService, SCHALEContext _context) : base(protocolHandlerFactory)
        {
            sessionKeyService = _sessionKeyService;
            context = _context;
        }

        [ProtocolHandler(Protocol.Shop_BeforehandGachaGet)]
        public ResponsePacket BeforehandGachaGetHandler(ShopBeforehandGachaGetRequest req)
        {
            return new ShopBeforehandGachaGetResponse();
        }

        [ProtocolHandler(Protocol.Shop_BeforehandGachaRun)]
        public ResponsePacket BeforehandGachaRunHandler(ShopBeforehandGachaRunRequest req)
        {
            SavedGachaResults = [16003, 16003, 16003, 16003, 16003, 16003, 16003, 16003, 16003, 16003];

            return new ShopBeforehandGachaRunResponse()
            {
                SelectGachaSnapshot = new BeforehandGachaSnapshotDB()
                {
                    ShopUniqueId = 3,
                    GoodsId = 1,
                    LastResults = SavedGachaResults
                }
            };
        }

        [ProtocolHandler(Protocol.Shop_BeforehandGachaSave)]
        public ResponsePacket BeforehandGachaPickHandler(ShopBeforehandGachaSaveRequest req)
        {
            return new ShopBeforehandGachaSaveResponse()
            {
                SelectGachaSnapshot = new BeforehandGachaSnapshotDB()
                {
                    ShopUniqueId = 3,
                    GoodsId = 1,
                    LastResults = SavedGachaResults
                }
            };
        }

        [ProtocolHandler(Protocol.Shop_BeforehandGachaPick)]
        public ResponsePacket BeforehandGachaPickHandler(ShopBeforehandGachaPickRequest req)
        {
            var account = sessionKeyService.GetAccount(req.SessionKey);
            var GachaResults = new List<GachaResult>();

            foreach (var charId in SavedGachaResults)
            {
                GachaResults.Add(new GachaResult(charId) // hardcode until table
                {
                    Character = new()
                    {
                        ServerId = account.ServerId,
                        UniqueId = charId,
                        StarGrade = 3,
                        Level = 1,
                        FavorRank = 1,
                        PublicSkillLevel = 1,
                        ExSkillLevel = 1,
                        PassiveSkillLevel = 1,
                        ExtraPassiveSkillLevel = 1,
                        LeaderSkillLevel = 1,
                        IsNew = true,
                        IsLocked = true
                    }
                });
            }


            return new ShopBeforehandGachaPickResponse()
            {
                GachaResults = GachaResults
            };
        }

        [ProtocolHandler(Protocol.Shop_BuyGacha3)]
        public ResponsePacket ShopBuyGacha3ResponseHandler(ShopBuyGacha3Request req)
        {
            var gachaResults = new List<GachaResult>();

            for (int i = 0; i < 10; i++)
            {
                long id = 10000 + new Random().Next(0, 94);

                gachaResults.Add(new(id)
                {
                    Character = new() // hardcoded util proper db
                    {
                        ServerId = req.AccountId,
                        UniqueId = id,
                        StarGrade = 3,
                        Level = 1,
                        FavorRank = 1,
                        PublicSkillLevel = 1,
                        ExSkillLevel = 1,
                        PassiveSkillLevel = 1,
                        ExtraPassiveSkillLevel = 1,
                        LeaderSkillLevel = 1,
                        IsNew = true,
                        IsLocked = true
                    }
                });
            }

            return new ShopBuyGacha3Response()
            {
                GachaResults = gachaResults,
                UpdateTime = DateTime.UtcNow,
                GemBonusRemain = long.MaxValue,
                ConsumedItems = [],
                AcquiredItems = [],
                MissionProgressDBs = [],
            };
        }

        [ProtocolHandler(Protocol.Shop_List)]
        public ResponsePacket ListHandler(ShopListRequest req)
        {
            return new ShopListResponse()
            {
                ShopInfos = []
            };
        }

    }
}
