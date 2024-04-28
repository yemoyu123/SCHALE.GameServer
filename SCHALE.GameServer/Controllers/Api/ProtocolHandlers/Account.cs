using SCHALE.Common.Database;
using SCHALE.Common.NetworkProtocol;
using SCHALE.GameServer.Services;
using MongoDB.Driver.Linq;

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
            context.MissionProgresses.AddRange([
                new MissionProgressDB
                {
                    AccountServerId = account.ServerId,
                    MissionUniqueId = 1501,
                    Complete = true,
                    StartTime = DateTime.Parse("2024-04-26T20:46:44")
                },
                new MissionProgressDB
                {
                    AccountServerId = account.ServerId,
                    MissionUniqueId = 1700,
                    StartTime = DateTime.Parse("2024-04-26T20:46:44"),
                    ProgressParameters = new Dictionary<long, long> { { 0, 1 } }
                },
                new MissionProgressDB
                {
                    AccountServerId = account.ServerId,
                    MissionUniqueId = 1500,
                    Complete = true,
                    StartTime = DateTime.Parse("2024-04-26T20:46:44")
                },
                new MissionProgressDB
                {
                    AccountServerId = account.ServerId,
                    MissionUniqueId = 2200,
                    StartTime = DateTime.Parse("2024-04-26T20:46:44"),
                    ProgressParameters = new Dictionary<long, long> { { 0, 1 }, { 1, 5 } }
                },
                new MissionProgressDB
                {
                    AccountServerId = account.ServerId,
                    MissionUniqueId = 300000,
                    StartTime = DateTime.Parse("2024-04-26T20:46:44"),
                    ProgressParameters = new Dictionary<long, long> { { 0, 1 } }
                },
                new MissionProgressDB
                {
                    AccountServerId = account.ServerId,
                    MissionUniqueId = 1000210,
                    Complete = true,
                    StartTime = DateTime.Parse("2024-04-26T20:46:44"),
                    ProgressParameters = new Dictionary<long, long> { { 0, 1 } }
                },
                new MissionProgressDB
                {
                    AccountServerId = account.ServerId,
                    MissionUniqueId = 1000220,
                    Complete = true,
                    StartTime = DateTime.Parse("2024-04-26T20:46:44"),
                    ProgressParameters = new Dictionary<long, long> { { 0, 1 } }
                },
                new MissionProgressDB
                {
                    AccountServerId = account.ServerId,
                    MissionUniqueId = 1000230,
                    Complete = true,
                    StartTime = DateTime.Parse("2024-04-26T20:46:44"),
                    ProgressParameters = new Dictionary<long, long> { { 0, 1 } }
                },
                new MissionProgressDB
                {
                    AccountServerId = account.ServerId,
                    MissionUniqueId = 1000240,
                    Complete = true,
                    StartTime = DateTime.Parse("2024-04-26T20:46:44"),
                    ProgressParameters = new Dictionary<long, long> { { 0, 1 } }
                },
                new MissionProgressDB
                {
                    AccountServerId = account.ServerId,
                    MissionUniqueId = 1000250,
                    Complete = true,
                    StartTime = DateTime.Parse("2024-04-26T20:46:44"),
                    ProgressParameters = new Dictionary<long, long> { { 0, 1 } }
                },
                new MissionProgressDB
                {
                    AccountServerId = account.ServerId,
                    MissionUniqueId = 1000260,
                    Complete = true,
                    StartTime = DateTime.Parse("2024-04-26T20:46:44"),
                    ProgressParameters = new Dictionary<long, long> { { 0, 1 } }
                },
                new MissionProgressDB
                {
                    AccountServerId = account.ServerId,
                    MissionUniqueId = 1000270,
                    Complete = true,
                    StartTime = DateTime.Parse("2024-04-26T20:46:44"),
                    ProgressParameters = new Dictionary<long, long> { { 0, 1 } }
                },
                new MissionProgressDB
                {
                    AccountServerId = account.ServerId,
                    MissionUniqueId = 1001327,
                    Complete = true,
                    StartTime = DateTime.Parse("2024-04-26T20:46:44"),
                    ProgressParameters = new Dictionary<long, long> { { 0, 1 } }
                },
                new MissionProgressDB
                {
                    AccountServerId = account.ServerId,
                    MissionUniqueId = 1001357,
                    Complete = true,
                    StartTime = DateTime.Parse("2024-04-26T20:46:44"),
                    ProgressParameters = new Dictionary<long, long> { { 0, 1 } }
                },
                new MissionProgressDB
                {
                    AccountServerId = account.ServerId,
                    MissionUniqueId = 1001377,
                    Complete = true,
                    StartTime = DateTime.Parse("2024-04-26T20:46:44"),
                    ProgressParameters = new Dictionary<long, long> { { 0, 1 } }
                }
            ]);
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

            };
        }

        [ProtocolHandler(Protocol.Account_GetTutorial)]
        public ResponsePacket GetTutorialHandler(AccountGetTutorialRequest req)
        {

            return new AccountGetTutorialResponse()
            {

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
        [ProtocolHandler(Protocol.Item_List)]
        public ResponsePacket Item_ListRequestHandler(ItemListRequest req)
        {
            return new ItemListResponse()
            {
                ItemDBs = [],
                ExpiryItemDBs = [],
                ServerNotification = ServerNotificationFlag.HasUnreadMail,
            };
        }

        [ProtocolHandler(Protocol.NetworkTime_Sync)]
        public ResponsePacket NetworkTime_Sync(NetworkTimeSyncRequest req)
        {
            long received_tick = DateTimeOffset.Now.Ticks;

            return new NetworkTimeSyncResponse()
            {
                ReceiveTick = received_tick,
                EchoSendTick = DateTimeOffset.Now.Ticks
            };
        }

        [ProtocolHandler(Protocol.ContentSave_Get)]
        public ResponsePacket ContentSave_Get(ContentSaveGetRequest req)
        {
            return new ContentSaveGetResponse()
            {
                ServerNotification = ServerNotificationFlag.HasUnreadMail,
            };
        }

        [ProtocolHandler(Protocol.Shop_BeforehandGachaGet)]
        public ResponsePacket Shop_BeforehandGachaGet(ShopBeforehandGachaGetRequest req)
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

        [ProtocolHandler(Protocol.Toast_List)]
        public ResponsePacket ToastListHandler(ToastListRequest req)
        {

            return new ToastListResponse()
            {

            };
        }

        [ProtocolHandler(Protocol.EventContent_CollectionList)]
        public ResponsePacket EventContent_CollectionListHandler(EventContentCollectionListRequest req)
        {

            return new EventContentCollectionListResponse()
            {

            };
        }
    }

}
