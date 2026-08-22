using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HawassaUnifiedCampusEventManagementSystem.Models;

[Index("created_by", Name = "idx_polls_creator")]
[Index("status", Name = "idx_polls_status")]
public partial class poll
{
    [Key]
    public ulong id { get; set; }

    public ulong created_by { get; set; }

    [StringLength(255)]
    public string title { get; set; } = null!;

    [Column(TypeName = "text")]
    public string question { get; set; } = null!;

    [Column(TypeName = "text")]
    public string? description { get; set; }

    [MaxLength(6)]
    public DateTime? start_at { get; set; }

    [MaxLength(6)]
    public DateTime? end_at { get; set; }

    public bool allow_multiple_answers { get; set; }

    public bool anonymous { get; set; }

    [Column(TypeName = "enum('DRAFT','ACTIVE','CLOSED','ARCHIVED')")]
    public string status { get; set; } = null!;

    [MaxLength(6)]
    public DateTime created_at { get; set; }

    [MaxLength(6)]
    public DateTime updated_at { get; set; }

    [ForeignKey("created_by")]
    [InverseProperty("polls")]
    public virtual user created_byNavigation { get; set; } = null!;

    [InverseProperty("poll")]
    public virtual ICollection<poll_option> poll_options { get; set; } = new List<poll_option>();

    [InverseProperty("poll")]
    public virtual ICollection<poll_response> poll_responses { get; set; } = new List<poll_response>();
}
