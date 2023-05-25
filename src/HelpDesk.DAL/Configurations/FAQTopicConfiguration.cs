using HelpDesk.Common.Constants;
using HelpDesk.DAL.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;

namespace HelpDesk.DAL.Configurations
{
    public class FAQTopicConfiguration : IEntityTypeConfiguration<FAQTopic>
    {
        public void Configure(EntityTypeBuilder<FAQTopic> builder)
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));

            builder.ToTable(TableConstants.FAQTopic)
               .HasKey(FAQTopic => FAQTopic.Id);

            builder.Property(FAQTopic => FAQTopic.Theme)
                .IsRequired()
                .HasMaxLength(ConfigurationContants.SqlMaxLengthMedium);
        }
    }
}
