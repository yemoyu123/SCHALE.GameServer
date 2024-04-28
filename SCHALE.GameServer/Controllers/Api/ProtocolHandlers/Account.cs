using SCHALE.Common.Database;
using SCHALE.Common.NetworkProtocol;
using SCHALE.GameServer.Services;
using MongoDB.Driver.Linq;
using SCHALE.Common.FlatData;
using Serilog;

namespace SCHALE.GameServer.Controllers.Api.ProtocolHandlers
{
    public class Account : ProtocolHandlerBase
    {
        private ISessionKeyService sessionKeyService;
        private SCHALEContext context;

        public Account(IProtocolHandlerFactory protocolHandlerFactory, ISessionKeyService _sessionKeyService, SCHALEContext _context) : base(protocolHandlerFactory)
        {
            sessionKeyService = _sessionKeyService;
            context = _context;
        }

        // most handlers empty
        [ProtocolHandler(Protocol.Account_CheckYostar)]
        public ResponsePacket CheckYostarHandler(AccountCheckYostarRequest req)
        {
            string[] uidToken = req.EnterTicket.Split(':');

            var account = context.GuestAccounts.SingleOrDefault(x => x.Uid == long.Parse(uidToken[0]) && x.Token == uidToken[1]);

            if (account is null)
            {
                return new AccountCheckYostarResponse()
                {
                    ResultState = 0,
                    ResultMessag = "Invalid account (EnterTicket, AccountCheckYostar)"
                };
            }

            return new AccountCheckYostarResponse()
            {
                ResultState = 1,
                SessionKey = sessionKeyService.Create(account.Uid) ?? new SessionKey() { MxToken = req.EnterTicket }
            };
        }

        [ProtocolHandler(Protocol.Account_Auth)]
        public ResponsePacket AuthHandler(AccountAuthRequest req)
        {
            if (req.SessionKey is null || req.SessionKey.AccountServerId == 0)
            {
                return new ErrorPacket()
                {
                    ErrorCode = WebAPIErrorCode.AccountAuthNotCreated
                };
            }
            else
            {
                var account = sessionKeyService.GetAccount(req.SessionKey);

                return new AccountAuthResponse()
                {
                    CurrentVersion = req.Version,
                    AccountDB = account,

                    MissionProgressDBs = [.. context.MissionProgresses.Where(x => x.AccountServerId == account.ServerId)]
                };

            }
        }

        [ProtocolHandler(Protocol.Account_Create)]
        public ResponsePacket CreateHandler(AccountCreateRequest req)
        {
            if (req.SessionKey is null)
            {
                return new ErrorPacket()
                {
                    ErrorCode = WebAPIErrorCode.InvalidSession
                };
            }

            string[] uidToken = req.SessionKey.MxToken.Split(':');
            var account = new AccountDB(long.Parse(uidToken[0]));

            context.Accounts.Add(account);
            context.SaveChanges();

            return new AccountCreateResponse()
            {
                SessionKey = sessionKeyService.Create(account.PublisherAccountId)
            };
        }

        [ProtocolHandler(Protocol.Account_Nickname)]
        public ResponsePacket NicknameHandler(AccountNicknameRequest req)
        {
            var account = sessionKeyService.GetAccount(req.SessionKey!);

            account.Nickname = req.Nickname;
            context.SaveChanges();

            return new AccountNicknameResponse()
            {
                AccountDB = account
            };

        }

        [ProtocolHandler(Protocol.Account_LoginSync)]
        public ResponsePacket LoginSyncHandler(AccountLoginSyncRequest req)
        {
            return new AccountLoginSyncResponse()
            {
                AccountCurrencySyncResponse = new AccountCurrencySyncResponse()
                {
                    AccountCurrencyDB = new AccountCurrencyDB
                    {
                        AccountLevel = 1,
                        AcademyLocationRankSum = 1,
                        CurrencyDict = new Dictionary<CurrencyTypes, long>
                        {
                            { CurrencyTypes.Gem, long.MaxValue }, // gacha currency 600
                            { CurrencyTypes.GemPaid, 0 },
                            { CurrencyTypes.GemBonus, 89473598435 }, // default blue gem?
                            { CurrencyTypes.Gold, long.MaxValue }, // credit 10,000
                            { CurrencyTypes.ActionPoint, long.MaxValue }, // energy  24
                            { CurrencyTypes.AcademyTicket, 3 },
                            { CurrencyTypes.ArenaTicket, 5 },
                            { CurrencyTypes.RaidTicket, 3 },
                            { CurrencyTypes.WeekDungeonChaserATicket, 0 },
                            { CurrencyTypes.WeekDungeonChaserBTicket, 0 },
                            { CurrencyTypes.WeekDungeonChaserCTicket, 0 },
                            { CurrencyTypes.SchoolDungeonATicket, 0 },
                            { CurrencyTypes.SchoolDungeonBTicket, 0 },
                            { CurrencyTypes.SchoolDungeonCTicket, 0 },
                            { CurrencyTypes.TimeAttackDungeonTicket, 3 },
                            { CurrencyTypes.MasterCoin, 0 },
                            { CurrencyTypes.WorldRaidTicketA, 40 },
                            { CurrencyTypes.WorldRaidTicketB, 40 },
                            { CurrencyTypes.WorldRaidTicketC, 40 },
                            { CurrencyTypes.ChaserTotalTicket, 6 },
                            { CurrencyTypes.SchoolDungeonTotalTicket, 6 },
                            { CurrencyTypes.EliminateTicketA, 1 },
                            { CurrencyTypes.EliminateTicketB, 1 },
                            { CurrencyTypes.EliminateTicketC, 1 },
                            { CurrencyTypes.EliminateTicketD, 1 }
                        },
                        UpdateTimeDict = new Dictionary<CurrencyTypes, DateTime>
                        {
                            { CurrencyTypes.ActionPoint, DateTime.Parse("2024-04-26T19:29:12") },
                            { CurrencyTypes.AcademyTicket, DateTime.Parse("2024-04-26T19:29:12") },
                            { CurrencyTypes.ArenaTicket, DateTime.Parse("2024-04-26T19:29:12") },
                            { CurrencyTypes.RaidTicket, DateTime.Parse("2024-04-26T19:29:12") },
                            { CurrencyTypes.WeekDungeonChaserATicket, DateTime.Parse("2024-04-26T19:29:12") },
                            { CurrencyTypes.WeekDungeonChaserBTicket, DateTime.Parse("2024-04-26T19:29:12") },
                            { CurrencyTypes.WeekDungeonChaserCTicket, DateTime.Parse("2024-04-26T19:29:12") },
                            { CurrencyTypes.SchoolDungeonATicket, DateTime.Parse("2024-04-26T19:29:12") },
                            { CurrencyTypes.SchoolDungeonBTicket, DateTime.Parse("2024-04-26T19:29:12") },
                            { CurrencyTypes.SchoolDungeonCTicket, DateTime.Parse("2024-04-26T19:29:12") },
                            { CurrencyTypes.TimeAttackDungeonTicket, DateTime.Parse("2024-04-26T19:29:12") },
                            { CurrencyTypes.MasterCoin, DateTime.Parse("2024-04-26T19:29:12") },
                            { CurrencyTypes.WorldRaidTicketA, DateTime.Parse("2024-04-26T19:29:12") },
                            { CurrencyTypes.WorldRaidTicketB, DateTime.Parse("2024-04-26T19:29:12") },
                            { CurrencyTypes.WorldRaidTicketC, DateTime.Parse("2024-04-26T19:29:12") },
                            { CurrencyTypes.ChaserTotalTicket, DateTime.Parse("2024-04-26T19:29:12") },
                            { CurrencyTypes.SchoolDungeonTotalTicket, DateTime.Parse("2024-04-26T19:29:12") },
                            { CurrencyTypes.EliminateTicketA, DateTime.Parse("2024-04-26T19:29:12") },
                            { CurrencyTypes.EliminateTicketB, DateTime.Parse("2024-04-26T19:29:12") },
                            { CurrencyTypes.EliminateTicketC, DateTime.Parse("2024-04-26T19:29:12") },
                            { CurrencyTypes.EliminateTicketD, DateTime.Parse("2024-04-26T19:29:12") }
                    }
                    }
                },
                CharacterListResponse = new CharacterListResponse()
                {
                    CharacterDBs = new List<CharacterDB>
                    {
                        new CharacterDB
                        {
                            ServerId = 1043998219,
                            UniqueId = 13003,
                            StarGrade = 2,
                            Level = 1,
                            FavorRank = 1,
                            PublicSkillLevel = 1,
                            ExSkillLevel = 1,
                            PassiveSkillLevel = 1,
                            ExtraPassiveSkillLevel = 1,
                            LeaderSkillLevel = 1,
                            IsNew = true,
                            IsLocked = true,
                            EquipmentServerIds = new List<long> { 0, 0, 0 },
                            PotentialStats = new Dictionary<int, int> { { 1, 0 }, { 2, 0 }, { 3, 0 } }
                        },
                        new CharacterDB
                        {
                            ServerId = 1043998217,
                            UniqueId = 13010,
                            StarGrade = 2,
                            Level = 1,
                            FavorRank = 1,
                            PublicSkillLevel = 1,
                            ExSkillLevel = 1,
                            PassiveSkillLevel = 1,
                            ExtraPassiveSkillLevel = 1,
                            LeaderSkillLevel = 1,
                            IsNew = true,
                            IsLocked = true,
                            EquipmentServerIds = new List<long> { 0, 0, 0 },
                            PotentialStats = new Dictionary<int, int> { { 1, 0 }, { 2, 0 }, { 3, 0 } }
                        },
                        new CharacterDB
                        {
                            ServerId = 1043998218,
                            UniqueId = 16003,
                            StarGrade = 1,
                            Level = 1,
                            FavorRank = 1,
                            PublicSkillLevel = 1,
                            ExSkillLevel = 1,
                            PassiveSkillLevel = 1,
                            ExtraPassiveSkillLevel = 1,
                            LeaderSkillLevel = 1,
                            IsNew = true,
                            IsLocked = true,
                            EquipmentServerIds = new List<long> { 0, 0, 0 },
                            PotentialStats = new Dictionary<int, int> { { 1, 0 }, { 2, 0 }, { 3, 0 } }
                        },
                        new CharacterDB
                        {
                            ServerId = 1043998220,
                            UniqueId = 26000,
                            StarGrade = 1,
                            Level = 1,
                            FavorRank = 1,
                            PublicSkillLevel = 1,
                            ExSkillLevel = 1,
                            PassiveSkillLevel = 1,
                            ExtraPassiveSkillLevel = 1,
                            LeaderSkillLevel = 1,
                            IsNew = true,
                            IsLocked = true,
                            EquipmentServerIds = new List<long> { 0, 0, 0 },
                            PotentialStats = new Dictionary<int, int> { { 1, 0 }, { 2, 0 }, { 3, 0 } }
                        }
                    },
                    TSSCharacterDBs = [],
                    WeaponDBs = [],
                    CostumeDBs = [],
                },
                EchelonListResponse = new EchelonListResponse()
                {
                    EchelonDBs = [
                        new EchelonDB()
                        {
                            AccountServerId = req.AccountId,
                            EchelonType = EchelonType.Adventure,
                            EchelonNumber = 1,
                            LeaderServerId = 123,
                            MainSlotServerIds = [1043998217, 1043998218, 1043998219, 0 ],
                            SupportSlotServerIds = [ 444, 0],
                            SkillCardMulliganCharacterIds = []
                        }
                    ]
                },
                EventContentPermanentListResponse = new EventContentPermanentListResponse()
                {
                    PermanentDBs = [
                        new() { EventContentId = 900801 },
                        new() { EventContentId = 900802 },
                        new() { EventContentId = 900803 },
                        new() { EventContentId = 900804 },
                        new() { EventContentId = 900805 },
                        new() { EventContentId = 900806 },
                        new() { EventContentId = 900808 },
                        new() { EventContentId = 900809 },
                        new() { EventContentId = 900810 },
                        new() { EventContentId = 900812 },
                        new() { EventContentId = 900813 },
                        new() { EventContentId = 900816 },
                        new() { EventContentId = 900701 },
                    ],
                },

                FriendCode = "SCHALEPS",

                ServerNotification = ServerNotificationFlag.HasUnreadMail,
                SessionKey = new()
                {
                    AccountServerId = req.AccountId,
                    MxToken = req.SessionKey.MxToken,
                },
            };
        }

        [ProtocolHandler(Protocol.Account_GetTutorial)]
        public ResponsePacket GetTutorialHandler(AccountGetTutorialRequest req)
        {
            return new AccountGetTutorialResponse()
            {
#if DEBUG
                TutorialIds = [1, 2]
#endif
            };
        }

        [ProtocolHandler(Protocol.Account_SetTutorial)]
        public ResponsePacket SetTutorialHandler(AccountSetTutorialRequest req)
        {

            return new AccountSetTutorialResponse()
            {

            };
        }

        // others handlers, move to different handler group later
        [ProtocolHandler(Protocol.NetworkTime_Sync)]
        public ResponsePacket NetworkTime_SyncHandler(NetworkTimeSyncRequest req)
        {
            long received_tick = DateTimeOffset.Now.Ticks;

            return new NetworkTimeSyncResponse()
            {
                ReceiveTick = received_tick,
                EchoSendTick = DateTimeOffset.Now.Ticks
            };
        }

        [ProtocolHandler(Protocol.ContentSave_Get)]
        public ResponsePacket ContentSave_GetHandler(ContentSaveGetRequest req)
        {
            return new ContentSaveGetResponse()
            {
                ServerNotification = ServerNotificationFlag.HasUnreadMail,
            };
        }

        [ProtocolHandler(Protocol.Toast_List)]
        public ResponsePacket ToastListHandler(ToastListRequest req)
        {

            return new ToastListResponse()
            {

            };
        }

        [ProtocolHandler(Protocol.ContentLog_UIOpenStatistics)]
        public ResponsePacket ContentLog_UIOpenStatisticsHandler(ContentLogUIOpenStatisticsRequest req)
        {

            return new ContentLogUIOpenStatisticsResponse()
            {

            };
        }

        [ProtocolHandler(Protocol.Event_RewardIncrease)]
        public ResponsePacket Event_RewardIncreaseHandler(EventRewardIncreaseRequest req)
        {

            return new EventRewardIncreaseResponse()
            {

            };
        }

        [ProtocolHandler(Protocol.OpenCondition_EventList)]
        public ResponsePacket OpenCondition_EventListHandler(OpenConditionEventListRequest req)
        {

            return new OpenConditionEventListResponse()
            {

            };
        }

        [ProtocolHandler(Protocol.Notification_EventContentReddotCheck)]
        public ResponsePacket Notification_EventContentReddotCheckHandler(NotificationEventContentReddotRequest req)
        {

            return new NotificationEventContentReddotResponse()
            {

            };
        }

        [ProtocolHandler(Protocol.Billing_PurchaseListByYostar)]
        public ResponsePacket Billing_PurchaseListByYostarHandler(BillingPurchaseListByYostarRequest req)
        {

            return new BillingPurchaseListByYostarResponse()
            {

            };
        }
    }

}
