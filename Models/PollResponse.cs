using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HawassaUnifiedCampusEventManagementSystem.Models;

[Table("poll_responses")]
[Index("option_id", Name = "idx_poll_responses_option")]
[Index("poll_id", Name = "idx_poll_responses_poll")]
[Index("user_id", Name = "idx_poll_responses_user")]
[Index("poll_id", "option_id", "user_id", Name = "uq_poll_response", IsUnique = true)]
public partial class poll_response
{
    [Key]
    public ulong id { get; set; }

    public ulong poll_id { get; set; }

    public ulong option_id { get; set; }

    public ulong user_id { get; set; }

    [MaxLength(6)]
    public DateTime responded_at { get; set; }

    [ForeignKey("option_id")]
    [InverseProperty("poll_responses")]
    public virtual poll_option option { get; set; } = null!;

    [ForeignKey("poll_id")]
    [InverseProperty("poll_responses")]
    public virtual Poll poll { get; set; } = null!;

    [ForeignKey("user_id")]
    [InverseProperty("poll_responses")]
    public virtual User user { get; set; } = null!;
}
