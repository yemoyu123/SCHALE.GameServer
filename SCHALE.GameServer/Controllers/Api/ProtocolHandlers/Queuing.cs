using SCHALE.Common.NetworkProtocol;

namespace SCHALE.GameServer.Controllers.Api.ProtocolHandlers
{
    public class Queuing : ProtocolHandlerBase
    {
        public Queuing(IServiceScopeFactory scopeFactory, IProtocolHandlerFactory protocolHandlerFactory) : base(scopeFactory, protocolHandlerFactory) { }

        [ProtocolHandler(Protocol.Queuing_GetTicket)]
        public ResponsePacket GetTicketHandler(QueuingGetTicketRequest req)
        {
            return new QueuingGetTicketResponse()
            {
                EnterTicket = $"{req.YostarUID}:{req.YostarToken}"
            };
        }
    }
}
