using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HawassaUnifiedCampusEventManagementSystem.Models;

[Table("calendar_syncs")]
[Index("user_id", "provider", Name = "uq_calendar_sync_user_provider", IsUnique = true)]
public partial class calendar_sync
{
    [Key]
    public ulong id { get; set; }

    public ulong user_id { get; set; }

    [Column(TypeName = "enum('GOOGLE','APPLE','OUTLOOK')")]
    public string provider { get; set; } = null!;

    [StringLength(255)]
    public string? provider_account_id { get; set; }

    [Column(TypeName = "text")]
    public string? access_token_encrypted { get; set; }

    [Column(TypeName = "text")]
    public string? refresh_token_encrypted { get; set; }

    [StringLength(500)]
    public string? calendar_id { get; set; }

    [Required]
    public bool? sync_enabled { get; set; }

    [MaxLength(6)]
    public DateTime? last_synced_at { get; set; }

    [MaxLength(6)]
    public DateTime created_at { get; set; }

    [MaxLength(6)]
    public DateTime updated_at { get; set; }

    [ForeignKey("user_id")]
    [InverseProperty("calendar_syncs")]
    public virtual User user { get; set; } = null!;
}
