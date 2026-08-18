using System;

namespace HawassaUnifiedCampusEventManagementSystem.Models;

public partial class UserSuspension
{
    public ulong id { get; set; }

    public ulong user_id { get; set; }

    public ulong suspended_by { get; set; }

    public string reason { get; set; } = null!;

    public DateTime start_date { get; set; }

    public DateTime? end_date { get; set; }

    public string status { get; set; } = "ACTIVE";

    public DateTime created_at { get; set; }
}