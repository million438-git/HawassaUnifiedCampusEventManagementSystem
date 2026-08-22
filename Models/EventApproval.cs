using System;

namespace HawassaUnifiedCampusEventManagementSystem.Models;

public partial class EventApproval
{
    public ulong id { get; set; }

    public ulong event_id { get; set; }

    public ulong reviewer_id { get; set; }

    public string action { get; set; } = null!;

    public string? reason { get; set; }

    public DateTime reviewed_at { get; set; }

    public DateTime created_at { get; set; }
}