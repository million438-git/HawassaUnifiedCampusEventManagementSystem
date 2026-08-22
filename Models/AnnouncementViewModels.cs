using System;
using System.Collections.Generic;

namespace HawassaUnifiedCampusEventManagementSystem.Models
{
    public class AnnouncementListViewModel
    {
        public List<AnnouncementItemViewModel> Announcements { get; set; } = new();
        public string? SelectedPriority { get; set; }
        public string? SelectedType { get; set; }
        public string? SearchTerm { get; set; }
        public int TotalCount => Announcements.Count;
        public int UrgentCount => Announcements.FindAll(a => a.IsUrgent).Count;
    }

    public class AnnouncementItemViewModel
    {
        public ulong Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? Summary { get; set; }
        public string Content { get; set; } = string.Empty;
        public string Priority { get; set; } = "NORMAL"; // LOW, NORMAL, HIGH, URGENT
        public string AnnouncementType { get; set; } = "GENERAL"; // NEWS, NOTICE, ALERT, CLOSURE, ACADEMIC, CAREER, GENERAL
        public string? ImageUrl { get; set; }
        public string AuthorName { get; set; } = "University Administration";
        public string DepartmentName { get; set; } = "Campus Directorate";
        public DateTime PublishedDate { get; set; }
        public DateTime? ExpiresDate { get; set; }
        public bool IsUrgent => Priority == "URGENT" || Priority == "HIGH";
        public string PriorityBadgeClass => Priority switch
        {
            "URGENT" => "bg-danger text-white",
            "HIGH" => "bg-warning text-dark",
            "LOW" => "bg-secondary text-white",
            _ => "bg-primary-subtle text-primary"
        };
    }

    public class AnnouncementDetailsViewModel
    {
        public ulong Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? Summary { get; set; }
        public string Content { get; set; } = string.Empty;
        public string Priority { get; set; } = "NORMAL";
        public string AnnouncementType { get; set; } = "GENERAL";
        public string? ImageUrl { get; set; }
        public string AuthorName { get; set; } = "University Administration";
        public string DepartmentName { get; set; } = "Campus Directorate";
        public DateTime PublishedDate { get; set; }
        public DateTime? ExpiresDate { get; set; }
        public bool IsUrgent => Priority == "URGENT" || Priority == "HIGH";
        public List<AnnouncementItemViewModel> RelatedNotices { get; set; } = new();
    }
}
