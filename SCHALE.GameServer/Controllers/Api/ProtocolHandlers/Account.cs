using SCHALE.Common.NetworkProtocol;
using SCHALE.Common.NetworkProtocol.Account;

namespace SCHALE.GameServer.Controllers.Api.ProtocolHandlers
{
    public static class Account
    {
        [ProtocolHandler(Protocol.Account_CheckYostar)]
        public static ResponsePacket CheckYostarHandler(AccountCheckYostarRequest req)
        {
            return new AccountCheckYostarResponse()
            {
                ResultState = 1,
                SessionKey = new()
                {
                    MxToken = req.EnterTicket,
                    AccountServerId = 1
                }
            };
        }

        [ProtocolHandler(Protocol.Account_Auth)]
        public static ResponsePacket AuthHandler(AccountAuthRequest req)
        {
            return new ErrorPacket()
            {
                ErrorCode = WebAPIErrorCode.AccountAuthNotCreated
            };
        }
    }
}
