using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HawassaUnifiedCampusEventManagementSystem.Models;

[Table("study_group_members")]
[PrimaryKey("study_group_id", "user_id")]
[Index("user_id", Name = "idx_study_group_members_user")]
public partial class study_group_member
{
    [Key]
    public ulong study_group_id { get; set; }

    [Key]
    public ulong user_id { get; set; }

    [Column(TypeName = "enum('MEMBER','MODERATOR','OWNER')")]
    public string member_role { get; set; } = null!;

    [MaxLength(6)]
    public DateTime joined_at { get; set; }

    [MaxLength(6)]
    public DateTime? left_at { get; set; }

    [Required]
    public bool? is_active { get; set; }

    [ForeignKey("study_group_id")]
    [InverseProperty("study_group_members")]
    public virtual study_group study_group { get; set; } = null!;

    [ForeignKey("user_id")]
    [InverseProperty("study_group_members")]
    public virtual User user { get; set; } = null!;
}
