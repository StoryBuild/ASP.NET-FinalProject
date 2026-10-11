using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASP.NET_FinalProject.Models
{
    public class ResourcePack
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

        public int ResourceCategoryId { get; set; }
        [ForeignKey(nameof(ResourceCategoryId))]
        public ResourceCategory ResourceCategory { get; set; } = null!;

        public int ResourceFeatureId { get; set; }
        [ForeignKey(nameof(ResourceFeatureId))]
        public ResourceFeature ResourceFeature { get; set; } = null!;

        public int ResourceResolutionId { get; set; }
        [ForeignKey(nameof(ResourceResolutionId))]
        public ResourceResolution ResourceResolution { get; set; } = null!;

        public int GameVersionId { get; set; }
        [ForeignKey(nameof(GameVersionId))]
        public GameVersion GameVersion { get; set; } = null!;
    }
}
