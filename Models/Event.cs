namespace HawassaUnifiedCampusEventManagementSystem.Models
{
    /// <summary>
    /// Canonical Event ViewModel utilized by Razor MVC Views, form models, and UI controllers.
    /// Provides projection and presentation formatting for the underlying '_event' database entity.
    /// </summary>
    public class Event
    {
        public ulong Id { get; set; }
        public string? Title { get; set; }
        public string? Category { get; set; }
        public int? Capacity { get; set; }
        public string? Description { get; set; }
        public DateTime EventDate { get; set; }
        public string? Venue { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public ulong? OrganizerId { get; set; }
        public string? Organizer { get; set; }
        public string? OrganizerEmail { get; set; }
        public string? ContactPhone { get; set; }
        public bool IsPublished { get; set; }
        public string? ApprovalStatus { get; set; } // PENDING, APPROVED, REJECTED
        public string? Status { get; set; } // DRAFT, PUBLISHED, CANCELLED, COMPLETED
        public bool IsUserRegistered { get; set; }
        public int RegisteredCount { get; set; }

        // Additional common properties used in details/list views
        public string? ShortDescription { get; set; }
        public string? ImageUrl { get; set; }
        public string? Slug { get; set; }
        public DateTime? CreatedAt { get; set; }
    }

    public class EventCategorySummaryViewModel
    {
        public ulong Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Icon { get; set; }
        public int EventCount { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class EventCategoryItemViewModel : EventCategorySummaryViewModel
    {
        public int Count
        {
            get => EventCount;
            set => EventCount = value;
        }
    }
}
