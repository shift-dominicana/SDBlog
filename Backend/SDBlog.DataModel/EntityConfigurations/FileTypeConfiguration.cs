using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SDBlog.DataModel.Entities.Files;

namespace Boundaries.Database.Configurations
{
    public sealed class FileTypeConfiguration : IEntityTypeConfiguration<FileType>
    {
        public void Configure(EntityTypeBuilder<FileType> builder)
        {
            builder.HasKey(fileType => fileType.Id);
            builder.Property(fileType => fileType.Name).IsRequired().HasMaxLength(30);

            builder.HasData(
                new FileType { Id = 1, Name = "Identificación" },
                new FileType { Id = 2, Name = "Acta de nacimiento" }
                );
        }
    }
}
