using System;
using System.ComponentModel.DataAnnotations;

namespace TheatreCMS3.Areas.Prod.Models
{
    public enum PositionEnum
    {
        Actor,
        Director,
        Technician,
        StageManager,
        Other
    }

    public class CastMember
    {
        [Key]
        public int CastMemberID { get; set; }

        [Required]
        public string Name { get; set; }
        public int? YearJoined { get; set; }
        public PositionEnum MainRole { get; set; }

        [Required]
        public string Bio { get; set; }
        public byte[] Photo { get; set; }
        public bool CurrentMember { get; set; }

        [Required]
        public string Character { get; set; }
        public int? CastYearLeft { get; set; }
        public int? DebutYear { get; set; }
    }
}
