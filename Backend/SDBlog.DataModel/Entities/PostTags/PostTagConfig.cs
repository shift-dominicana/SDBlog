using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDBlog.DataModel.Entities.PostTags
{
    public class PostTagConfig : IEntityTypeConfiguration<PostTag>
    {
        public void Configure(EntityTypeBuilder<PostTag> builder)
        {
            builder.ToTable("PostTag");
            builder.HasKey(x => x.Id).HasName("FK_PostTagId");
            builder.Property(x => x.Id).HasColumnName("PostTagId");
            builder.HasIndex(x => new { x.PostId, x.TagId}).IsUnique();

            builder.HasOne(x => x.Post)
                   .WithMany()
                   .HasForeignKey(x => x.PostId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("FK_PostTag_Post");

            builder.HasOne(x => x.Tag)
                   .WithMany()
                   .HasForeignKey(x => x.TagId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("FK_PostTag_Tag");
        }
    }
}
