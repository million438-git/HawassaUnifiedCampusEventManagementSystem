using System;
using System.Collections.Generic;
using HawassaUnifiedCampusEventManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace HawassaUnifiedCampusEventManagementSystem.Data;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<_event> events { get; set; }

    public virtual DbSet<announcement> announcements { get; set; }

    public virtual DbSet<audit_log> audit_logs { get; set; }

    public virtual DbSet<auth_token> auth_tokens { get; set; }

    public virtual DbSet<calendar_sync> calendar_syncs { get; set; }

    public virtual DbSet<class_schedule> class_schedules { get; set; }

    public virtual DbSet<department> departments { get; set; }

    public virtual DbSet<device_token> device_tokens { get; set; }

    public virtual DbSet<employer> employers { get; set; }

    public virtual DbSet<event_category> event_categories { get; set; }

    public virtual DbSet<event_comment> event_comments { get; set; }

    public virtual DbSet<event_feedback> event_feedbacks { get; set; }

    public virtual DbSet<event_tag> event_tags { get; set; }

    public virtual DbSet<faculty> faculties { get; set; }

    public virtual DbSet<interview_booking> interview_bookings { get; set; }

    public virtual DbSet<interview_slot> interview_slots { get; set; }

    public virtual DbSet<job_posting> job_postings { get; set; }

    public virtual DbSet<notification> notifications { get; set; }

    public virtual DbSet<organization> organizations { get; set; }

    public virtual DbSet<organization_member> organization_members { get; set; }

    public virtual DbSet<permission> permissions { get; set; }

    public virtual DbSet<poll> polls { get; set; }

    public virtual DbSet<poll_option> poll_options { get; set; }

    public virtual DbSet<poll_response> poll_responses { get; set; }

    public virtual DbSet<registration> registrations { get; set; }

    public virtual DbSet<role> roles { get; set; }

    public virtual DbSet<role_permission> role_permissions { get; set; }

    public virtual DbSet<session> sessions { get; set; }

    public virtual DbSet<study_group> study_groups { get; set; }

    public virtual DbSet<study_group_member> study_group_members { get; set; }

    public virtual DbSet<user> users { get; set; }

    public virtual DbSet<user_category_interest> user_category_interests { get; set; }

    public virtual DbSet<user_dept_subscription> user_dept_subscriptions { get; set; }

    public virtual DbSet<user_preference> user_preferences { get; set; }

    public virtual DbSet<user_role> user_roles { get; set; }

    public virtual DbSet<venue> venues { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySQL("Server=localhost;Port=3306;Database=university_event_management;User=root;Password=@root;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<_event>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.Property(e => e.approval_status).HasDefaultValueSql("'PENDING'");
            entity.Property(e => e.created_at).HasDefaultValueSql("'CURRENT_TIMESTAMP(6)'");
            entity.Property(e => e.event_mode).HasDefaultValueSql("'IN_PERSON'");
            entity.Property(e => e.is_public).HasDefaultValueSql("'1'");
            entity.Property(e => e.registration_required).HasDefaultValueSql("'1'");
            entity.Property(e => e.status).HasDefaultValueSql("'DRAFT'");
            entity.Property(e => e.updated_at)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("'CURRENT_TIMESTAMP(6)'");

            entity.HasOne(d => d.approved_byNavigation).WithMany(p => p._eventapproved_byNavigations)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_events_approved_by");

            entity.HasOne(d => d.category).WithMany(p => p._events)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_events_category");

            entity.HasOne(d => d.organization).WithMany(p => p._events)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_events_organization");

            entity.HasOne(d => d.organizer).WithMany(p => p._eventorganizers)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_events_organizer");

            entity.HasOne(d => d.venue).WithMany(p => p._events)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_events_venue");

            entity.HasMany(d => d.tags).WithMany(p => p.events)
                .UsingEntity<Dictionary<string, object>>(
                    "event_tag_map",
                    r => r.HasOne<event_tag>().WithMany()
                        .HasForeignKey("tag_id")
                        .HasConstraintName("fk_event_tag_map_tag"),
                    l => l.HasOne<_event>().WithMany()
                        .HasForeignKey("event_id")
                        .HasConstraintName("fk_event_tag_map_event"),
                    j =>
                    {
                        j.HasKey("event_id", "tag_id").HasName("PRIMARY");
                        j.ToTable("event_tag_map");
                        j.HasIndex(new[] { "tag_id" }, "idx_event_tag_map_tag");
                    });
        });

        modelBuilder.Entity<announcement>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.Property(e => e.announcement_type).HasDefaultValueSql("'GENERAL'");
            entity.Property(e => e.created_at).HasDefaultValueSql("'CURRENT_TIMESTAMP(6)'");
            entity.Property(e => e.priority).HasDefaultValueSql("'NORMAL'");
            entity.Property(e => e.status).HasDefaultValueSql("'DRAFT'");
            entity.Property(e => e.updated_at)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("'CURRENT_TIMESTAMP(6)'");

            entity.HasOne(d => d.author).WithMany(p => p.announcements)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_announcements_author");

            entity.HasOne(d => d.department).WithMany(p => p.announcements)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_announcements_department");
        });

        modelBuilder.Entity<audit_log>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.Property(e => e.created_at).HasDefaultValueSql("'CURRENT_TIMESTAMP(6)'");

            entity.HasOne(d => d.user).WithMany(p => p.audit_logs)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_audit_logs_user");
        });

        modelBuilder.Entity<auth_token>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.Property(e => e.created_at).HasDefaultValueSql("'CURRENT_TIMESTAMP(6)'");

            entity.HasOne(d => d.user).WithMany(p => p.auth_tokens).HasConstraintName("fk_auth_tokens_user");
        });

        modelBuilder.Entity<calendar_sync>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.Property(e => e.created_at).HasDefaultValueSql("'CURRENT_TIMESTAMP(6)'");
            entity.Property(e => e.sync_enabled).HasDefaultValueSql("'1'");
            entity.Property(e => e.updated_at)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("'CURRENT_TIMESTAMP(6)'");

            entity.HasOne(d => d.user).WithMany(p => p.calendar_syncs).HasConstraintName("fk_calendar_syncs_user");
        });

        modelBuilder.Entity<class_schedule>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.Property(e => e.created_at).HasDefaultValueSql("'CURRENT_TIMESTAMP(6)'");
            entity.Property(e => e.updated_at)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("'CURRENT_TIMESTAMP(6)'");

            entity.HasOne(d => d.department).WithMany(p => p.class_schedules)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_class_schedules_department");
        });

        modelBuilder.Entity<department>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.Property(e => e.created_at).HasDefaultValueSql("'CURRENT_TIMESTAMP(6)'");
            entity.Property(e => e.is_active).HasDefaultValueSql("'1'");
            entity.Property(e => e.updated_at)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("'CURRENT_TIMESTAMP(6)'");

            entity.HasOne(d => d.faculty).WithMany(p => p.departments)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_departments_faculty");
        });

        modelBuilder.Entity<device_token>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.Property(e => e.created_at).HasDefaultValueSql("'CURRENT_TIMESTAMP(6)'");
            entity.Property(e => e.is_active).HasDefaultValueSql("'1'");
            entity.Property(e => e.platform).HasDefaultValueSql("'WEB'");
            entity.Property(e => e.updated_at)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("'CURRENT_TIMESTAMP(6)'");

            entity.HasOne(d => d.user).WithMany(p => p.device_tokens).HasConstraintName("fk_device_tokens_user");
        });

        modelBuilder.Entity<employer>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.Property(e => e.created_at).HasDefaultValueSql("'CURRENT_TIMESTAMP(6)'");
            entity.Property(e => e.status).HasDefaultValueSql("'PENDING'");
            entity.Property(e => e.updated_at)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("'CURRENT_TIMESTAMP(6)'");

            entity.HasOne(d => d.created_byNavigation).WithMany(p => p.employers)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_employers_created_by");
        });

        modelBuilder.Entity<event_category>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.Property(e => e.created_at).HasDefaultValueSql("'CURRENT_TIMESTAMP(6)'");
            entity.Property(e => e.is_active).HasDefaultValueSql("'1'");
            entity.Property(e => e.updated_at)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("'CURRENT_TIMESTAMP(6)'");
        });

        modelBuilder.Entity<event_comment>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.Property(e => e.created_at).HasDefaultValueSql("'CURRENT_TIMESTAMP(6)'");
            entity.Property(e => e.updated_at)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("'CURRENT_TIMESTAMP(6)'");

            entity.HasOne(d => d._event).WithMany(p => p.event_comments).HasConstraintName("fk_event_comments_event");

            entity.HasOne(d => d.parent_comment).WithMany(p => p.Inverseparent_comment)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_event_comments_parent");

            entity.HasOne(d => d.user).WithMany(p => p.event_comments).HasConstraintName("fk_event_comments_user");
        });

        modelBuilder.Entity<event_feedback>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.Property(e => e.created_at).HasDefaultValueSql("'CURRENT_TIMESTAMP(6)'");
            entity.Property(e => e.updated_at)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("'CURRENT_TIMESTAMP(6)'");

            entity.HasOne(d => d._event).WithMany(p => p.event_feedbacks).HasConstraintName("fk_event_feedback_event");

            entity.HasOne(d => d.user).WithMany(p => p.event_feedbacks).HasConstraintName("fk_event_feedback_user");
        });

        modelBuilder.Entity<event_tag>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.Property(e => e.created_at).HasDefaultValueSql("'CURRENT_TIMESTAMP(6)'");
        });

        modelBuilder.Entity<faculty>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.Property(e => e.created_at).HasDefaultValueSql("'CURRENT_TIMESTAMP(6)'");
            entity.Property(e => e.is_active).HasDefaultValueSql("'1'");
            entity.Property(e => e.updated_at)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("'CURRENT_TIMESTAMP(6)'");
        });

        modelBuilder.Entity<interview_booking>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.Property(e => e.booked_at).HasDefaultValueSql("'CURRENT_TIMESTAMP(6)'");
            entity.Property(e => e.status).HasDefaultValueSql("'BOOKED'");

            entity.HasOne(d => d.interview_slot).WithMany(p => p.interview_bookings).HasConstraintName("fk_interview_bookings_slot");

            entity.HasOne(d => d.user).WithMany(p => p.interview_bookings).HasConstraintName("fk_interview_bookings_user");
        });

        modelBuilder.Entity<interview_slot>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.Property(e => e.capacity).HasDefaultValueSql("'1'");
            entity.Property(e => e.created_at).HasDefaultValueSql("'CURRENT_TIMESTAMP(6)'");
            entity.Property(e => e.status).HasDefaultValueSql("'AVAILABLE'");
            entity.Property(e => e.updated_at)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("'CURRENT_TIMESTAMP(6)'");

            entity.HasOne(d => d.job_posting).WithMany(p => p.interview_slots).HasConstraintName("fk_interview_slots_job");

            entity.HasOne(d => d.venue).WithMany(p => p.interview_slots)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_interview_slots_venue");
        });

        modelBuilder.Entity<job_posting>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.Property(e => e.created_at).HasDefaultValueSql("'CURRENT_TIMESTAMP(6)'");
            entity.Property(e => e.job_type).HasDefaultValueSql("'FULL_TIME'");
            entity.Property(e => e.salary_currency).HasDefaultValueSql("'ETB'");
            entity.Property(e => e.status).HasDefaultValueSql("'DRAFT'");
            entity.Property(e => e.updated_at)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("'CURRENT_TIMESTAMP(6)'");
            entity.Property(e => e.workplace_type).HasDefaultValueSql("'ON_SITE'");

            entity.HasOne(d => d.created_byNavigation).WithMany(p => p.job_postings)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_jobs_created_by");

            entity.HasOne(d => d.employer).WithMany(p => p.job_postings).HasConstraintName("fk_jobs_employer");
        });

        modelBuilder.Entity<notification>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.Property(e => e.created_at).HasDefaultValueSql("'CURRENT_TIMESTAMP(6)'");
            entity.Property(e => e.notification_type).HasDefaultValueSql("'SYSTEM'");

            entity.HasOne(d => d.user).WithMany(p => p.notifications).HasConstraintName("fk_notifications_user");
        });

        modelBuilder.Entity<organization>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.Property(e => e.created_at).HasDefaultValueSql("'CURRENT_TIMESTAMP(6)'");
            entity.Property(e => e.organization_type).HasDefaultValueSql("'CLUB'");
            entity.Property(e => e.status).HasDefaultValueSql("'PENDING'");
            entity.Property(e => e.updated_at)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("'CURRENT_TIMESTAMP(6)'");

            entity.HasOne(d => d.department).WithMany(p => p.organizations)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_organizations_department");
        });

        modelBuilder.Entity<organization_member>(entity =>
        {
            entity.HasKey(e => new { e.organization_id, e.user_id }).HasName("PRIMARY");

            entity.Property(e => e.is_active).HasDefaultValueSql("'1'");
            entity.Property(e => e.joined_at).HasDefaultValueSql("'CURRENT_TIMESTAMP(6)'");
            entity.Property(e => e.membership_role).HasDefaultValueSql("'MEMBER'");

            entity.HasOne(d => d.organization).WithMany(p => p.organization_members).HasConstraintName("fk_org_members_organization");

            entity.HasOne(d => d.user).WithMany(p => p.organization_members).HasConstraintName("fk_org_members_user");
        });

        modelBuilder.Entity<permission>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.Property(e => e.created_at).HasDefaultValueSql("'CURRENT_TIMESTAMP(6)'");
            entity.Property(e => e.updated_at)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("'CURRENT_TIMESTAMP(6)'");
        });

        modelBuilder.Entity<poll>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.Property(e => e.created_at).HasDefaultValueSql("'CURRENT_TIMESTAMP(6)'");
            entity.Property(e => e.status).HasDefaultValueSql("'DRAFT'");
            entity.Property(e => e.updated_at)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("'CURRENT_TIMESTAMP(6)'");

            entity.HasOne(d => d.created_byNavigation).WithMany(p => p.polls)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_polls_creator");
        });

        modelBuilder.Entity<poll_option>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.Property(e => e.created_at).HasDefaultValueSql("'CURRENT_TIMESTAMP(6)'");

            entity.HasOne(d => d.poll).WithMany(p => p.poll_options).HasConstraintName("fk_poll_options_poll");
        });

        modelBuilder.Entity<poll_response>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.Property(e => e.responded_at).HasDefaultValueSql("'CURRENT_TIMESTAMP(6)'");

            entity.HasOne(d => d.option).WithMany(p => p.poll_responses).HasConstraintName("fk_poll_responses_option");

            entity.HasOne(d => d.poll).WithMany(p => p.poll_responses).HasConstraintName("fk_poll_responses_poll");

            entity.HasOne(d => d.user).WithMany(p => p.poll_responses).HasConstraintName("fk_poll_responses_user");
        });

        modelBuilder.Entity<registration>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.Property(e => e.registered_at).HasDefaultValueSql("'CURRENT_TIMESTAMP(6)'");
            entity.Property(e => e.status).HasDefaultValueSql("'REGISTERED'");

            entity.HasOne(d => d._event).WithMany(p => p.registrations).HasConstraintName("fk_registrations_event");

            entity.HasOne(d => d.user).WithMany(p => p.registrations).HasConstraintName("fk_registrations_user");
        });

        modelBuilder.Entity<role>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.Property(e => e.created_at).HasDefaultValueSql("'CURRENT_TIMESTAMP(6)'");
            entity.Property(e => e.is_system_role).HasDefaultValueSql("'1'");
            entity.Property(e => e.updated_at)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("'CURRENT_TIMESTAMP(6)'");
        });

        modelBuilder.Entity<role_permission>(entity =>
        {
            entity.HasKey(e => new { e.role_id, e.permission_id }).HasName("PRIMARY");

            entity.Property(e => e.created_at).HasDefaultValueSql("'CURRENT_TIMESTAMP(6)'");

            entity.HasOne(d => d.permission).WithMany(p => p.role_permissions).HasConstraintName("fk_role_permissions_permission");

            entity.HasOne(d => d.role).WithMany(p => p.role_permissions).HasConstraintName("fk_role_permissions_role");
        });

        modelBuilder.Entity<session>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.Property(e => e.last_activity_at).HasDefaultValueSql("'CURRENT_TIMESTAMP(6)'");
            entity.Property(e => e.started_at).HasDefaultValueSql("'CURRENT_TIMESTAMP(6)'");

            entity.HasOne(d => d.user).WithMany(p => p.sessions).HasConstraintName("fk_sessions_user");
        });

        modelBuilder.Entity<study_group>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.Property(e => e.created_at).HasDefaultValueSql("'CURRENT_TIMESTAMP(6)'");
            entity.Property(e => e.group_type).HasDefaultValueSql("'PUBLIC'");
            entity.Property(e => e.status).HasDefaultValueSql("'ACTIVE'");
            entity.Property(e => e.updated_at)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("'CURRENT_TIMESTAMP(6)'");

            entity.HasOne(d => d.created_byNavigation).WithMany(p => p.study_groups)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_study_groups_creator");

            entity.HasOne(d => d.department).WithMany(p => p.study_groups)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_study_groups_department");
        });

        modelBuilder.Entity<study_group_member>(entity =>
        {
            entity.HasKey(e => new { e.study_group_id, e.user_id }).HasName("PRIMARY");

            entity.Property(e => e.is_active).HasDefaultValueSql("'1'");
            entity.Property(e => e.joined_at).HasDefaultValueSql("'CURRENT_TIMESTAMP(6)'");
            entity.Property(e => e.member_role).HasDefaultValueSql("'MEMBER'");

            entity.HasOne(d => d.study_group).WithMany(p => p.study_group_members).HasConstraintName("fk_study_group_members_group");

            entity.HasOne(d => d.user).WithMany(p => p.study_group_members).HasConstraintName("fk_study_group_members_user");
        });

        modelBuilder.Entity<user>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.Property(e => e.account_status).HasDefaultValueSql("'PENDING'");
            entity.Property(e => e.account_type).HasDefaultValueSql("'STUDENT'");
            entity.Property(e => e.created_at).HasDefaultValueSql("'CURRENT_TIMESTAMP(6)'");
            entity.Property(e => e.updated_at)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("'CURRENT_TIMESTAMP(6)'");

            entity.HasOne(d => d.department).WithMany(p => p.users)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_users_department");
        });

        modelBuilder.Entity<user_category_interest>(entity =>
        {
            entity.HasKey(e => new { e.user_id, e.category_id }).HasName("PRIMARY");

            entity.Property(e => e.created_at).HasDefaultValueSql("'CURRENT_TIMESTAMP(6)'");
            entity.Property(e => e.interest_level).HasDefaultValueSql("'MEDIUM'");

            entity.HasOne(d => d.category).WithMany(p => p.user_category_interests).HasConstraintName("fk_user_category_category");

            entity.HasOne(d => d.user).WithMany(p => p.user_category_interests).HasConstraintName("fk_user_category_user");
        });

        modelBuilder.Entity<user_dept_subscription>(entity =>
        {
            entity.HasKey(e => new { e.user_id, e.department_id }).HasName("PRIMARY");

            entity.Property(e => e.subscribed_at).HasDefaultValueSql("'CURRENT_TIMESTAMP(6)'");

            entity.HasOne(d => d.department).WithMany(p => p.user_dept_subscriptions).HasConstraintName("fk_user_dept_sub_department");

            entity.HasOne(d => d.user).WithMany(p => p.user_dept_subscriptions).HasConstraintName("fk_user_dept_sub_user");
        });

        modelBuilder.Entity<user_preference>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.Property(e => e.announcement_notifications).HasDefaultValueSql("'1'");
            entity.Property(e => e.career_notifications).HasDefaultValueSql("'1'");
            entity.Property(e => e.comment_notifications).HasDefaultValueSql("'1'");
            entity.Property(e => e.created_at).HasDefaultValueSql("'CURRENT_TIMESTAMP(6)'");
            entity.Property(e => e.email_notifications).HasDefaultValueSql("'1'");
            entity.Property(e => e.event_reminders).HasDefaultValueSql("'1'");
            entity.Property(e => e.preferred_language).HasDefaultValueSql("'en'");
            entity.Property(e => e.push_notifications).HasDefaultValueSql("'1'");
            entity.Property(e => e.reminder_minutes).HasDefaultValueSql("'30'");
            entity.Property(e => e.timezone).HasDefaultValueSql("'Africa/Addis_Ababa'");
            entity.Property(e => e.updated_at)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("'CURRENT_TIMESTAMP(6)'");

            entity.HasOne(d => d.user).WithOne(p => p.user_preference).HasConstraintName("fk_user_preferences_user");
        });

        modelBuilder.Entity<user_role>(entity =>
        {
            entity.HasKey(e => new { e.user_id, e.role_id }).HasName("PRIMARY");

            entity.Property(e => e.assigned_at).HasDefaultValueSql("'CURRENT_TIMESTAMP(6)'");

            entity.HasOne(d => d.assigned_byNavigation).WithMany(p => p.user_roleassigned_byNavigations)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_user_roles_assigned_by");

            entity.HasOne(d => d.role).WithMany(p => p.user_roles).HasConstraintName("fk_user_roles_role");

            entity.HasOne(d => d.user).WithMany(p => p.user_roleusers).HasConstraintName("fk_user_roles_user");
        });

        modelBuilder.Entity<venue>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.Property(e => e.capacity).HasDefaultValueSql("'1'");
            entity.Property(e => e.created_at).HasDefaultValueSql("'CURRENT_TIMESTAMP(6)'");
            entity.Property(e => e.status).HasDefaultValueSql("'AVAILABLE'");
            entity.Property(e => e.updated_at)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("'CURRENT_TIMESTAMP(6)'");
            entity.Property(e => e.venue_type).HasDefaultValueSql("'CLASSROOM'");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
