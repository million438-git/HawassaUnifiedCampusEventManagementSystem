using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HawassaUnifiedCampusEventManagementSystem.Models;

[Index("created_by", Name = "fk_jobs_created_by")]
[Index("deadline_at", Name = "idx_jobs_deadline")]
[Index("employer_id", Name = "idx_jobs_employer")]
[Index("status", Name = "idx_jobs_status")]
[Index("slug", Name = "uq_job_postings_slug", IsUnique = true)]
public partial class job_posting
{
    [Key]
    public ulong id { get; set; }

    public ulong employer_id { get; set; }

    [StringLength(255)]
    public string title { get; set; } = null!;

    [StringLength(300)]
    public string slug { get; set; } = null!;

    [Column(TypeName = "text")]
    public string description { get; set; } = null!;

    [Column(TypeName = "text")]
    public string? requirements { get; set; }

    [Column(TypeName = "text")]
    public string? responsibilities { get; set; }

    [Column(TypeName = "enum('FULL_TIME','PART_TIME','INTERNSHIP','CONTRACT','VOLUNTEER','TEMPORARY')")]
    public string job_type { get; set; } = null!;

    [Column(TypeName = "enum('ON_SITE','REMOTE','HYBRID')")]
    public string workplace_type { get; set; } = null!;

    [StringLength(255)]
    public string? location { get; set; }

    [Precision(15)]
    public decimal? salary_min { get; set; }

    [Precision(15)]
    public decimal? salary_max { get; set; }

    [StringLength(10)]
    public string salary_currency { get; set; } = null!;

    [StringLength(1000)]
    public string? application_url { get; set; }

    [StringLength(255)]
    public string? application_email { get; set; }

    [MaxLength(6)]
    public DateTime? published_at { get; set; }

    [MaxLength(6)]
    public DateTime? deadline_at { get; set; }

    [Column(TypeName = "enum('DRAFT','PUBLISHED','CLOSED','EXPIRED','SUSPENDED')")]
    public string status { get; set; } = null!;

    public ulong? created_by { get; set; }

    [MaxLength(6)]
    public DateTime created_at { get; set; }

    [MaxLength(6)]
    public DateTime updated_at { get; set; }

    [ForeignKey("created_by")]
    [InverseProperty("job_postings")]
    public virtual user? created_byNavigation { get; set; }

    [ForeignKey("employer_id")]
    [InverseProperty("job_postings")]
    public virtual employer employer { get; set; } = null!;

    [InverseProperty("job_posting")]
    public virtual ICollection<interview_slot> interview_slots { get; set; } = new List<interview_slot>();
}
