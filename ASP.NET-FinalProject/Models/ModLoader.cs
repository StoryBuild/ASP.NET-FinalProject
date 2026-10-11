using System.ComponentModel.DataAnnotations;

namespace ASP.NET_FinalProject.Models
{
    public class ModLoader
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; } = null!;
    }
}
