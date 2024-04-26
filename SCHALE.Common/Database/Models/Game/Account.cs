using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using SCHALE.Common.FlatData;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.Core.Servers;

namespace SCHALE.Common.Database.Models.Game
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public class Account
    {
        [Key]
        [Column("_id")]
        public uint ServerId { get; set; }

        public AccountDB AccountDB { get; set; }

        public static Account Create(uint guest_account_uid) // make sure ServerId matches GuestAccount UID
        {
            Account account = new()
            {
                ServerId = guest_account_uid,
                AccountDB = new AccountDB()
                {
                    ServerId = guest_account_uid,
                    State = AccountState.Normal,
                    Level = 0,
                    Exp = 0,
                    RepresentCharacterServerId = 1037810385, // i think this is the default
                    LastConnectTime = DateTime.Now,
                    CreateDate = DateTime.Now,
                }
            };

            return account;
        }

    }
}
