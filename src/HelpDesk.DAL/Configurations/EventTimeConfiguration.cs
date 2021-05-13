using HelpDesk.Common.Constants;
using HelpDesk.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace HelpDesk.DAL.Configurations
{
    /// <summary>
    /// EF Configuration for EventTimer entity.
    /// </summary>
    public class EventTimeConfiguration : IEntityTypeConfiguration<EventTime>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<EventTime> builder)
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));

            builder.ToTable(TableConstants.EventTime, ShemeConstants.Event)
                .HasKey(status => status.Id);

            builder.Property(EventTime => EventTime.Time)
                .IsRequired();
        }
    }
}

