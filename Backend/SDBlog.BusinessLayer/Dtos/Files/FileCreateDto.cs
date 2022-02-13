using Microsoft.AspNetCore.Http;

namespace SDBlog.BusinessLayer.Dtos.Files
{
    public sealed class FileCreateDto
    {
        public long FileId { get; set; }
        public IFormFile Data { get; set; }
        public int FileTypeId { get; set; }
        public string FileName { get; set; }
    }
}
