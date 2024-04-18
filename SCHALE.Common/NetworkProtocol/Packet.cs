namespace SCHALE.Common.NetworkProtocol
{
    public class SessionKey
    {
        public long AccountServerId { get; set; }

        public required string MxToken { get; set; }
    }

    public abstract class BasePacket
    {
        public SessionKey? SessionKey { get; set; }

        public abstract Protocol Protocol { get; }

        public long AccountId => SessionKey?.AccountServerId ?? 0;
    }

    public abstract class RequestPacket : BasePacket
    {
	    private static int _counter;

        public int ClientUpTime { get; set; }

        public bool Resendable { get; set; } = true;


        public long Hash { get; set; }

        public bool IsTest { get; set; }

        public DateTime? ModifiedServerTime__DebugOnly { get; set; }

        public static long CreateHash(Protocol protocol)
        {
            return _counter++ | ((int)protocol << 32);
        }
    }

    public abstract class ResponsePacket : BasePacket
    {
        public long ServerTimeTicks { get; set; }

        public ServerNotificationFlag ServerNotification { get; set; }

        // public List<MissionProgressDB> MissionProgressDBs { get; set; }

        // public Dictionary<long, List<MissionProgressDB>> EventMissionProgressDBDict { get; set; }

        // public Dictionary<OpenConditionContent, OpenConditionLockReason> StaticOpenConditions { get; set; }
    }

    [Flags]
    public enum ServerNotificationFlag
    {
        // Token: 0x04009560 RID: 38240
        None = 0,
        // Token: 0x04009561 RID: 38241
        NewMailArrived = 4,
        // Token: 0x04009562 RID: 38242
        HasUnreadMail = 8,
        // Token: 0x04009563 RID: 38243
        NewToastDetected = 16,
        // Token: 0x04009564 RID: 38244
        CanReceiveArenaDailyReward = 32,
        // Token: 0x04009565 RID: 38245
        CanReceiveRaidReward = 64,
        // Token: 0x04009566 RID: 38246
        ServerMaintenance = 256,
        // Token: 0x04009567 RID: 38247
        CannotReceiveMail = 512,
        // Token: 0x04009568 RID: 38248
        InventoryFullRewardMail = 1024,
        // Token: 0x04009569 RID: 38249
        CanReceiveClanAttendanceReward = 2048,
        // Token: 0x0400956A RID: 38250
        HasClanApplicant = 4096,
        // Token: 0x0400956B RID: 38251
        HasFriendRequest = 8192,
        // Token: 0x0400956C RID: 38252
        CheckConquest = 16384,
        // Token: 0x0400956D RID: 38253
        CanReceiveEliminateRaidReward = 32768,
        // Token: 0x0400956E RID: 38254
        CanReceiveMultiFloorRaidReward = 65536
    }
}
