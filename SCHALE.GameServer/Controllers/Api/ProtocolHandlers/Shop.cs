using SCHALE.Common.Database;
using SCHALE.Common.NetworkProtocol;
using SCHALE.GameServer.Services;
using Serilog;

namespace SCHALE.GameServer.Controllers.Api.ProtocolHandlers
{
    public class Shop : ProtocolHandlerBase
    {
        private ISessionKeyService sessionKeyService;
        private SCHALEContext context;

        public List<long> SavedGachaResults { get; set; } // temp storage until gacha management

        public Shop(IProtocolHandlerFactory protocolHandlerFactory, ISessionKeyService _sessionKeyService, SCHALEContext _context) : base(protocolHandlerFactory)
        {
            sessionKeyService = _sessionKeyService;
            context = _context;
        }

        [ProtocolHandler(Protocol.Shop_BeforehandGachaGet)]
        public ResponsePacket BeforehandGachaGetHandler(ShopBeforehandGachaGetRequest req)
        {
            return new ShopBeforehandGachaGetResponse()
            {
                SessionKey = new()
                {
                    MxToken = req.SessionKey.MxToken,
                    AccountServerId = req.SessionKey.AccountServerId,
                }
            };
        }

        [ProtocolHandler(Protocol.Shop_BeforehandGachaRun)]
        public ResponsePacket BeforehandGachaRunHandler(ShopBeforehandGachaRunRequest req)
        {
            // character ids here, just some limited 3 stars for now for fun
            SavedGachaResults = [10059, 20007, 10033, 10074, 10045, 10053, 10054, 10021, 20022, 10057];

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


    }
}
