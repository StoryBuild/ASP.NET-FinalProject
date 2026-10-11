using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASP.NET_FinalProject.Models
{
    public class ModProject
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Title { get; set; } = null!;

        [Required, MaxLength(1000)]
        public string Description { get; set; } = null!;

        [Required, MaxLength(50)]
        public string Author { get; set; } = null!;

        [Url]
        public string? ImageUrl { get; set; }

        [Required, Url]
        public string DownloadUrl { get; set; } = null!;

        [Required, MaxLength(20)]
        public string Environment { get; set; } = null!;

        public int Downloads { get; set; }

        public int CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; } = null!;

        public int GameVersionId { get; set; }
        [ForeignKey(nameof(GameVersionId))]
        public GameVersion GameVersion { get; set; } = null!;

        public int ModLoaderId { get; set; }
        [ForeignKey(nameof(ModLoaderId))]
        public ModLoader ModLoader { get; set; } = null!;
    }
}
