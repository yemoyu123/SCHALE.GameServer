using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SCHALE.Common.Database.Models.Game
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public class Account : AccountDB
    {
        [Key]
        [Column("_id")]
        public new uint ServerId { get; set; }
    }
}
