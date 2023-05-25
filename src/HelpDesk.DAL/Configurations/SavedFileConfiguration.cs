using HelpDesk.Common.Constants;
using HelpDesk.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace HelpDesk.DAL.Configurations
{
    class SavedFileConfiguration : IEntityTypeConfiguration<SavedFile>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<SavedFile> builder)
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));

            builder.ToTable(TableConstants.SavedFile)
                .HasKey(savedFile => savedFile.Id);

            builder.Property(savedFile => savedFile.Name)
                .HasMaxLength(ConfigurationContants.SqlMaxLengthMedium)
                .IsRequired();

            builder.Property(savedFile => savedFile.ContentType)
                .HasMaxLength(ConfigurationContants.SqlMaxLengthShort)
                .IsRequired();

            builder.Property(savedFile => savedFile.FileBody)
                .IsRequired();

            builder.HasOne(savedFile => savedFile.Problem)
                .WithMany(problem => problem.SavedFiles)
                .HasForeignKey(savedFile => savedFile.ProblemId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
