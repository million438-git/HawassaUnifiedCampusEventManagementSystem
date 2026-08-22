using System.ComponentModel.DataAnnotations;

namespace HawassaUnifiedCampusEventManagementSystem.Models
{
    public class ContactViewModel
    {
        [Required(ErrorMessage = "Please enter your full name.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter your university or personal email address.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [StringLength(150, ErrorMessage = "Email cannot exceed 150 characters.")]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Please enter a valid phone number.")]
        [Display(Name = "Phone Number (Optional)")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Please select an inquiry topic.")]
        [Display(Name = "Inquiry Topic")]
        public string Subject { get; set; } = "General Campus Question";

        [Display(Name = "Department / Campus")]
        public string? Department { get; set; } = "Main Campus";

        [Required(ErrorMessage = "Please enter your message.")]
        [MinLength(15, ErrorMessage = "Message must be at least 15 characters long.")]
        [MaxLength(2000, ErrorMessage = "Message cannot exceed 2000 characters.")]
        [Display(Name = "Message Details")]
        public string Message { get; set; } = string.Empty;
    }
}
