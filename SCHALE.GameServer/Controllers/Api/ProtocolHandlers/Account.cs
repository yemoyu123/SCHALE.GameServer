using SCHALE.Common.NetworkProtocol;

namespace SCHALE.GameServer.Controllers.Api.ProtocolHandlers
{
    public class Account : ProtocolHandlerBase
    {
        public Account(IServiceScopeFactory scopeFactory, IProtocolHandlerFactory protocolHandlerFactory) : base(scopeFactory, protocolHandlerFactory) { }

        [ProtocolHandler(Protocol.Account_CheckYostar)]
        public ResponsePacket CheckYostarHandler(AccountCheckYostarRequest req)
        {
            var account = Context.GuestAccounts.SingleOrDefault(x => x.Uid == uint.Parse(req.EnterTicket.Split(":", StringSplitOptions.None).First()) && x.Token == req.EnterTicket.Split(":", StringSplitOptions.None).Last());
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
                    MxToken = req.EnterTicket,
                    AccountServerId = account.Uid
                }
            };
        }

        [ProtocolHandler(Protocol.Account_Auth)]
        public ResponsePacket AuthHandler(AccountAuthRequest req)
        {
            return new ErrorPacket()
            {
                ErrorCode = WebAPIErrorCode.AccountAuthNotCreated
            };
        }
    }
}
