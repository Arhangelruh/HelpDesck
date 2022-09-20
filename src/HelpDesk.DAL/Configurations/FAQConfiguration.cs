using HelpDesk.Common.Constants;
using HelpDesk.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace HelpDesk.DAL.Configurations
{
    internal class FAQConfiguration : IEntityTypeConfiguration<FAQ>
    {
        public void Configure(EntityTypeBuilder<FAQ> builder)
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));

            builder.ToTable(TableConstants.FAQ)
               .HasKey(FAQ => FAQ.Id);

            builder.Property(FAQ => FAQ.Theme)
                .IsRequired()
                .HasMaxLength(ConfigurationContants.SqlMaxLengthMedium);

            builder.Property(FAQ => FAQ.Description)
                .HasMaxLength(ConfigurationContants.SqlMaxLengthLongForDescription)
                .IsRequired();
        }
    }
}
