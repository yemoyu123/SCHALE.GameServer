using SCHALE.Common.Database.Models;
namespace SCHALE.Common.NetworkProtocol.Account
{
    public class AccountAuthResponse : ResponsePacket
    {
        public override Protocol Protocol => Protocol.Account_Auth;
        public long CurrentVersion { get; set; }
        public long MinimumVersion { get; set; }
        public bool IsDevelopment { get; set; }
        public bool UpdateRequired { get; set; }
        public required string TTSCdnUri { get; set; }
        /*public AccountDB AccountDB { get; set; }
        public IEnumerable<AttendanceBookReward> AttendanceBookRewards { get; set; }
        public IEnumerable<AttendanceHistoryDB> AttendanceHistoryDBs { get; set; }
        public IEnumerable<OpenConditionDB> OpenConditions { get; set; }
        public IEnumerable<PurchaseCountDB> RepurchasableMonthlyProductCountDBs { get; set; }
        public IEnumerable<ParcelInfo> MonthlyProductParcel { get; set; }
        public IEnumerable<ParcelInfo> MonthlyProductMail { get; set; }
        public IEnumerable<ParcelInfo> BiweeklyProductParcel { get; set; }
        public IEnumerable<ParcelInfo> BiweeklyProductMail { get; set; }
        public IEnumerable<ParcelInfo> WeeklyProductParcel { get; set; }
        public IEnumerable<ParcelInfo> WeeklyProductMail { get; set; }*/
        public required string EncryptedUID { get; set; }
    }


    public class AccountAuthRequest : RequestPacket
    {
        public override Protocol Protocol => Protocol.Account_Auth;
        public long Version { get; set; }
        public string? DevId { get; set; }
        public long IMEI { get; set; }
        public string AccessIP { get; set; } = string.Empty;
        public string MarketId { get; set; } = string.Empty;
        public string? UserType { get; set; }
        public string? AdvertisementId { get; set; }
        public string OSType { get; set; } = string.Empty;
        public string OSVersion { get; set; } = string.Empty;
        public string DeviceUniqueId { get; set; } = string.Empty;
        public string DeviceModel { get; set; } = string.Empty;
        public int DeviceSystemMemorySize { get; set; }
    }
}
