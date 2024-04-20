using SCHALE.Common.NetworkProtocol;
using SCHALE.Common.NetworkProtocol.Queuing;

namespace SCHALE.GameServer.Controllers.Api.ProtocolHandlers
{
    public static class Queuing
    {
        [ProtocolHandler(Protocol.Queuing_GetTicket)]
        public static ResponsePacket GetTicketHandler(QueuingGetTicketRequest req)
        {
            return new QueuingGetTicketResponse()
            {
                EnterTicket = req.YostarToken
            };
        }
    }
}
