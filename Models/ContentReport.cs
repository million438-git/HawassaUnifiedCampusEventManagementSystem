using System;

namespace HawassaUnifiedCampusEventManagementSystem.Models;

public partial class ContentReport
{
    public ulong id { get; set; }

    public ulong reporter_id { get; set; }

    public string content_type { get; set; } = null!;

    public ulong content_id { get; set; }

    public string reason { get; set; } = null!;

    public string? description { get; set; }

    public string status { get; set; } = "PENDING";

    public ulong? reviewed_by { get; set; }

    public DateTime? reviewed_at { get; set; }

    public DateTime created_at { get; set; }
}