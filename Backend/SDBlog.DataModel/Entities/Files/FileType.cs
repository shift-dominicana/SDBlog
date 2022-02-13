using SDBlog.Core.Base;

namespace SDBlog.DataModel.Entities.Files
{
    public sealed class FileType : EntityAuditableBase
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
