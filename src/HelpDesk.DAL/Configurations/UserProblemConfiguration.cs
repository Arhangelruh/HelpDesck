using HelpDesk.Common.Constants;
using HelpDesk.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace HelpDesk.DAL.Configurations
{
    /// <summary>
    /// EF Configuration for TransactionProfiles entity.
    /// </summary>
    class UserProblemConfiguration : IEntityTypeConfiguration<UserProblem>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<UserProblem> builder)
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));

            builder.ToTable(TableConstants.UserProblems)
                .HasKey(userproblems => userproblems.Id);

            builder.HasOne(UserProblems => UserProblems.Problem)
                .WithMany(problem => problem.UsersProblem)
                .HasForeignKey(UserProblems => UserProblems.ProblemId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(UserProblems => UserProblems.Profile)
                .WithMany(profile => profile.UsersProblem)
                .HasForeignKey(UserProblems => UserProblems.ProfileId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
