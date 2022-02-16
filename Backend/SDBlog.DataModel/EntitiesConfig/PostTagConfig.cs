    using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SDBlog.DataModel.Entities;

namespace SDBlog.DataModel.EntitiesConfig
{
    public class PostTagConfig : IEntityTypeConfiguration<PostTag>
    {
        public void Configure(EntityTypeBuilder<PostTag> builder)
        {
            builder.ToTable("PostTag");
            builder.HasKey(x => x.Id).HasName("PK_PostTagId");
            builder.Property(x => x.Id).HasColumnName("PostTagId");
            builder.HasIndex(x => new { x.PostId, x.TagId }).IsUnique();

            builder.HasOne(x => x.Tag)
                   .WithMany()
                   .HasForeignKey(x => x.TagId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("FK_PostTag_Tag");
        }
    }
}
