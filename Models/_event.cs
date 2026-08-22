using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HawassaUnifiedCampusEventManagementSystem.Models;

/// <summary>
/// Relational Database Entity mapped to the MySQL 'events' table.
/// Named '_event' (with leading underscore) to prevent collision with the C# reserved language keyword 'event'.
/// </summary>
[Table("events")]
[Index("approved_by", Name = "fk_events_approved_by")]
[Index("approval_status", Name = "idx_events_approval")]
[Index("category_id", Name = "idx_events_category")]
[Index("organization_id", Name = "idx_events_organization")]
[Index("organizer_id", Name = "idx_events_organizer")]
[Index("start_at", Name = "idx_events_start")]
[Index("status", Name = "idx_events_status")]
[Index("venue_id", Name = "idx_events_venue")]
[Index("slug", Name = "uq_events_slug", IsUnique = true)]
public partial class _event
{
    [Key]
    public ulong id { get; set; }

    [StringLength(255)]
    public string title { get; set; } = null!;

    [StringLength(300)]
    public string slug { get; set; } = null!;

    [Column(TypeName = "text")]
    public string? description { get; set; }

    [StringLength(500)]
    public string? short_description { get; set; }

    public ulong category_id { get; set; }

    public ulong organizer_id { get; set; }

    public ulong? organization_id { get; set; }

    public ulong? venue_id { get; set; }

    [MaxLength(6)]
    public DateTime start_at { get; set; }

    [MaxLength(6)]
    public DateTime end_at { get; set; }

    [MaxLength(6)]
    public DateTime? registration_start_at { get; set; }

    [MaxLength(6)]
    public DateTime? registration_end_at { get; set; }

    public uint? capacity { get; set; }

    [Required]
    public bool? registration_required { get; set; }

    public bool allow_waitlist { get; set; }

    [Column(TypeName = "enum('IN_PERSON','ONLINE','HYBRID')")]
    public string event_mode { get; set; } = null!;

    [StringLength(1000)]
    public string? online_url { get; set; }

    [StringLength(1000)]
    public string? image_url { get; set; }

    [Column(TypeName = "enum('DRAFT','PENDING_APPROVAL','APPROVED','PUBLISHED','REJECTED','CANCELLED','COMPLETED')")]
    public string status { get; set; } = null!;

    [Column(TypeName = "enum('NOT_REQUIRED','PENDING','APPROVED','REJECTED')")]
    public string approval_status { get; set; } = null!;

    public ulong? approved_by { get; set; }

    [MaxLength(6)]
    public DateTime? approved_at { get; set; }

    [Column(TypeName = "text")]
    public string? rejection_reason { get; set; }

    public bool is_featured { get; set; }

    [Required]
    public bool? is_public { get; set; }

    [MaxLength(6)]
    public DateTime created_at { get; set; }

    [MaxLength(6)]
    public DateTime updated_at { get; set; }

    [ForeignKey("approved_by")]
    [InverseProperty("_eventapproved_byNavigations")]
    public virtual User? approved_byNavigation { get; set; }

    [ForeignKey("category_id")]
    [InverseProperty("_events")]
    public virtual event_category category { get; set; } = null!;

    [InverseProperty("_event")]
    public virtual ICollection<event_comment> event_comments { get; set; } = new List<event_comment>();

    [InverseProperty("_event")]
    public virtual ICollection<event_feedback> event_feedbacks { get; set; } = new List<event_feedback>();

    [ForeignKey("organization_id")]
    [InverseProperty("_events")]
    public virtual Organization? organization { get; set; }

    [ForeignKey("organizer_id")]
    [InverseProperty("_eventorganizers")]
    public virtual User organizer { get; set; } = null!;

    [InverseProperty("_event")]
    public virtual ICollection<Registration> registrations { get; set; } = new List<Registration>();

    [ForeignKey("venue_id")]
    [InverseProperty("_events")]
    public virtual Venue? venue { get; set; }

    [ForeignKey("event_id")]
    [InverseProperty("events")]
    public virtual ICollection<event_tag> tags { get; set; } = new List<event_tag>();
}
