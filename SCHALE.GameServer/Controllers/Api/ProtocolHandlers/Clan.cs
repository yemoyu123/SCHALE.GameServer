using SCHALE.Common.NetworkProtocol;

namespace SCHALE.GameServer.Controllers.Api.ProtocolHandlers
{
    public class Clan : ProtocolHandlerBase
    {
        public Clan(IProtocolHandlerFactory protocolHandlerFactory) : base(protocolHandlerFactory) { }

        [ProtocolHandler(Protocol.Clan_Check)]
        public ResponsePacket CheckHandler(ClanCheckRequest req)
        {

            return new ClanCheckResponse()
            {

            };
        }
    }
}
