using System.ComponentModel.DataAnnotations;

namespace HawassaUnifiedCampusEventManagementSystem.Models
{
    public class CommunityUser
    {
        public ulong Id { get; set; }

        [Required]
        public string FullName { get; set; } = string.Empty;

        public string? Username { get; set; }

        public string? Department { get; set; }

        public string? Bio { get; set; }

        public string? ProfileImage { get; set; }

        public int Followers { get; set; }

        public int Following { get; set; }

        public bool IsFollowing { get; set; }
    }
}