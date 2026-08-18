using System;

namespace HawassaUnifiedCampusEventManagementSystem.Models
{
    // Simple view model matching views that expect Models.Event
    public class Event
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Category { get; set; }
        public int? Capacity { get; set; }
        public string? Description { get; set; }
        public DateTime EventDate { get; set; }
        public string? Venue { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public string? Organizer { get; set; }
        public string? OrganizerEmail { get; set; }
        public string? ContactPhone { get; set; }
        public bool IsPublished { get; set; }

        // Additional common properties used in details/list views
        public string? ShortDescription { get; set; }
        public string? ImageUrl { get; set; }
        public string? Slug { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
