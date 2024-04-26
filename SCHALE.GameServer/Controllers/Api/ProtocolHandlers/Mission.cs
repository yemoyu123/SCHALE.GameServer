using SCHALE.Common.Database;
using SCHALE.Common.NetworkProtocol;
using Serilog;

namespace SCHALE.GameServer.Controllers.Api.ProtocolHandlers
{
    public class Mission : ProtocolHandlerBase
    {
        public Mission(IServiceScopeFactory scopeFactory, IProtocolHandlerFactory protocolHandlerFactory) : base(scopeFactory, protocolHandlerFactory) { }

        [ProtocolHandler(Protocol.Mission_List)]
        public ResponsePacket ListHandler(MissionListRequest req)
        {
            var player = Context.Players.SingleOrDefault(x => x.ServerId == req.AccountId);

            Log.Information($"MissionListRequest EventContentId: {req.EventContentId}");

            Context.CurrentPlayer.MissionProgressDBs = new List<MissionProgressDB>
            {
                new MissionProgressDB
                {
                    MissionUniqueId = 1501,
                    Complete = true,
                    StartTime = DateTime.Now    },
                new MissionProgressDB
                {
                    MissionUniqueId = 1700,
                    StartTime = DateTime.Now,
                    ProgressParameters = new Dictionary<long, long> { { 0, 2 } }
                },
                new MissionProgressDB
                {
                    MissionUniqueId = 1500,
                    Complete = true,
                    StartTime = DateTime.Now    },
                new MissionProgressDB
                {
                    MissionUniqueId = 2200,
                    StartTime = DateTime.Now,
                    ProgressParameters = new Dictionary<long, long> { { 0, 1 }, { 1, 5 } }
                },
                new MissionProgressDB
                {
                    MissionUniqueId = 300000,
                    StartTime = DateTime.Now,
                    ProgressParameters = new Dictionary<long, long> { { 0, 1 } }
                },
                new MissionProgressDB
                {
                    MissionUniqueId = 1000210,
                    Complete = true,
                    StartTime = DateTime.Now,
                    ProgressParameters = new Dictionary<long, long> { { 0, 1 } }
                },
                new MissionProgressDB
                {
                    MissionUniqueId = 1000220,
                    Complete = true,
                    StartTime = DateTime.Now,
                    ProgressParameters = new Dictionary<long, long> { { 0, 1 } }
                },
                new MissionProgressDB
                {
                    MissionUniqueId = 1000230,
                    Complete = true,
                    StartTime = DateTime.Now,
                    ProgressParameters = new Dictionary<long, long> { { 0, 1 } }
                },
                new MissionProgressDB
                {
                    MissionUniqueId = 1000240,
                    Complete = true,
                    StartTime = DateTime.Now,
                    ProgressParameters = new Dictionary<long, long> { { 0, 1 } }
                },
                new MissionProgressDB
                {
                    MissionUniqueId = 1000250,
                    Complete = true,
                    StartTime = DateTime.Now,
                    ProgressParameters = new Dictionary<long, long> { { 0, 1 } }
                },
                new MissionProgressDB
                {
                    MissionUniqueId = 1000260,
                    Complete = true,
                    StartTime = DateTime.Now,
                    ProgressParameters = new Dictionary<long, long> { { 0, 1 } }
                },
                new MissionProgressDB
                {
                    MissionUniqueId = 1000270,
                    Complete = true,
                    StartTime = DateTime.Now,
                    ProgressParameters = new Dictionary<long, long> { { 0, 1 } }
                },
                new MissionProgressDB
                {
                    MissionUniqueId = 1001327,
                    Complete = true,
                    StartTime = DateTime.Now,
                    ProgressParameters = new Dictionary<long, long> { { 0, 1 } }
                },
                new MissionProgressDB
                {
                    MissionUniqueId = 1001357,
                    Complete = true,
                    StartTime = DateTime.Now,
                    ProgressParameters = new Dictionary<long, long> { { 0, 1 } }
                },
                new MissionProgressDB
                {
                    MissionUniqueId = 1001377,
                    Complete = true,
                    StartTime = DateTime.Now,
                    ProgressParameters = new Dictionary<long, long> { { 0, 1 } }
                },
                new MissionProgressDB
                {
                    MissionUniqueId = 1001011,
                    StartTime = DateTime.Now,
                    ProgressParameters = new Dictionary<long, long> { { 0, 1 } }
                },
                new MissionProgressDB
                {
                    MissionUniqueId = 1001014,
                    StartTime = DateTime.Now,
                    ProgressParameters = new Dictionary<long, long> { { 0, 0 } }
                },
                new MissionProgressDB
                {
                    MissionUniqueId = 1001015,
                    StartTime = DateTime.Now,
                    ProgressParameters = new Dictionary<long, long> { { 13003, 1 }, { 13010, 1 }, { 16003, 1 }, { 26000, 1 } }
                },
                new MissionProgressDB
                {
                    MissionUniqueId = 1001016,
                    StartTime = DateTime.Now,
                    ProgressParameters = new Dictionary<long, long> { { 0, 0 } }
                },
                new MissionProgressDB
                {
                    MissionUniqueId = 1001019,
                    StartTime = DateTime.Now,
                    ProgressParameters = new Dictionary<long, long> { { 0, 4 } }
                },
                new MissionProgressDB
                {
                    MissionUniqueId = 1001020,
                    Complete = true,
                    StartTime = DateTime.Now,
                    ProgressParameters = new Dictionary<long, long> { { 0, 1 } }
                },
                new MissionProgressDB
                {
                    MissionUniqueId = 1001021,
                    StartTime = DateTime.Now,
                    ProgressParameters = new Dictionary<long, long> { { 0, 1 } }
                },
                new MissionProgressDB
                {
                    MissionUniqueId = 1001025,
                    StartTime = DateTime.Now,
                    ProgressParameters = new Dictionary<long, long> { { 13003, 1 }, { 13010, 1 }, { 16003, 1 }, { 26000, 1 } }
                },
                new MissionProgressDB
                {
                    MissionUniqueId = 1001026,
                    StartTime = DateTime.Now,
                    ProgressParameters = new Dictionary<long, long> { { 0, 0 } }
                },
                new MissionProgressDB
                {
                    MissionUniqueId = 1001028,
                    StartTime = DateTime.Now,
                    ProgressParameters = new Dictionary<long, long> { { 0, 0 } }
                },
                new MissionProgressDB
                {
                    MissionUniqueId = 1001030,
                    StartTime = DateTime.Now,
                    ProgressParameters = new Dictionary<long, long> { { 1, 0 } }
                },
                new MissionProgressDB
                {
                    MissionUniqueId = 1001031,
                    StartTime = DateTime.Now,
                    ProgressParameters = new Dictionary<long, long> { { 0, 1 } }
                },
                new MissionProgressDB
                {
                    MissionUniqueId = 1001036,
                    StartTime = DateTime.Now,
                    ProgressParameters = new Dictionary<long, long> { { 13003, 1 }, { 13010, 1 }, { 16003, 1 }, { 26000, 1 } }
                },
                new MissionProgressDB
                {
                    MissionUniqueId = 1001037,
                    StartTime = DateTime.Now,
                    ProgressParameters = new Dictionary<long, long> { { 0, 0 } }
                },
                new MissionProgressDB
                {
                    MissionUniqueId = 1001039,
                    StartTime = DateTime.Now,
                    ProgressParameters = new Dictionary<long, long> { { 0, 1 } }
                },
                new MissionProgressDB
                {
                    MissionUniqueId = 1001041,
                    StartTime = DateTime.Now,
                    ProgressParameters = new Dictionary<long, long> { { 0, 1 } }
                },
                new MissionProgressDB
                {
                    MissionUniqueId = 1001046,
                    StartTime = DateTime.Now,
                    ProgressParameters = new Dictionary<long, long> { { 13003, 1 }, { 13010, 1 }, { 16003, 1 }, { 26000, 1 } }
                },
                new MissionProgressDB
                {
                    MissionUniqueId = 1001050,
                    StartTime = DateTime.Now,
                    ProgressParameters = new Dictionary<long, long> { { 0, 0 } }
                },
                new MissionProgressDB
                {
                    MissionUniqueId = 1001051,
                    StartTime = DateTime.Now,
                    ProgressParameters = new Dictionary<long, long> { { 0, 1 } }
                },
                new MissionProgressDB
                {
                    MissionUniqueId = 1001055,
                    StartTime = DateTime.Now,
                    ProgressParameters = new Dictionary<long, long> { { 1, 0 } }
                },
                new MissionProgressDB
                {
                    MissionUniqueId = 1001056,
                    StartTime = DateTime.Now,
                    ProgressParameters = new Dictionary<long, long> { { 13003, 1 }, { 13010, 1 }, { 16003, 1 }, { 26000, 1 } }
                },
                new MissionProgressDB
                {
                    MissionUniqueId = 1001057,
                    StartTime = DateTime.Now,
                    ProgressParameters = new Dictionary<long, long> { { 0, 0 } }
                },
                new MissionProgressDB
                {
                    MissionUniqueId = 1001059,
                    StartTime = DateTime.Now,
                    ProgressParameters = new Dictionary<long, long> { { 0, 0 } }
                },
                new MissionProgressDB
                {
                    MissionUniqueId = 1001060,
                    StartTime = DateTime.Now,
                    ProgressParameters = new Dictionary<long, long> { { 0, 0 } }
                },
                new MissionProgressDB
                {
                    MissionUniqueId = 1001061,
                    StartTime = DateTime.Now,
                    ProgressParameters = new Dictionary<long, long> { { 0, 1 } }
                },
                new MissionProgressDB
                {
                    MissionUniqueId = 1001065,
                    StartTime = DateTime.Now,
                    ProgressParameters = new Dictionary<long, long> { { 13003, 1 }, { 13010, 1 }, { 16003, 1 }, { 26000, 1 } }
                },
                new MissionProgressDB
                {
                    MissionUniqueId = 1001067,
                    StartTime = DateTime.Now,
                    ProgressParameters = new Dictionary<long, long> { { 0, 0 } }
                },
                new MissionProgressDB
                {
                    MissionUniqueId = 1001069,
                    StartTime = DateTime.Now,
                    ProgressParameters = new Dictionary<long, long> { { 0, 0 } }
                },
                new MissionProgressDB
                {
                    MissionUniqueId = 1001070,
                    StartTime = DateTime.Now,
                    ProgressParameters = new Dictionary<long, long> { { 0, 0 } }
                },
                new MissionProgressDB
                {
                    MissionUniqueId = 1001071,
                    StartTime = DateTime.Now,
                    ProgressParameters = new Dictionary<long, long> { { 0, 1 } }
                },
                new MissionProgressDB
                {
                    MissionUniqueId = 1001075,
                    StartTime = DateTime.Now,
                    ProgressParameters = new Dictionary<long, long> { { 13003, 1 }, { 13010, 1 }, { 16003, 1 }, { 26000, 1 } }
                },
                new MissionProgressDB
                {
                    MissionUniqueId = 1001079,
                    StartTime = DateTime.Now,
                    ProgressParameters = new Dictionary<long, long> { { 7, 0 } }
                },
                new MissionProgressDB
                {
                    MissionUniqueId = 1001080,
                    StartTime = DateTime.Now,
                    ProgressParameters = new Dictionary<long, long> { { 0, 0 } }
                },
                new MissionProgressDB
                {
                    MissionUniqueId = 1001013,
                    StartTime = DateTime.Now    },
                new MissionProgressDB
                {
                    MissionUniqueId = 1001023,
                    StartTime = DateTime.Now    },
                new MissionProgressDB
                {
                    MissionUniqueId = 1001034,
                    StartTime = DateTime.Now    },
                new MissionProgressDB
                {
                    MissionUniqueId = 1001044,
                    StartTime = DateTime.Now    },
                new MissionProgressDB
                {
                    MissionUniqueId = 1001054,
                    StartTime = DateTime.Now    },
            };

            //Context.CurrentPlayer.MissionProgressDBs = [];
            Context.SaveChanges();

            return new MissionListResponse
            {
                //ProgressDBs = Context.CurrentPlayer.MissionProgressDBs
                ProgressDBs = Context.CurrentPlayer.MissionProgressDBs
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
