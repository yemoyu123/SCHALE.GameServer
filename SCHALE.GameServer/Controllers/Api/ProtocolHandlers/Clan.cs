using SCHALE.Common.NetworkProtocol;

namespace SCHALE.GameServer.Controllers.Api.ProtocolHandlers
{
    public class Clan : ProtocolHandlerBase
    {
        public Clan(IProtocolHandlerFactory protocolHandlerFactory) : base(protocolHandlerFactory) { }

        [ProtocolHandler(Protocol.Clan_Check)]
        public ResponsePacket CheckHandler(ClanCheckRequest req)
        {
            return new ClanCheckResponse();
        }

        [ProtocolHandler(Protocol.Clan_AllAssistList)]
        public ResponsePacket AllAssistListHandler(ClanAllAssistListRequest req)
        {
            return new ClanAllAssistListResponse()
            {
                AssistCharacterDBs = [
                    new() {
                        AccountId = 1,
                        AssistCharacterServerId = 1,
                        EchelonType = Common.FlatData.EchelonType.Raid,
                        AssistRelation = Common.Database.AssistRelation.Friend,
                        EquipmentDBs = [],
                        WeaponDB = new(),
                        NickName = "Arona",
                        UniqueId = 20024,
                    }
                ],
                AssistCharacterRentHistoryDBs = []
            };
        }

        
    }
}
