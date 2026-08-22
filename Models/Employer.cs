using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HawassaUnifiedCampusEventManagementSystem.Models;

[Table("employers")]
[Index("created_by", Name = "fk_employers_created_by")]
[Index("name", Name = "idx_employers_name")]
[Index("status", Name = "idx_employers_status")]
[Index("slug", Name = "uq_employers_slug", IsUnique = true)]
public partial class Employer
{
    [Key]
    public ulong id { get; set; }

    public string name { get; set; } = null!;

    [StringLength(300)]
    public string slug { get; set; } = null!;

    [Column(TypeName = "text")]
    public string? description { get; set; }

    [StringLength(1000)]
    public string? website_url { get; set; }

    [StringLength(1000)]
    public string? logo_url { get; set; }

    [StringLength(255)]
    public string? email { get; set; }

    [StringLength(50)]
    public string? phone { get; set; }

    [StringLength(500)]
    public string? address { get; set; }

    [StringLength(150)]
    public string? industry { get; set; }

    [StringLength(200)]
    public string? contact_person_name { get; set; }

    [StringLength(255)]
    public string? contact_person_email { get; set; }

    [StringLength(50)]
    public string? contact_person_phone { get; set; }

    public bool verified { get; set; }

    [Column(TypeName = "enum('PENDING','ACTIVE','SUSPENDED','INACTIVE')")]
    public string status { get; set; } = null!;

    public ulong? created_by { get; set; }

    [MaxLength(6)]
    public DateTime created_at { get; set; }

    [MaxLength(6)]
    public DateTime updated_at { get; set; }

    [ForeignKey("created_by")]
    [InverseProperty("employers")]
    public virtual User? created_byNavigation { get; set; }

    [InverseProperty("employer")]
    public virtual ICollection<job_posting> job_postings { get; set; } = new List<job_posting>();
}
