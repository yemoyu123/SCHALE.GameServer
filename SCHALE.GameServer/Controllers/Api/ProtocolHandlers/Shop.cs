﻿using Microsoft.EntityFrameworkCore;
using SCHALE.Common.Database;
using SCHALE.Common.Database.ModelExtensions;
using SCHALE.Common.NetworkProtocol;
using SCHALE.Common.Utils;
using SCHALE.GameServer.Services;
using SCHALE.GameServer.Utils;

namespace SCHALE.GameServer.Controllers.Api.ProtocolHandlers
{
    public class Shop : ProtocolHandlerBase
    {
        private readonly ISessionKeyService _sessionKeyService;
        private readonly SCHALEContext _context;
        private readonly SharedDataCacheService _sharedData;
        private readonly ILogger<Shop> _logger;

        // TODO: temp storage until gacha management
        public List<long> SavedGachaResults { get; set; } = [];

        public Shop(IProtocolHandlerFactory protocolHandlerFactory, ISessionKeyService sessionKeyService, SCHALEContext context, SharedDataCacheService sharedData, ILogger<Shop> logger) : base(protocolHandlerFactory)
        {
            _sessionKeyService = sessionKeyService;
            _context = context;
            _sharedData = sharedData;
            _logger = logger;
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
            var accountChSet = account.Characters.Select(x => x.UniqueId).ToHashSet();

            // TODO: Implement FES Gacha
            // TODO: Check Gacha currency
            // TODO: SR pickup
            // TODO: pickup stone count
            // TODO: even more...
            // Type          Rate  Acc.R
            // -------------------------
            // Current SSR   0.7%   0.7% 
            // Other SSR     2.3%   3.0%ServerId
            // SR           18.5%  21.5%
            // R            78.5%  100.%

            const int gpStoneID = 90070086;
            const int chUniStoneID = 23;

            var rateUpChId = 10010; // 10094, 10095
            var rateUpIsNormalStudent = false;
            var gachaList = new List<GachaResult>(10);
            var itemDict = new AccDict<long>();
            bool shouldDoGuaranteedSR = true;
            // itemDict[gpStoneID] = 10;

            for (int i = 0; i < 10; ++i)
            {
                var randomNumber = Random.Shared.NextInt64(1000);
                if (randomNumber < 7)
                {
                    // always 3 star
                    shouldDoGuaranteedSR = false;
                    var isNew = accountChSet.Add(rateUpChId);
                    gachaList.Add(new(rateUpChId)
                    {
                        Character = !isNew ? null : new()
                        {
                            AccountServerId = account.ServerId,
                            UniqueId = rateUpChId,
                            StarGrade = 3,
                        },
                        Stone = isNew ? null : new()
                        {
                            UniqueId = chUniStoneID,
                            StackCount = 50,
                        }
                    });
                    if (!isNew)
                    {
                        itemDict[chUniStoneID] += 50;
                        itemDict[rateUpChId] += 100;
                    }
                }
                else if (randomNumber < 30)
                {
                    shouldDoGuaranteedSR = false;
                    var normalSSRList = _sharedData.CharaListSSRNormal;
                    var poolSize = normalSSRList.Count;
                    if (rateUpIsNormalStudent) poolSize--;

                    var randomPoolIdx = (int)Random.Shared.NextInt64(poolSize);
                    if (normalSSRList[randomPoolIdx].Id == rateUpChId) randomPoolIdx++;

                    var chId = normalSSRList[randomPoolIdx].Id;
                    var isNew = accountChSet.Add(chId);
                    gachaList.Add(new(chId)
                    {
                        Character = !isNew ? null : new()
                        {
                            AccountServerId = account.ServerId,
                            UniqueId = chId,
                            StarGrade = 3,
                        },
                        Stone = isNew ? null : new()
                        {
                            UniqueId = chUniStoneID,
                            StackCount = 50,
                        }
                    });
                    if (!isNew)
                    {
                        itemDict[chUniStoneID] += 50;
                        itemDict[chId] += 30;
                    }
                }
                else if (randomNumber < 215 || (i == 9 && shouldDoGuaranteedSR))
                {
                    shouldDoGuaranteedSR = false;
                    var normalSRList = _sharedData.CharaListSRNormal;
                    var randomPoolIdx = (int)Random.Shared.NextInt64(normalSRList.Count);
                    var chId = normalSRList[randomPoolIdx].Id;
                    var isNew = accountChSet.Add(chId);

                    gachaList.Add(new(chId)
                    {
                        Character = !isNew ? null : new()
                        {
                            AccountServerId = account.ServerId,
                            UniqueId = chId,
                            StarGrade = 2,
                        },
                        Stone = isNew ? null : new()
                        {
                            UniqueId = chUniStoneID,
                            StackCount = 10,
                        }
                    });
                    if (!isNew)
                    {
                        itemDict[chUniStoneID] += 10;
                        itemDict[chId] += 5;
                    }
                }
                else
                {
                    var normalRList = _sharedData.CharaListRNormal;
                    var randomPoolIdx = (int)Random.Shared.NextInt64(normalRList.Count);
                    var chId = normalRList[randomPoolIdx].Id;
                    var isNew = accountChSet.Add(chId);

                    gachaList.Add(new(chId)
                    {
                        Character = !isNew ? null : new()
                        {
                            AccountServerId = account.ServerId,
                            UniqueId = chId,
                            StarGrade = 1,
                        },
                        Stone = isNew ? null : new()
                        {
                            UniqueId = chUniStoneID,
                            StackCount = 1,
                        }
                    });
                    if (!isNew)
                    {
                        itemDict[chUniStoneID] += 1;
                        itemDict[chId] += 1;
                    }
                }
            }

            account.AddCharacters(_context, [.. gachaList.Where(x => x.Character != null).Select(x => x.Character)]);

            var acquiredItems = new List<ItemDB>();

            acquiredItems = itemDict.Keys.Select(x => new ItemDB()
            {
                IsNew = true,
                UniqueId = x,
                StackCount = itemDict[x],
            }).ToList();

            account.AddItems(_context, [.. acquiredItems]);
            _context.SaveChanges();

            return new ShopBuyGacha3Response()
            {
                GachaResults = gachaList,
                GemBonusRemain = int.MaxValue,
                ConsumedItems = [],
                AcquiredItems = acquiredItems,
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
