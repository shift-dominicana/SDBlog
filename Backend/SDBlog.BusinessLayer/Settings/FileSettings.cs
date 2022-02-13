
using System.Collections.Generic;

namespace SDBlog.BusinessLayer.Settings
{
    public sealed class FileSettings
    {
        public string PrincipalPath { get; set; }

        public string PrincipalFolderName { get; set; }

        public IEnumerable<string> AllowedExtensions { get; set; }

        public long MaxFileSize { get; set; }
    }
}
