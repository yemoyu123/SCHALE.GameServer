using Microsoft.EntityFrameworkCore;
using SCHALE.Common.Database;
using SCHALE.Common.Database.ModelExtensions;
using SCHALE.Common.FlatData;
using SCHALE.Common.NetworkProtocol;
using SCHALE.GameServer.Services;

namespace SCHALE.GameServer.Controllers.Api.ProtocolHandlers
{
    public class Echelon(
        IProtocolHandlerFactory protocolHandlerFactory,
        ISessionKeyService _sessionKeyService,
        SCHALEContext _context,
        ExcelTableService _excelTableService,
        ILogger<Echelon> _logger
    ) : ProtocolHandlerBase(protocolHandlerFactory)
    {
        private readonly ISessionKeyService sessionKeyService = _sessionKeyService;
        private readonly SCHALEContext context = _context;
        private readonly ExcelTableService excelTableService = _excelTableService;
        private readonly ILogger<Echelon> logger = _logger;

        [ProtocolHandler(Protocol.Echelon_List)]
        public ResponsePacket ListHandler(EchelonListRequest req)
        {
            var account = sessionKeyService.GetAccount(req.SessionKey);

            return new EchelonListResponse() { EchelonDBs = [.. account.Echelons] };
        }

        [ProtocolHandler(Protocol.Echelon_Save)]
        public ResponsePacket SaveHandler(EchelonSaveRequest req)
        {
            var db = req.EchelonDB;
            
            context.Echelons.Add(db);
            context.SaveChanges();
            
            return new EchelonSaveResponse() { EchelonDB = req.EchelonDB, };
        }
    }
}
