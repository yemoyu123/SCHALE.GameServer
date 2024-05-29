using AutoMapper;
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
        IMapper _mapper
    ) : ProtocolHandlerBase(protocolHandlerFactory)
    {
        private readonly ISessionKeyService sessionKeyService = _sessionKeyService;
        private readonly SCHALEContext context = _context;
        private readonly ExcelTableService excelTableService = _excelTableService;

        private readonly IMapper mapper = _mapper;

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
            var old = context.Echelons.FirstOrDefault(e =>
                e.AccountServerId == db.AccountServerId
                && e.EchelonType == db.EchelonType
                && e.EchelonNumber == db.EchelonNumber
                && e.ExtensionType == db.ExtensionType
            );
            if (old == null)
                context.Echelons.Add(db);
            else
            {
                db.ServerId = old.ServerId;
                mapper.Map(db, old);
            }
            context.SaveChanges();
            return new EchelonSaveResponse() { EchelonDB = db, };
        }
    }
}
