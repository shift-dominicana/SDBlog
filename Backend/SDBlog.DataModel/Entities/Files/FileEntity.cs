using SDBlog.Core.Base;
using System;

namespace SDBlog.DataModel.Entities.Files
{
    public sealed class FileEntity : EntityAuditableBase
    {
        public string Name { get; set; }

        public string Path { get; set; }

        public string Year { get; } = DateTime.Now.Year.ToString();

        public FileType FileType { get; set; }
    }
}
