using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SDBlog.BusinessLayer.Dtos.Files;
using SDBlog.BusinessLayer.Interfaces.Files;
using SDBlog.BusinessLayer.Settings;
using SDBlog.Core.Classes;
using SDBlog.Core.Enums;
using SDBlog.Core.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SDBlog.BusinessLayer.Services.Files
{
    public sealed class FileService : IFileService
    {
        private readonly FileSettings _fileSettings;

        public FileService(IOptions<FileSettings> fileSettings)
        {
            _fileSettings = fileSettings.Value;
        }

        async Task<IOperationResult<string>> IFileService.UploadFile(FileCreateDto file)
        {
            try
            {
                string extension = Path.GetExtension(file.Data.FileName);
                string principalFolderPath = Path.Combine(_fileSettings.PrincipalPath, _fileSettings.PrincipalFolderName);
                string yearFolderPath = Path.Combine(principalFolderPath, DateTime.Now.Year.ToString());
                string contentFolderPath = Path.Combine(yearFolderPath, file.FileTypeId.ToString());
                string fileName = $"{file.FileId}{extension}";
                string filePath = Path.Combine(contentFolderPath, fileName);

                CreateFolders(principalFolderPath, yearFolderPath, contentFolderPath);

                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.Data.CopyToAsync(fileStream);
                }

                return OperationResult<string>.Ok(filePath);
            }
            catch (Exception ex)
            {
                return OperationResult<string>.Fail("Ha ocurrido un error guardando el archivo", ex.ToString());
            }
        }

        private void CreateFolders(string principalFolderPath, string yearFolderPath, string contentFolderPath)
        {
            if (!Directory.Exists(principalFolderPath))
            {
                Directory.CreateDirectory(contentFolderPath);
            }

            if (!Directory.Exists(yearFolderPath))
            {
                Directory.CreateDirectory(contentFolderPath);
            }

            if (!Directory.Exists(contentFolderPath))
            {
                Directory.CreateDirectory(contentFolderPath);
            }
        }

        public IOperationResult<string> GetFileExtension(IFormFile file)
        {
            try
            {
                string fileExtension = Path.GetExtension(file.FileName);
                return OperationResult<string>.Ok(fileExtension);
            }
            catch (Exception ex)
            {
                return OperationResult<string>.Fail("Ha ocurrido un error obteniendo la extension del archivo", ex.ToString());
            }
        }

        public IOperationResult<string> GetFileExtension(string path)
        {
            try
            {
                string fileExtension = Path.GetExtension(path);
                return OperationResult<string>.Ok(fileExtension);
            }
            catch (Exception ex)
            {
                return OperationResult<string>.Fail("Ha ocurrido un error obteniendo la extension del archivo", ex.ToString());
            }
        }

        public IOperationResult<long> GetFileSize(IFormFile file)
        {
            long fileSize = file.Length / (long)SizeTypes.MegaByte;

            return OperationResult<long>.Ok(fileSize);
        }

        async Task<IOperationResult<byte[]>> IFileService.GetFileAsync(string path)
        {
            using (var stream = new FileStream(path, FileMode.Open))
            {
                using (var memoryStream = new MemoryStream())
                {
                    await stream.CopyToAsync(memoryStream);
                    return OperationResult<byte[]>.Ok(memoryStream.GetBuffer());
                }
            }
        }
    }
}
