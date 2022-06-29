using HelpDesk.Common.Constants;
using HelpDesk.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace HelpDesk.DAL.Configurations
{
    /// <summary>
    /// EF Configuration for Problem entity.
    /// </summary>
    class ProblemConfiguration : IEntityTypeConfiguration<Problem>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<Problem> builder)
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));

            builder.ToTable(TableConstants.Problems)
                .HasKey(problem => problem.Id);

            builder.Property(problem => problem.Theme)
                .HasMaxLength(ConfigurationContants.SqlMaxLengthMedium);

            builder.Property(problem => problem.Description)
                .HasMaxLength(ConfigurationContants.SqlMaxLengthLong);

            builder.Property(problem => problem.Ip)
                .HasMaxLength(ConfigurationContants.SqlMaxLengthMedium);

            builder.Property(problem => problem.IncomingDate)
                .HasColumnType("Timestamp");

            builder.Property(problem => problem.CloseDate)
                .HasColumnType("Timestamp");

            builder.HasOne(problem => problem.Status)
                .WithMany(status => status.Problems)
                .HasForeignKey(problem => problem.StatusId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
