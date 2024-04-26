using SCHALE.Common.NetworkProtocol;

namespace SCHALE.GameServer.Controllers.Api.ProtocolHandlers
{
    public class ProofToken : ProtocolHandlerBase
    {
        public ProofToken(IServiceScopeFactory scopeFactory, IProtocolHandlerFactory protocolHandlerFactory) : base(scopeFactory, protocolHandlerFactory) { }

        [ProtocolHandler(Protocol.ProofToken_RequestQuestion)]
        public ResponsePacket RequestQuestionHandler(ProofTokenRequestQuestionRequest req)
        {
            return new ProofTokenRequestQuestionResponse()
            {
                Hint = 69,
                Question = "seggs",
                SessionKey = new()
                {
                    MxToken = req.SessionKey.MxToken,
                    AccountServerId = req.SessionKey.AccountServerId,
                },
            };
        }

        [ProtocolHandler(Protocol.ProofToken_Submit)]
        public ResponsePacket SubmitHandler(ProofTokenSubmitRequest req)
        {
            return new ProofTokenSubmitResponse()
            {
                SessionKey = new()
                {
                    MxToken = req.SessionKey.MxToken,
                    AccountServerId = req.SessionKey.AccountServerId,
                },
            };
        }
    }
}
