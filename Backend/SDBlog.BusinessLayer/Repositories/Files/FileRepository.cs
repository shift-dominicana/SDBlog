using Core.Interfaces;
using SDBlog.BusinessLayer.Interfaces.Files;
using SDBlog.BusinessLayer.Services.Base;
using SDBlog.DataModel.Context;
using SDBlog.DataModel.Entities.Files;

namespace SDBlog.BusinessLayer.Services.Files
{
    public sealed class FileRepository : FileBaseRepository<FileEntity>, IFileRepository
    {
        public FileRepository(MainDbContext mainDbContext) : base(mainDbContext)
        {

        }
    }
}
