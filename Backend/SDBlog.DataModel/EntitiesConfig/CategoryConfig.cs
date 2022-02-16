using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SDBlog.DataModel.Entities;

namespace SDBlog.DataModel.EntitiesConfig
{
    public class CategoryConfig : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");
            builder.HasKey(x => x.Id).HasName("FK_CategoryId");
            builder.Property(x => x.Id).HasColumnName("CategoryId");
            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Description).IsRequired().HasMaxLength(150);
        }
    }
}
