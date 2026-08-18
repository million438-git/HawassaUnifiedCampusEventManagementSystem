using System.ComponentModel.DataAnnotations;

namespace HawassaUnifiedCampusEventManagementSystem.Models
{
    public class CommunityPost
    {
        public int Id { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;

        public string AuthorName { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int Likes { get; set; }

        public int Comments { get; set; }
    }
}