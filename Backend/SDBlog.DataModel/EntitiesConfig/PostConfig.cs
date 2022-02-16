using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SDBlog.DataModel.Entities;

namespace SDBlog.DataModel.EntitiesConfig
{
    public class PostConfig : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.ToTable("Posts");
            builder.HasKey(x => x.Id).HasName("FK_PostId");
            builder.Property(x => x.Id).HasColumnName("PostId");
            builder.Property(x => x.Title).IsRequired(true).HasMaxLength(100);
            builder.Property(x => x.Content).IsRequired(true);
            builder.Property(x => x.PublicDate).IsRequired(true);
            builder.Property(x => x.Link).IsRequired(true).HasMaxLength(150);

            builder.HasOne(x => x.User)
                   .WithMany(x => x.Posts)
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK_Post_User");

            builder.HasOne(x => x.Category)
                   .WithMany(x => x.Posts)
                   .HasForeignKey(x => x.CategoryId)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK_Post_Category");

            builder.HasMany(x => x.PostTags)
                .WithOne(x => x.Post);
        }
    }
}
