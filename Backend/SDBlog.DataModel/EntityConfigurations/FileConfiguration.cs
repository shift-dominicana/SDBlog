using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SDBlog.DataModel.Entities.Files;

namespace Boundaries.Database.Configurations
{
    public sealed class FileConfiguration : IEntityTypeConfiguration<FileEntity>
    {
        void IEntityTypeConfiguration<FileEntity>.Configure(EntityTypeBuilder<FileEntity> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(300);
            builder.Property(x => x.Path).HasMaxLength(500);
            builder.Property(x => x.Year).IsRequired().HasMaxLength(4);
            builder.Property(x => x.Borrado).HasDefaultValue(false);
        }
    }
}
