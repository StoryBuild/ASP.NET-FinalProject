using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASP.NET_FinalProject.Models
{
    public class DataPack
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

        public int Downloads { get; set; }

        public int DataPackCategoryId { get; set; }
        [ForeignKey(nameof(DataPackCategoryId))]
        public DataPackCategory DataPackCategory { get; set; } = null!;

        public int GameVersionId { get; set; }
        [ForeignKey(nameof(GameVersionId))]
        public GameVersion GameVersion { get; set; } = null!;
    }
}
