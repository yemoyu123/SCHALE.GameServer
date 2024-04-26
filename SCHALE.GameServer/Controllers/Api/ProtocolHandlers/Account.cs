using SCHALE.Common.Database;
using SCHALE.Common.FlatData;
using SCHALE.Common.NetworkProtocol;
using SCHALE.Common.Parcel;
using Serilog;

namespace SCHALE.GameServer.Controllers.Api.ProtocolHandlers
{   
    public class Account : ProtocolHandlerBase
    {
        public Account(IServiceScopeFactory scopeFactory, IProtocolHandlerFactory protocolHandlerFactory) : base(scopeFactory, protocolHandlerFactory) { }

        // most handlers empty
        [ProtocolHandler(Protocol.Account_CheckYostar)]
        public ResponsePacket CheckYostarHandler(AccountCheckYostarRequest req)
        {
            string[] uid_token = req.EnterTicket.Split(':');

            var account = Context.GuestAccounts.SingleOrDefault(x => x.Uid == uint.Parse(uid_token[0]) && x.Token == uid_token[1]);
            
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
                SessionKey = new()
                {
                    AccountServerId = account.Uid,
                    MxToken = req.EnterTicket,
                }
            };
        }

        [ProtocolHandler(Protocol.Account_Auth)]
        public ResponsePacket AuthHandler(AccountAuthRequest req)
        {
            var account = Context.Accounts.SingleOrDefault(x => x.ServerId == req.AccountId);

            if (account == null)
            {
                return new ErrorPacket()
                {
                    ErrorCode = WebAPIErrorCode.AccountAuthNotCreated
                };
            } 
            else
            {
                return new AccountAuthResponse()
                {
                    CurrentVersion = req.Version,
                    AccountDB = account.AccountDB,

                    SessionKey = new()
                    {
                        AccountServerId = account.ServerId,
                        MxToken = req.SessionKey.MxToken,
                    }
                };
            }
        }

        [ProtocolHandler(Protocol.Account_Create)]
        public ResponsePacket CreateHandler(AccountCreateRequest req)
        {
            var account = Common.Database.Models.Game.Account.Create((uint)req.AccountId);

            Context.Accounts.Add(account);
            Context.SaveChanges();

            Log.Information("Account Created " + Context.Accounts.Count());

            return new AccountCreateResponse()
            {
                SessionKey = new()
                {
                    AccountServerId = account.ServerId,
                    MxToken = req.SessionKey.MxToken,
                },
            };
        }

        [ProtocolHandler(Protocol.Account_Nickname)]
        public ResponsePacket NicknameHandler(AccountNicknameRequest req)
        {
            var account = Context.Accounts.SingleOrDefault(x => x.ServerId == req.AccountId);
            
            account.AccountDB.Nickname = req.Nickname;
            Context.SaveChanges();

            return new AccountNicknameResponse()
            {
                AccountDB = account.AccountDB
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
                EchoSendTick = DateTimeOffset.Now.Ticks,
                ServerTimeTicks = received_tick,
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
                SessionKey = new ()
                {
                    MxToken = req.SessionKey.MxToken,
                    AccountServerId = req.SessionKey.AccountServerId,
                },
            };
        }

        [ProtocolHandler(Protocol.Toast_List)]
        public ResponsePacket ToastListHandler(ToastListRequest req)
        {

            return new ToastListResponse()
            {

            };
        }
    }
    
}
