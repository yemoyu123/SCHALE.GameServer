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
            Log.Information($"ScenarioSkipRequest ScriptGroupId:" + req.ScriptGroupId);
            Log.Information($"ScenarioSkipRequest SkipPointScriptCount: " + req.SkipPointScriptCount);

            // skip story doesn't work yet, probably need to implement missiondb 
            return new ScenarioSkipResponse();
        }

        [ProtocolHandler(Protocol.Scenario_Select)]
        public ResponsePacket SelectHandler(ScenarioSelectRequest req)
        {
            return new ScenarioSelectResponse();
        }
    }
}
