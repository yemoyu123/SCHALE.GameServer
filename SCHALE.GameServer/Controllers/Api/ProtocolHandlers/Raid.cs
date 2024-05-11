using SCHALE.Common.Database;
using SCHALE.Common.NetworkProtocol;
using SCHALE.GameServer.Services;

namespace SCHALE.GameServer.Controllers.Api.ProtocolHandlers
{
    public class Raid : ProtocolHandlerBase
    {
        private readonly ISessionKeyService sessionKeyService;
        private readonly SCHALEContext context;
        private readonly ExcelTableService excelTableService;

        public Raid(IProtocolHandlerFactory protocolHandlerFactory, ISessionKeyService _sessionKeyService, SCHALEContext _context, ExcelTableService _excelTableService) : base(protocolHandlerFactory)
        {
            sessionKeyService = _sessionKeyService;
            context = _context;
            excelTableService = _excelTableService;
        }

        [ProtocolHandler(Protocol.Raid_Lobby)]
        public ResponsePacket LobbyHandler(RaidLobbyRequest req)
        {
            return new RaidLobbyResponse()
            {

            };
        }

        [ProtocolHandler(Protocol.Raid_OpponentList)]
        public ResponsePacket OpponentListHandler(RaidOpponentListRequest req)
        {
            return new RaidOpponentListResponse()
            {
                OpponentUserDBs = []
            };
        }

        [ProtocolHandler(Protocol.Raid_CreateBattle)]
        public ResponsePacket CreateBattleHandller(RaidCreateBattleRequest req)
        {
            return new RaidCreateBattleResponse()
            {

            };
        }

        [ProtocolHandler(Protocol.Raid_EnterBattle)] // clicked restart
        public ResponsePacket EnterBattleHandler(RaidEnterBattleRequest req)
        {
            return new RaidEnterBattleResponse()
            {

            };
        }

        [ProtocolHandler(Protocol.Raid_EndBattle)]
        public ResponsePacket EndBattleHandler(RaidEndBattleRequest req)
        {
            return new RaidEndBattleResponse()
            {

            };
        }
    }
}
