namespace SDBlog.BusinessLayer.Dtos.Files
{
    public sealed class FileUploadDto
    {
        public long FileId { get; set; }

        public string Base64 { get; set; }

        public string FileName { get; set; }

        public int FileTypeId { get; set; }
    }
}
