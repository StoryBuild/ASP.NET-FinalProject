using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASP.NET_FinalProject.Models
{
    public class ResourcePack
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Title must be between 2 and 100 characters.")]
        public string Title { get; set; } = null!;

        [Required]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "Description must be between 10 and 1000 characters.")]
        public string Description { get; set; } = null!;

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Author must be between 2 and 50 characters.")]
        public string Author { get; set; } = null!;

        [Url(ErrorMessage = "Please enter a valid image URL.")]
        public string? ImageUrl { get; set; }

        [Required]
        [Url(ErrorMessage = "Please enter a valid download URL.")]
        public string DownloadUrl { get; set; } = null!;

        [Range(0, int.MaxValue, ErrorMessage = "Downloads count cannot be negative.")]
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