using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SCHALE.Common.Database.Models.Game
{
    public class Player
    {
        [Key]
        [Column("_id")]
        public required uint ServerId { get; set; }

        public List<MissionProgressDB> MissionProgressDBs { get; set; }
    }
}
