using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HawassaUnifiedCampusEventManagementSystem.Models;

[Index("faculty_id", Name = "idx_departments_faculty")]
[Index("code", Name = "uq_departments_code", IsUnique = true)]
[Index("faculty_id", "name", Name = "uq_departments_faculty_name", IsUnique = true)]
public partial class department
{
    [Key]
    public ulong id { get; set; }

    public ulong faculty_id { get; set; }

    [StringLength(200)]
    public string name { get; set; } = null!;

    [StringLength(50)]
    public string? code { get; set; }

    [Column(TypeName = "text")]
    public string? description { get; set; }

    [StringLength(200)]
    public string? head_name { get; set; }

    [StringLength(255)]
    public string? email { get; set; }

    [StringLength(50)]
    public string? phone { get; set; }

    [Required]
    public bool? is_active { get; set; }

    [MaxLength(6)]
    public DateTime created_at { get; set; }

    [MaxLength(6)]
    public DateTime updated_at { get; set; }

    [InverseProperty("department")]
    public virtual ICollection<announcement> announcements { get; set; } = new List<announcement>();

    [InverseProperty("department")]
    public virtual ICollection<class_schedule> class_schedules { get; set; } = new List<class_schedule>();

    [ForeignKey("faculty_id")]
    [InverseProperty("departments")]
    public virtual faculty faculty { get; set; } = null!;

    [InverseProperty("department")]
    public virtual ICollection<organization> organizations { get; set; } = new List<organization>();

    [InverseProperty("department")]
    public virtual ICollection<study_group> study_groups { get; set; } = new List<study_group>();

    [InverseProperty("department")]
    public virtual ICollection<user_dept_subscription> user_dept_subscriptions { get; set; } = new List<user_dept_subscription>();

    [InverseProperty("department")]
    public virtual ICollection<user> users { get; set; } = new List<user>();
}
