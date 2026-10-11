using System.ComponentModel.DataAnnotations;

namespace ASP.NET_FinalProject.Models
{
    public class GameVersion
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string VersionNumber { get; set; } = null!;
    }
}
