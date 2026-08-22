using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HawassaUnifiedCampusEventManagementSystem.Models;

[Index("course_code", Name = "idx_study_groups_course")]
[Index("created_by", Name = "idx_study_groups_creator")]
[Index("department_id", Name = "idx_study_groups_department")]
public partial class study_group
{
    [Key]
    public ulong id { get; set; }

    public ulong? department_id { get; set; }

    public ulong created_by { get; set; }

    [StringLength(255)]
    public string name { get; set; } = null!;

    [Column(TypeName = "text")]
    public string? description { get; set; }

    [StringLength(50)]
    public string? course_code { get; set; }

    [StringLength(255)]
    public string? course_name { get; set; }

    [StringLength(50)]
    public string? academic_year { get; set; }

    [StringLength(50)]
    public string? semester { get; set; }

    public uint? max_members { get; set; }

    [Column(TypeName = "enum('PUBLIC','PRIVATE')")]
    public string group_type { get; set; } = null!;

    [Column(TypeName = "enum('ACTIVE','FULL','CLOSED','ARCHIVED')")]
    public string status { get; set; } = null!;

    [MaxLength(6)]
    public DateTime created_at { get; set; }

    [MaxLength(6)]
    public DateTime updated_at { get; set; }

    [ForeignKey("created_by")]
    [InverseProperty("study_groups")]
    public virtual user created_byNavigation { get; set; } = null!;

    [ForeignKey("department_id")]
    [InverseProperty("study_groups")]
    public virtual department? department { get; set; }

    [InverseProperty("study_group")]
    public virtual ICollection<study_group_member> study_group_members { get; set; } = new List<study_group_member>();
}
