

using SDBlog.BusinessLayer.Interfaces.Base;
using SDBlog.DataModel.Entities.Files;

namespace SDBlog.BusinessLayer.Interfaces.Files
{
    public interface IFileRepository : IFileBaseRepository<FileEntity>
    {
    }
}
