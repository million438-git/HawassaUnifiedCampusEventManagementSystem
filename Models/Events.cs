using System;

namespace HawassaUnifiedCampusEventManagementSystem.Models
{
    // View model for Events views (keeps view strongly-typed and avoids naming mismatch with EF entities)
    public class Events
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Category { get; set; }

        public int? Capacity { get; set; }

        public string? Description { get; set; }

        public DateTime? EventDate { get; set; }

        public string? Venue { get; set; }

        public TimeSpan? StartTime { get; set; }

        public TimeSpan? EndTime { get; set; }

        public string? Organizer { get; set; }

        public string? OrganizerEmail { get; set; }

        public string? ContactPhone { get; set; }

        public bool IsPublished { get; set; }
    }
}
