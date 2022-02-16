using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SDBlog.DataModel.Entities;

namespace SDBlog.DataModel.EntitiesConfig
{
    public class TagConfig : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.ToTable("Tags");
            builder.HasKey(x => x.Id).HasName("FK_TagId");
            builder.Property(x => x.Id).HasColumnName("TagId");
            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Description).IsRequired().HasMaxLength(150);
        }
    }
}
