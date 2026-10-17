using System.ComponentModel.DataAnnotations;

namespace ASP.NET_FinalProject.Models
{
    public class DataPackCategory
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; } = null!;
    }
}
