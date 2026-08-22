using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HawassaUnifiedCampusEventManagementSystem.Models
{
    [Table("job_applications")]
    [Index("job_posting_id", "applicant_user_id", Name = "uq_job_applications_job_user", IsUnique = true)]
    [Index("applicant_user_id", Name = "idx_job_applications_user")]
    [Index("job_posting_id", Name = "idx_job_applications_job")]
    [Index("status", Name = "idx_job_applications_status")]
    public partial class job_application
    {
        [Key]
        public ulong id { get; set; }

        public ulong job_posting_id { get; set; }

        public ulong applicant_user_id { get; set; }

        [StringLength(50)]
        public string application_code { get; set; } = string.Empty;

        [StringLength(150)]
        public string full_name { get; set; } = string.Empty;

        [StringLength(150)]
        public string email { get; set; } = string.Empty;

        [StringLength(50)]
        public string? phone { get; set; }

        [StringLength(50)]
        public string? student_id { get; set; }

        [StringLength(150)]
        public string? department { get; set; }

        [StringLength(50)]
        public string? year_of_study { get; set; }

        [StringLength(20)]
        public string? gpa { get; set; }

        [StringLength(500)]
        public string? portfolio_url { get; set; }

        [Column(TypeName = "text")]
        public string? cover_letter { get; set; }

        [StringLength(500)]
        public string? resume_path { get; set; }

        [Column(TypeName = "enum('SUBMITTED','UNDER_REVIEW','SHORTLISTED','INTERVIEW_SCHEDULED','REJECTED','ACCEPTED')")]
        public string status { get; set; } = "SUBMITTED";

        [Column(TypeName = "text")]
        public string? reviewer_notes { get; set; }

        [MaxLength(6)]
        public DateTime applied_at { get; set; } = DateTime.UtcNow;

        [MaxLength(6)]
        public DateTime updated_at { get; set; } = DateTime.UtcNow;

        [ForeignKey("job_posting_id")]
        [InverseProperty("job_applications")]
        public virtual job_posting job_posting { get; set; } = null!;

        [ForeignKey("applicant_user_id")]
        [InverseProperty("job_applications")]
        public virtual User applicant_user { get; set; } = null!;
    }
}
