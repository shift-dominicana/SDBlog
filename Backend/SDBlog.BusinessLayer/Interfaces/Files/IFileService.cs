using Microsoft.AspNetCore.Http;
using SDBlog.BusinessLayer.Dtos.Files;
using SDBlog.Core.Interfaces;
using System.Threading.Tasks;

namespace SDBlog.BusinessLayer.Interfaces.Files
{
    public interface IFileService
    {
        Task<IOperationResult<string>> UploadFile(FileCreateDto file);

        IOperationResult<string> GetFileExtension(IFormFile file);

        IOperationResult<string> GetFileExtension(string path);

        IOperationResult<long> GetFileSize(IFormFile file);

        Task<IOperationResult<byte[]>> GetFileAsync(string path);
    }
}
