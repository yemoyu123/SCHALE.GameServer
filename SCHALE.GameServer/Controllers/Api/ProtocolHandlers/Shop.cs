using SCHALE.Common.Database;
using SCHALE.Common.Database.ModelExtensions;
using SCHALE.Common.NetworkProtocol;
using SCHALE.GameServer.Services;

namespace SCHALE.GameServer.Controllers.Api.ProtocolHandlers
{
    public class Shop : ProtocolHandlerBase
    {
        private readonly ISessionKeyService _sessionKeyService;
        private readonly SCHALEContext _context;
        private readonly SharedDataCacheService _sharedData;

        // TODO: temp storage until gacha management
        public List<long> SavedGachaResults { get; set; } = [];

        public Shop(IProtocolHandlerFactory protocolHandlerFactory, ISessionKeyService sessionKeyService, SCHALEContext context, SharedDataCacheService sharedData) : base(protocolHandlerFactory)
        {
            _sessionKeyService = sessionKeyService;
            _context = context;
            _sharedData = sharedData;
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
            var account = _sessionKeyService.GetAccount(req.SessionKey);
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
            var account = _sessionKeyService.GetAccount(req.SessionKey);
            var accountCharacters = account.Characters.Select(x => x.UniqueId).ToHashSet();

            // TODO: Implement FES Gacha
            // TODO: Check Gacha currency
            // TODO: even more...
            // Type          Rate  Acc.R
            // -------------------------
            // Current SSR   0.7%   0.7% 
            // Other SSR     2.3%   3.0%ServerId
            // SR           18.5%  21.5%
            // R            78.5%  100.%

            var rateUpChId = 10094; // 10094, 10095
            var rateUpIsNormalStudent = false;
            var gachaList = new List<GachaResult>(10);
            var newChList = new List<CharacterDB>();

            for (int i = 0; i < 10; ++i)
            {
                var randomNumber = Random.Shared.NextInt64(1000);
                if (randomNumber < 7)
                {
                    // always 3 star
                    var isNew = accountCharacters.Add(rateUpChId);
                    gachaList.Add(new(rateUpChId)
                    {
                        Character = new()
                        {
                            AccountServerId = account.ServerId,
                            UniqueId = rateUpChId,
                            StarGrade = 3,
                            IsNew = isNew,
                            IsLocked = true,
                        }
                    });
                    if (isNew) newChList.Add(gachaList[i].Character);
                }
                else if (randomNumber < 30)
                {
                    var normalSSRList = _sharedData.CharaListSSRNormal;
                    var poolSize = normalSSRList.Count;
                    if (rateUpIsNormalStudent) poolSize--;

                    var randomPoolIdx = (int)Random.Shared.NextInt64(poolSize);
                    if (normalSSRList[randomPoolIdx].Id == rateUpChId) randomPoolIdx++;

                    var chId = normalSSRList[randomPoolIdx].Id;
                    var isNew = accountCharacters.Add(chId);
                    gachaList.Add(new(chId)
                    {
                        Character = new()
                        {
                            AccountServerId = account.ServerId,
                            UniqueId = chId,
                            StarGrade = 3,
                            IsNew = isNew,
                            IsLocked = true,
                        }
                    });
                    if (isNew) newChList.Add(gachaList[i].Character);
                }
                else if (randomNumber < 215 ||
                    (i == 9 && gachaList.All(x => x.Character.StarGrade == 1)))
                {
                    var normalSRList = _sharedData.CharaListSRNormal;
                    var randomPoolIdx = (int)Random.Shared.NextInt64(normalSRList.Count);
                    var chId = normalSRList[randomPoolIdx].Id;
                    var isNew = accountCharacters.Add(chId);

                    gachaList.Add(new(chId)
                    {
                        Character = new()
                        {
                            AccountServerId = account.ServerId,
                            UniqueId = chId,
                            StarGrade = 2,
                            IsNew = isNew,
                            IsLocked = true,
                        }
                    });
                    if (isNew) newChList.Add(gachaList[i].Character);
                }
                else
                {
                    var normalRList = _sharedData.CharaListRNormal;
                    var randomPoolIdx = (int)Random.Shared.NextInt64(normalRList.Count);
                    var chId = normalRList[randomPoolIdx].Id;
                    var isNew = accountCharacters.Add(chId);

                    gachaList.Add(new(chId)
                    {
                        Character = new()
                        {
                            AccountServerId = account.ServerId,
                            UniqueId = chId,
                            StarGrade = 1,
                            IsNew = isNew,
                            IsLocked = true,
                        }
                    });
                    if (isNew) newChList.Add(gachaList[i].Character);
                }
            }

            foreach (var ch in newChList)
            {
                ch.AccountServerId = account.ServerId;
            }
            _context.Characters.AddRange(newChList);
            _context.SaveChanges();

            return new ShopBuyGacha3Response()
            {
                GachaResults = gachaList,
                UpdateTime = DateTime.UtcNow,
                GemBonusRemain = int.MaxValue,
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
