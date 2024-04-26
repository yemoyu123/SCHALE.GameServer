using SCHALE.Common.Database;
using SCHALE.Common.NetworkProtocol;

namespace SCHALE.GameServer.Controllers.Api.ProtocolHandlers
{
    public class Academy : ProtocolHandlerBase
    {
        public Academy(IServiceScopeFactory scopeFactory, IProtocolHandlerFactory protocolHandlerFactory) : base(scopeFactory, protocolHandlerFactory) { }

        [ProtocolHandler(Protocol.Academy_GetInfo)]
        public ResponsePacket GetInfoHandler(AcademyGetInfoRequest req)
        {
            //Context.CurrentPlayer.MissionProgressDBs
            var MissionProgressDBs = new List<MissionProgressDB> { 
                new MissionProgressDB() {
                    MissionUniqueId = 1700,
                    Complete = false,
                    StartTime = DateTime.UtcNow,
                    ProgressParameters = new Dictionary<long, long>
                    {
                        { 0, 2 }
                    }
                }
            };

            return new AcademyGetInfoResponse()
            {
                AcademyDB = new()
                {
                    AccountId = Context.CurrentPlayer.ServerId,
                },
                AcademyLocationDBs = [],
                MissionProgressDBs = MissionProgressDBs,
                //MissionProgressDBs = Context.CurrentPlayer.MissionProgressDBs,
            };
        }
    }
}
