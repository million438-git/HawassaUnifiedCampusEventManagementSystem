using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HawassaUnifiedCampusEventManagementSystem.Models
{
    [Table("user_relationships")]
    [Index("follower_user_id", "followed_user_id", Name = "uq_user_relationships_pair", IsUnique = true)]
    [Index("followed_user_id", Name = "idx_user_relationships_followed")]
    public partial class user_relationship
    {
        [Key]
        public ulong id { get; set; }

        public ulong follower_user_id { get; set; }

        public ulong followed_user_id { get; set; }

        public DateTime created_at { get; set; } = DateTime.UtcNow;

        [ForeignKey("follower_user_id")]
        public virtual User? follower_user { get; set; }

        [ForeignKey("followed_user_id")]
        public virtual User? followed_user { get; set; }
    }
}
