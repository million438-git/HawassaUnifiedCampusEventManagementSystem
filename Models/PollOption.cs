using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HawassaUnifiedCampusEventManagementSystem.Models;

[Table("poll_options")]
[Index("poll_id", "display_order", Name = "idx_poll_options_order")]
[Index("poll_id", Name = "idx_poll_options_poll")]
public partial class poll_option
{
    [Key]
    public ulong id { get; set; }

    public ulong poll_id { get; set; }

    [StringLength(500)]
    public string option_text { get; set; } = null!;

    public uint display_order { get; set; }

    [MaxLength(6)]
    public DateTime created_at { get; set; }

    [ForeignKey("poll_id")]
    [InverseProperty("poll_options")]
    public virtual Poll poll { get; set; } = null!;

    [InverseProperty("option")]
    public virtual ICollection<poll_response> poll_responses { get; set; } = new List<poll_response>();
}
