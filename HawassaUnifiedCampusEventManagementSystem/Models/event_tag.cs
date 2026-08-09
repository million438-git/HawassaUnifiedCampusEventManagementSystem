using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HawassaUnifiedCampusEventManagementSystem.Models;

[Index("name", Name = "uq_event_tags_name", IsUnique = true)]
[Index("slug", Name = "uq_event_tags_slug", IsUnique = true)]
public partial class event_tag
{
    [Key]
    public ulong id { get; set; }

    [StringLength(100)]
    public string name { get; set; } = null!;

    [StringLength(120)]
    public string slug { get; set; } = null!;

    [StringLength(500)]
    public string? description { get; set; }

    [MaxLength(6)]
    public DateTime created_at { get; set; }

    [ForeignKey("tag_id")]
    [InverseProperty("tags")]
    public virtual ICollection<_event> events { get; set; } = new List<_event>();
}
