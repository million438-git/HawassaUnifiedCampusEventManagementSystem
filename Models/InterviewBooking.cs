using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HawassaUnifiedCampusEventManagementSystem.Models;

[Table("interview_bookings")]
[Index("user_id", Name = "idx_interview_bookings_user")]
[Index("interview_slot_id", "user_id", Name = "uq_interview_booking_slot_user", IsUnique = true)]
public partial class interview_booking
{
    [Key]
    public ulong id { get; set; }

    public ulong interview_slot_id { get; set; }

    public ulong user_id { get; set; }

    [Column(TypeName = "enum('BOOKED','CANCELLED','ATTENDED','NO_SHOW')")]
    public string status { get; set; } = null!;

    [Column(TypeName = "text")]
    public string? notes { get; set; }

    [MaxLength(6)]
    public DateTime booked_at { get; set; }

    [MaxLength(6)]
    public DateTime? cancelled_at { get; set; }

    [ForeignKey("interview_slot_id")]
    [InverseProperty("interview_bookings")]
    public virtual interview_slot interview_slot { get; set; } = null!;

    [ForeignKey("user_id")]
    [InverseProperty("interview_bookings")]
    public virtual User user { get; set; } = null!;
}
