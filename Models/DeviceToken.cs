using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HawassaUnifiedCampusEventManagementSystem.Models;

[Table("device_tokens")]
[Index("user_id", Name = "idx_device_tokens_user")]
[Index("token", Name = "uq_device_tokens_token", IsUnique = true)]
public partial class device_token
{
    [Key]
    public ulong id { get; set; }

    public ulong user_id { get; set; }

    [StringLength(1000)]
    public string token { get; set; } = null!;

    [Column(TypeName = "enum('WEB','ANDROID','IOS','DESKTOP','OTHER')")]
    public string platform { get; set; } = null!;

    [StringLength(255)]
    public string? device_name { get; set; }

    [Required]
    public bool? is_active { get; set; }

    [MaxLength(6)]
    public DateTime? last_used_at { get; set; }

    [MaxLength(6)]
    public DateTime created_at { get; set; }

    [MaxLength(6)]
    public DateTime updated_at { get; set; }

    [ForeignKey("user_id")]
    [InverseProperty("device_tokens")]
    public virtual User user { get; set; } = null!;
}
