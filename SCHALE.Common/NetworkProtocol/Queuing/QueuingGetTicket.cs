namespace SCHALE.Common.NetworkProtocol.Queuing
{
    public class QueuingGetTicketRequest : RequestPacket
    {
        public override Protocol Protocol => Protocol.Queuing_GetTicket;
        public long YostarUID { get; set; }
        public required string YostarToken { get; set; }
        public bool MakeStandby { get; set; }
        public bool PassCheck { get; set; }
        public bool PassCheckYostar { get; set; }
        public string WaitingTicket { get; set; } = string.Empty;
        public string ClientVersion { get; set; } = "0.0.0";
    }

    public class QueuingGetTicketResponse : ResponsePacket
    {
        public override Protocol Protocol => Protocol.Queuing_GetTicket;
        public string WaitingTicket { get; set; } = string.Empty;
        public string EnterTicket { get; set; } = string.Empty;
        public long TicketSequence { get; set; }
        public long AllowedSequence { get; set; }
        public double RequiredSecondsPerUser { get; set; }
        public string Birth { get; set; } = string.Empty;
        public string ServerSeed { get; set; } = string.Empty;
    }
}
