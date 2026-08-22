using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HawassaUnifiedCampusEventManagementSystem.Models
{
    // =========================================================
    // JOB POSTING VIEW MODEL (Used on /Jobs and /Jobs/Details)
    // =========================================================
    public class JobPostingViewModel
    {
        public ulong Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string? CompanyLogo { get; set; }
        public string CompanyInitials { get; set; } = "HU";
        public string CompanyColor { get; set; } = "#6f42c1";
        public string Industry { get; set; } = "Technology";
        public string JobType { get; set; } = "Internship"; // Internship, Full-Time, Part-Time, Contract, Remote
        public string WorkplaceType { get; set; } = "On-site"; // On-site, Hybrid, Remote
        public string Location { get; set; } = "Hawassa, Ethiopia";
        public string CampusLocation { get; set; } = "Main Campus";
        public string Description { get; set; } = string.Empty;
        public string ShortDescription { get; set; } = string.Empty;
        public List<string> Requirements { get; set; } = new();
        public List<string> Responsibilities { get; set; } = new();
        public List<string> Skills { get; set; } = new();
        public string SalaryDisplay { get; set; } = "Competitive";
        public DateTime? Deadline { get; set; }
        public string DeadlineString => Deadline?.ToString("MMM dd, yyyy") ?? "Open until filled";
        public bool IsClosingSoon { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsNew { get; set; }
        public bool IsVerifiedEmployer { get; set; } = true;
        public string? ApplicationUrl { get; set; }
        public string? ApplicationEmail { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int ApplicantCount { get; set; } = 12;
        public int ViewsCount { get; set; } = 84;
        public string Eligibility { get; set; } = "Open to Hawassa University Students & Graduates";
        public string ExperienceLevel { get; set; } = "Student / Internship";
    }

    // =========================================================
    // JOB SEARCH & FILTER VIEW MODEL
    // =========================================================
    public class JobFilterViewModel
    {
        public string? Search { get; set; }
        public string? JobType { get; set; } // All, Internship, Full-Time, Part-Time, Remote
        public string? WorkplaceType { get; set; } // On-site, Hybrid, Remote
        public string? Location { get; set; }
        public string? Department { get; set; }
        public string? Industry { get; set; }
        public string? SortBy { get; set; } = "newest"; // newest, deadline, popular, salary
        public bool OnlyClosingSoon { get; set; }
        public bool OnlyFeatured { get; set; }
        public int TotalJobs { get; set; }
        public List<JobPostingViewModel> Jobs { get; set; } = new();
        
        // Filter options for UI dropdowns and badges
        public List<string> AvailableLocations { get; set; } = new();
        public List<string> AvailableIndustries { get; set; } = new();
        public List<string> AvailableJobTypes { get; set; } = new();
        public Dictionary<string, int> JobTypeCounts { get; set; } = new();
    }

    // =========================================================
    // STUDENT JOB APPLICATION VIEW MODEL
    // =========================================================
    public class JobApplicationViewModel
    {
        public ulong JobId { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string JobType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter your full name.")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please provide your email address.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please provide your contact phone number.")]
        [Phone(ErrorMessage = "Please enter a valid phone number.")]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; } = string.Empty;

        [Display(Name = "Hawassa Student ID (Optional)")]
        public string? StudentId { get; set; }

        [Required(ErrorMessage = "Please select your department / faculty.")]
        [Display(Name = "Department / Faculty")]
        public string Department { get; set; } = "Computer Science & IT";

        [Display(Name = "Year of Study / Graduation Year")]
        public string YearOfStudy { get; set; } = "3rd Year";

        [Display(Name = "Cumulative GPA (Optional)")]
        public string? Gpa { get; set; }

        [Url(ErrorMessage = "Please provide a valid URL for your GitHub/Portfolio.")]
        [Display(Name = "Portfolio / GitHub / LinkedIn Link (Optional)")]
        public string? PortfolioUrl { get; set; }

        [Required(ErrorMessage = "Please write a brief cover letter or statement of interest.")]
        [MinLength(30, ErrorMessage = "Please write at least 30 characters about your interest.")]
        [Display(Name = "Cover Letter / Statement of Interest")]
        public string CoverLetter { get; set; } = string.Empty;

        [Display(Name = "Resume / CV File Name")]
        public string? ResumeFileName { get; set; }

        public bool AgreeToTerms { get; set; } = true;
    }

    // =========================================================
    // STUDENT APPLICATION HISTORY VIEW MODEL
    // =========================================================
    public class MyApplicationsViewModel
    {
        public List<StudentApplicationItemViewModel> Applications { get; set; } = new();
        public int TotalApplications => Applications.Count;
        public int UnderReviewCount => Applications.FindAll(a => a.Status == "Under Review" || a.Status == "Submitted").Count;
        public int ShortlistedCount => Applications.FindAll(a => a.Status == "Shortlisted" || a.Status == "Interview Scheduled").Count;
    }

    public class StudentApplicationItemViewModel
    {
        public string ApplicationId { get; set; } = string.Empty;
        public ulong JobId { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string JobType { get; set; } = string.Empty;
        public string ApplicantEmail { get; set; } = string.Empty;
        public string? UserId { get; set; }
        public DateTime AppliedAt { get; set; }
        public string Status { get; set; } = "Submitted"; // Submitted, Under Review, Shortlisted, Interview Scheduled, Accepted
        public string StatusBadgeClass { get; set; } = "bg-primary-subtle text-primary";
        public string? Notes { get; set; }
    }
}
