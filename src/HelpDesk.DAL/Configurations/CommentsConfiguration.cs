﻿using HelpDesk.Common.Constants;
using HelpDesk.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace HelpDesk.DAL.Configurations
{
    class CommentsConfiguration : IEntityTypeConfiguration<Comments>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<Comments> builder)
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));

            builder.ToTable(TableConstants.Comments)
                .HasKey(comment => comment.Id);

            builder.Property(comment => comment.CreateComment)
                .IsRequired();

            builder.Property(comment => comment.Comment)
                .HasMaxLength(ConfigurationContants.SqlMaxLengthMedium)
                .IsRequired();

            builder.HasOne(Comments => Comments.Problem)
                .WithMany(problem => problem.Comments)
                .HasForeignKey(comment => comment.ProblemId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
