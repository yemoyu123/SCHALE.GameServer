using SCHALE.Common.NetworkProtocol;
using Serilog;

namespace SCHALE.GameServer.Controllers.Api.ProtocolHandlers
{
    public class Scenario : ProtocolHandlerBase
    {
        public Scenario(IProtocolHandlerFactory protocolHandlerFactory) : base(protocolHandlerFactory) { }

        [ProtocolHandler(Protocol.Scenario_Skip)]
        public ResponsePacket SkipHandler(ScenarioSkipRequest req)
        {
            return new ScenarioSkipResponse();
        }

        [ProtocolHandler(Protocol.Scenario_Select)]
        public ResponsePacket SelectHandler(ScenarioSelectRequest req)
        {
            return new ScenarioSelectResponse();
        }

        [ProtocolHandler(Protocol.Scenario_GroupHistoryUpdate)]
        public ResponsePacket GroupHistoryUpdateHandler(ScenarioGroupHistoryUpdateRequest req)
        {
            return new ScenarioGroupHistoryUpdateResponse();
        }
    }
}
