using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SDBlog.BusinessLayer.Dtos.Files;
using SDBlog.BusinessLayer.Interfaces.Files;
using SDBlog.BusinessLayer.Settings;
using SDBlog.Core.Classes;
using SDBlog.Core.Interfaces;
using SDBlog.DataModel.Entities.Files;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SDBlog.BusinessLayer.Services.Files
{
    public sealed class FileDirectoryService
    {
        private readonly IFileService _fileService;
        private readonly IFileRepository _fileRepository;
        private readonly IFileTypeRepository _fileTypeRepository;
        private readonly FileSettings _fileSettings;

        public FileDirectoryService(IFileService fileService, IFileRepository fileRepository, IFileTypeRepository fileTypeRepository, IOptions<FileSettings> fileSettings)
        {
            _fileService = fileService;
            _fileRepository = fileRepository;
            _fileTypeRepository = fileTypeRepository;
            _fileSettings = fileSettings.Value;
        }

        public async Task<IOperationResult<long>> UploadFile(IFormFile fileToCreate, int fileTypeId)
        {
            try
            {
                ValidateFile(fileToCreate);

                FileEntity file = await PersistFile(fileToCreate, fileTypeId);
                FileCreateDto fileToUpload = BuildFileToUpload(file, fileToCreate);

                IOperationResult<string> filePathResult = await _fileService.UploadFile(fileToUpload);

                if (!filePathResult.Success)
                {
                    await InactivateFile(file);
                    return OperationResult<long>.Fail("Ha ocurrido un error guardando el documento en el servidor.", filePathResult.ErrorDetail);
                }

                await SetFilePath(file, filePathResult.Entity);

                return OperationResult<long>.Ok(file.Id);
            }
            catch (Exception ex)
            {
                return OperationResult<long>.Fail(ex.Message, ex.ToString());
            }
        }

        public async Task<IOperationResult<object>> UploadFileList(List<FileUploadDto> files)
        {
            List<FileCreateDto> fileList = PrepareFileList(files);
            
            Response  result = ValidateFiles(fileList);
            
            if (!result.Status) 
            {
                Response responseError = new()
                {
                    Status = false,
                    Message = ((FormFile)result.Object).FileName,
                    MessageDetail = result.Message
                };
                return OperationResult<object>.Ok(responseError);
            }

            var saveResult = await SaveFileList(fileList);

            Response respuesta = new()
            {
                Status = true,
                Message = "Los archivos fueron almacenados exitosamente.",
                MessageDetail = ""
            };
            return OperationResult<object>.Ok(respuesta);
        }

        private List<FileCreateDto> PrepareFileList(List<FileUploadDto> files) 
        {
            List<FileCreateDto> formFileList = new List<FileCreateDto>();

            foreach (FileUploadDto newFile in files)
            {

                byte[] fileBytes = Convert.FromBase64String(newFile.Base64);

                string filePath = Path.Combine(_fileSettings.PrincipalPath, _fileSettings.PrincipalFolderName, newFile.FileName);
                MemoryStream memoryStream = new MemoryStream();
                memoryStream.Write(fileBytes, 0, fileBytes.Length);

                FormFile fileData = new FormFile(memoryStream, 0, memoryStream.Length, newFile.FileName, newFile.FileName);

                FileCreateDto fileDto = new()
                {
                    FileId = 0,
                    Data = fileData,
                    FileName = newFile.FileName,
                    FileTypeId = newFile.FileTypeId
                };
                formFileList.Add(fileDto);
            }

            return formFileList;
        }

        private Response ValidateFiles(List<FileCreateDto> fileList)
        {
            foreach (FileCreateDto fileObj in fileList)
            {
                IFormFile file = fileObj.Data;
                try
                {
                    ValidateFile(file);
                }
                catch (Exception exception) 
                {
                    return new Response { Status = false, Message = exception.Message, Object = file };    
                }
            }
            return new Response { Status = true, Message = ""};
        }

        private async Task<IOperationResult<long>> SaveFileList(List<FileCreateDto> fileList)
        {
            foreach (FileCreateDto file in fileList) 
            {
                FileEntity fileEntity = await PersistFile(file.Data, file.FileTypeId);
                FileCreateDto fileToUpload = BuildFileToUpload(fileEntity, file.Data);

                IOperationResult<string> filePathResult = await _fileService.UploadFile(fileToUpload);

                if (!filePathResult.Success)
                {
                    await InactivateFile(fileEntity);
                    return OperationResult<long>.Fail("Ha ocurrido un error guardando el documento en el servidor.", filePathResult.ErrorDetail);
                }

                await SetFilePath(fileEntity, filePathResult.Entity);
            }

            return OperationResult<long>.Ok(200);

        }

        private FileCreateDto BuildFileToUpload(FileEntity file, IFormFile fileToCreate)
        {
            return new FileCreateDto
            {
                FileId = file.Id,
                Data = fileToCreate,
                // DocumentTypeId = file.FileType.FileTypeId
            };
        }

        private async Task InactivateFile(FileEntity file)
        {
            try
            {
                file.Borrado = true;
                await _fileRepository.SaveAsync();
            }
            catch (Exception)
            {
                throw new Exception("Ha ocurrido un error guardando el estado del archivo.");
            }
        }

        private async Task SetFilePath(FileEntity file, string path)
        {
            try
            {
                file.Path = path;
                await _fileRepository.SaveAsync();
            }
            catch (Exception)
            {
                throw new Exception("Ha ocurrido un error guardando la ruta del archivo.");
            }
        }

        private async Task<FileEntity> PersistFile(IFormFile fileToCreate, int fileTypeId)
        {
            FileType fileType = await _fileTypeRepository.FindAsync(fileType => fileType.Id == fileTypeId);

            if (fileType == null)
            {
                throw new Exception("El tipo de documento es incorrecto.");
            }

            try
            {
                var file = new FileEntity
                {
                    Name = fileToCreate.FileName,
                    FileType = fileType
                };

                return await _fileRepository.Create(file);
            }
            catch
            {
                throw new Exception("Ha ocurrido un error guardando el documento en la base de datos.");
            }
        }

        private void ValidateFile(IFormFile fileToCreate)
        {
            if (fileToCreate == null)
            {
                throw new Exception("No ha enviado ningun archivo.");
            }

            IOperationResult<string> fileExtensionResult = _fileService.GetFileExtension(fileToCreate);

            if (!fileExtensionResult.Success)
            {
                throw new Exception(fileExtensionResult.ErrorMessage);
            }

            if (!_fileSettings.AllowedExtensions.Contains(fileExtensionResult.Entity))
            {
                throw new Exception("La extención del archivo no es permitida.");
            }

            IOperationResult<long> fileSizeResult = _fileService.GetFileSize(fileToCreate);

            if (!fileSizeResult.Success)
            {
                throw new Exception("Ha ocurrido un error obteniendo el tamaño del archivo.");
            }

            if (fileSizeResult.Entity > _fileSettings.MaxFileSize)
            {
                throw new Exception("El tamaño del archivo supera el limite.");
            }
        }


        public async Task<IOperationResult<FileDto>> DownloadFile(long fileId)
        {
            FileEntity file = await _fileRepository.FindAsync(file => file.Id == fileId &&  !file.Borrado);

            if (file == null)
            {
                return OperationResult<FileDto>.Fail("El archivo no existe");
            }

            IOperationResult<byte[]> fileResult = await _fileService.GetFileAsync(file.Path);

            FileDto FileDto = new FileDto
            {
                Data = fileResult.Entity,
                ContentType = GetContentType(file.Path),
                DownloadName = file.Name
            };

            return OperationResult<FileDto>.Ok(FileDto);
        }

        private string GetContentType(string path)
        {
            IOperationResult<string> fileExtension = _fileService.GetFileExtension(path);

            if (!fileExtension.Success)
            {
                throw new Exception("Ha ocurrido un error obteniendo el tipo de archivo.");
            }

            switch (fileExtension.Entity)
            {
                case ".pdf":
                    return "application/pdf";

                case ".jpeg":
                    return "image/jpeg";

                case ".jpg":
                    return "image/jpeg";

                case ".doc":
                    return "application/msword";

                case "xls":
                    return "application/vnd.ms-excel";

                default:
                    return "application/octet-stream";
            }
        }
    }
}
