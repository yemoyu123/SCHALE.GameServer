using SCHALE.Common.NetworkProtocol;

namespace SCHALE.GameServer.Controllers.Api.ProtocolHandlers
{
    public class Mission : ProtocolHandlerBase
    {
        public Mission(IServiceScopeFactory scopeFactory, IProtocolHandlerFactory protocolHandlerFactory) : base(scopeFactory, protocolHandlerFactory) { }

        [ProtocolHandler(Protocol.Mission_List)]
        public ResponsePacket ListHandler(MissionListRequest req)
        {
            return new MissionListResponse()
            {

            };
        }

        [ProtocolHandler(Protocol.Mission_GuideMissionSeasonList)]
        public ResponsePacket GuideMissionSeasonListHandler(GuideMissionSeasonListRequest req)
        {
            return new GuideMissionSeasonListResponse()
            {

            };
        }
    }
}
