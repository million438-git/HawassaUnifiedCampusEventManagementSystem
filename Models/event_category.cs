using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HawassaUnifiedCampusEventManagementSystem.Models;

[Index("name", Name = "uq_event_categories_name", IsUnique = true)]
[Index("slug", Name = "uq_event_categories_slug", IsUnique = true)]
public partial class event_category
{
    [Key]
    public ulong id { get; set; }

    [StringLength(100)]
    public string name { get; set; } = null!;

    [StringLength(120)]
    public string slug { get; set; } = null!;

    [StringLength(500)]
    public string? description { get; set; }

    [StringLength(100)]
    public string? icon { get; set; }

    [Required]
    public bool? is_active { get; set; }

    [MaxLength(6)]
    public DateTime created_at { get; set; }

    [MaxLength(6)]
    public DateTime updated_at { get; set; }

    [InverseProperty("category")]
    public virtual ICollection<_event> _events { get; set; } = new List<_event>();

    [InverseProperty("category")]
    public virtual ICollection<user_category_interest> user_category_interests { get; set; } = new List<user_category_interest>();
}
