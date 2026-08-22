using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HawassaUnifiedCampusEventManagementSystem.Models;

[Index("job_posting_id", Name = "idx_interview_slots_job")]
[Index("start_at", Name = "idx_interview_slots_start")]
[Index("venue_id", Name = "idx_interview_slots_venue")]
public partial class interview_slot
{
    [Key]
    public ulong id { get; set; }

    public ulong job_posting_id { get; set; }

    [StringLength(255)]
    public string? title { get; set; }

    [MaxLength(6)]
    public DateTime start_at { get; set; }

    [MaxLength(6)]
    public DateTime end_at { get; set; }

    public ulong? venue_id { get; set; }

    [StringLength(1000)]
    public string? meeting_url { get; set; }

    public uint capacity { get; set; }

    [Column(TypeName = "enum('AVAILABLE','FULL','CANCELLED','COMPLETED')")]
    public string status { get; set; } = null!;

    [MaxLength(6)]
    public DateTime created_at { get; set; }

    [MaxLength(6)]
    public DateTime updated_at { get; set; }

    [InverseProperty("interview_slot")]
    public virtual ICollection<interview_booking> interview_bookings { get; set; } = new List<interview_booking>();

    [ForeignKey("job_posting_id")]
    [InverseProperty("interview_slots")]
    public virtual job_posting job_posting { get; set; } = null!;

    [ForeignKey("venue_id")]
    [InverseProperty("interview_slots")]
    public virtual venue? venue { get; set; }
}
