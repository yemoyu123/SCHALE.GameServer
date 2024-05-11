using SCHALE.Common.Database;
using SCHALE.Common.Database.ModelExtensions;
using SCHALE.Common.NetworkProtocol;
using SCHALE.GameServer.Services;

namespace SCHALE.GameServer.Controllers.Api.ProtocolHandlers
{
    public class Character : ProtocolHandlerBase
    {
        private readonly ISessionKeyService sessionKeyService;
        private readonly SCHALEContext context;
        private readonly ExcelTableService excelTableService;

        public Character(IProtocolHandlerFactory protocolHandlerFactory, ISessionKeyService _sessionKeyService, SCHALEContext _context, ExcelTableService _excelTableService) : base(protocolHandlerFactory)
        {
            sessionKeyService = _sessionKeyService;
            context = _context;
            excelTableService = _excelTableService;
        }

        [ProtocolHandler(Protocol.Character_SetFavorites)]
        public ResponsePacket SetFavoritesHandler(CharacterSetFavoritesRequest req)
        {
            return new CharacterSetFavoritesResponse();
        }

        [ProtocolHandler(Protocol.Character_UnlockWeapon)]
        public ResponsePacket UnlockWeaponHandler(CharacterUnlockWeaponRequest req)
        {
            var account = sessionKeyService.GetAccount(req.SessionKey);
            var newWeapon = new WeaponDB()
            {
                UniqueId = account.Characters.FirstOrDefault(x => x.ServerId == req.TargetCharacterServerId).UniqueId,
                BoundCharacterServerId = req.TargetCharacterServerId,
                IsLocked = false,
                StarGrade = 1,
                Level = 1
            };

            account.AddWeapons(context, [newWeapon]);
            context.SaveChanges();

            return new CharacterUnlockWeaponResponse()
            {
                WeaponDB = newWeapon,
            };
        }

        [ProtocolHandler(Protocol.Character_PotentialGrowth)]
        public ResponsePacket PotentialGrowthHandler(CharacterPotentialGrowthRequest req)
        {
            var account = sessionKeyService.GetAccount(req.SessionKey);
            var targetCharacter = account.Characters.FirstOrDefault(x => x.ServerId == req.TargetCharacterDBId);

            foreach (var growthReq in req.PotentialGrowthRequestDBs)
            {
                targetCharacter.PotentialStats[(int)growthReq.Type] = growthReq.Level;
            }

            context.SaveChanges();

            return new CharacterPotentialGrowthResponse()
            {
                CharacterDB = targetCharacter
            };
        }
    }
}
