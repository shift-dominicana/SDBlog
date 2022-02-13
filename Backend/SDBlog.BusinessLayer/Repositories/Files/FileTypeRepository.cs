using Core.Interfaces;
using SDBlog.DataModel.Context;
using SDBlog.DataModel.Entities.Files;

namespace Boundaries.Database.Repositories
{
    public sealed class FileTypeRepository : BaseFileRepository<FileType>, IFileTypeRepository
    {
        public FileTypeRepository(MainDbContext mainDbContext) : base(mainDbContext)
        {

        }
    }
}
