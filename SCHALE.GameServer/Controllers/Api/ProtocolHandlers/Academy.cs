using SCHALE.Common.NetworkProtocol;

namespace SCHALE.GameServer.Controllers.Api.ProtocolHandlers
{
    public class Academy : ProtocolHandlerBase
    {
        public Academy(IServiceScopeFactory scopeFactory, IProtocolHandlerFactory protocolHandlerFactory) : base(scopeFactory, protocolHandlerFactory) { }

        [ProtocolHandler(Protocol.Academy_GetInfo)]
        public ResponsePacket GetInfoHandler(AcademyGetInfoRequest req)
        {
            return new AcademyGetInfoResponse()
            {
                AcademyDB = new()
                {
                    AccountId = 1
                },
                AcademyLocationDBs = [],
            };
        }
    }
}
