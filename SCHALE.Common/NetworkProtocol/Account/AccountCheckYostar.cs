namespace SCHALE.Common.NetworkProtocol.Account
{
    public class AccountCheckYostarRequest : RequestPacket
    {
        public override Protocol Protocol => NetworkProtocol.Protocol.Account_CheckYostar;
        public long UID { get; set; }
        public string YostarToken { get; set; } = string.Empty;
        public string EnterTicket { get; set; } = string.Empty;
        public bool PassCookieResult { get; set; }
        public string Cookie { get; set; } = string.Empty;
    }


    public class AccountCheckYostarResponse : ResponsePacket
    {
        public override Protocol Protocol
        {
            get
            {
                return NetworkProtocol.Protocol.Account_CheckYostar;
            }
        }
        public int ResultState { get; set; }
        public string ResultMessag { get; set; } = string.Empty;
        public string Birth { get; set; } = string.Empty;
    }
}
