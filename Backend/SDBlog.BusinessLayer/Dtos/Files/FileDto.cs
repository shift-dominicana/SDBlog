namespace SDBlog.BusinessLayer.Dtos.Files
{
    public sealed class FileDto
    {
        public byte[] Data { get; set; }

        public string ContentType { get; set; }

        public string DownloadName { get; set; }
    }
}
