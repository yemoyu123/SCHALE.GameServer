using SCHALE.Common.NetworkProtocol;

namespace SCHALE.GameServer.Controllers.Api.ProtocolHandlers
{
    public class Scenario : ProtocolHandlerBase
    {
        public Scenario(IServiceScopeFactory scopeFactory, IProtocolHandlerFactory protocolHandlerFactory) : base(scopeFactory, protocolHandlerFactory) { }

        [ProtocolHandler(Protocol.Scenario_Skip)]
        public ResponsePacket SkipHandler(ScenarioSkipRequest req)
        {
            // skip story doesn't work yet, probably need to implement missiondb 
            return new ScenarioSkipResponse()
            {

            };
        }

        [ProtocolHandler(Protocol.Scenario_Select)]
        public ResponsePacket SelectHandler(ScenarioSelectRequest req)
        {

            return new ScenarioSelectResponse()
            {

            };
        }


    }
}
