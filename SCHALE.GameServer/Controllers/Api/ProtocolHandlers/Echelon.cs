using SCHALE.Common.Database;
using SCHALE.Common.Database.ModelExtensions;
using SCHALE.Common.FlatData;
using SCHALE.Common.NetworkProtocol;
using SCHALE.GameServer.Services;

namespace SCHALE.GameServer.Controllers.Api.ProtocolHandlers
{
    public class Echelon : ProtocolHandlerBase
    {
        private readonly ISessionKeyService sessionKeyService;
        private readonly SCHALEContext context;
        private readonly ExcelTableService excelTableService;

        public Echelon(IProtocolHandlerFactory protocolHandlerFactory, ISessionKeyService _sessionKeyService, SCHALEContext _context, ExcelTableService _excelTableService) : base(protocolHandlerFactory)
        {
            sessionKeyService = _sessionKeyService;
            context = _context;
            excelTableService = _excelTableService;
        }

        [ProtocolHandler(Protocol.Echelon_List)]
        public ResponsePacket ListHandler(EchelonListRequest req)
        {
            var account = sessionKeyService.GetAccount(req.SessionKey);

            return new EchelonListResponse()
            {
                EchelonDBs = account.Echelons.ToList()
            };
        }

        [ProtocolHandler(Protocol.Echelon_Save)]
        public ResponsePacket SaveHandler(EchelonSaveRequest req)
        {
            return new EchelonSaveResponse()
            {
                EchelonDB = req.EchelonDB,
            };
        }
    }
}
