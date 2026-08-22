-- ============================================================================
-- HUCEMS Schema Migration: job_applications Table
-- Relational Job Application Persistence, Duplication Guard & Interview Workflow
-- ============================================================================

CREATE TABLE IF NOT EXISTS `job_applications` (
    `id` BIGINT UNSIGNED NOT NULL AUTO_INCREMENT,
    `job_posting_id` BIGINT UNSIGNED NOT NULL,
    `applicant_user_id` BIGINT UNSIGNED NOT NULL,
    `application_code` VARCHAR(50) NOT NULL,
    `full_name` VARCHAR(150) NOT NULL,
    `email` VARCHAR(150) NOT NULL,
    `phone` VARCHAR(50) NULL,
    `student_id` VARCHAR(50) NULL,
    `department` VARCHAR(150) NULL,
    `year_of_study` VARCHAR(50) NULL,
    `gpa` VARCHAR(20) NULL,
    `portfolio_url` VARCHAR(500) NULL,
    `cover_letter` TEXT NULL,
    `resume_path` VARCHAR(500) NULL,
    `status` ENUM('SUBMITTED','UNDER_REVIEW','SHORTLISTED','INTERVIEW_SCHEDULED','REJECTED','ACCEPTED') NOT NULL DEFAULT 'SUBMITTED',
    `reviewer_notes` TEXT NULL,
    `applied_at` DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `updated_at` DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    PRIMARY KEY (`id`),
    UNIQUE KEY `uq_job_applications_job_user` (`job_posting_id`, `applicant_user_id`),
    KEY `idx_job_applications_job` (`job_posting_id`),
    KEY `idx_job_applications_user` (`applicant_user_id`),
    KEY `idx_job_applications_status` (`status`),
    CONSTRAINT `fk_job_app_posting` FOREIGN KEY (`job_posting_id`) REFERENCES `job_postings` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT `fk_job_app_user` FOREIGN KEY (`applicant_user_id`) REFERENCES `users` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
