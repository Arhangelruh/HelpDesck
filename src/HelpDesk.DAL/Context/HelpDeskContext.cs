using HelpDesk.DAL.Configurations;
using HelpDesk.DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace HelpDesk.DAL.Context
{
    /// <summary>
    /// Database context.
    /// </summary>
    public class HelpDeskContext : IdentityDbContext<User>
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="options">Database context options.</param>
        public HelpDeskContext(DbContextOptions<HelpDeskContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Profiles.
        /// </summary>
        public DbSet<Profile> Profiles { get; set; }

        /// <summary>
        /// Statuses.
        /// </summary>
        public DbSet<Status> Statuses { get; set; }

        /// <summary>
        /// Problems.
        /// </summary>
        public DbSet<Problem> Problems { get; set; }

        /// <summary>
        /// UserProblem.
        /// </summary>
        public DbSet<UserProblem> UserProblems { get; set; }

        /// <summary>
        /// Comments.
        /// </summary>
        public DbSet<Comments> Comments { get; set; }

        /// <summary>
        /// SavedFiles.
        /// </summary>
        public DbSet<SavedFile> SavedFile { get; set; }

        /// <summary>
        /// FAQs.
        /// </summary>
        public DbSet<FAQ> FAQ { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder = modelBuilder ?? throw new ArgumentNullException(nameof(modelBuilder));

            modelBuilder.ApplyConfiguration(new ProfileConfiguration());
            modelBuilder.ApplyConfiguration(new StatusConfiguration());
            modelBuilder.ApplyConfiguration(new ProblemConfiguration());
            modelBuilder.ApplyConfiguration(new UserProblemConfiguration());
            modelBuilder.ApplyConfiguration(new CommentsConfiguration());
            modelBuilder.ApplyConfiguration(new SavedFileConfiguration());
            modelBuilder.ApplyConfiguration(new FAQConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
