using HelpDesk.Common.Constants;
using HelpDesk.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace HelpDesk.DAL.Configurations
{
    /// <summary>
    /// EF Configuration for Status entity.
    /// </summary>
    class StatusConfiguration : IEntityTypeConfiguration<Status>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<Status> builder)
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));

            builder.ToTable(TableConstants.Statuses)
                .HasKey(status => status.Id);

            builder.Property(status => status.StatusName)
                .HasMaxLength(ConfigurationContants.SqlMaxLengthMedium);

        }
    }
}
