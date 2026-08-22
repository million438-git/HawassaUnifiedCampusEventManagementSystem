using System;

namespace HawassaUnifiedCampusEventManagementSystem.Models;

public partial class SystemSetting
{
    public ulong id { get; set; }

    public string setting_key { get; set; } = null!;

    public string? setting_value { get; set; }

    public string? description { get; set; }

    public ulong? updated_by { get; set; }

    public DateTime created_at { get; set; }

    public DateTime updated_at { get; set; }
}