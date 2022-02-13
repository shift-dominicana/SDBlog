using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SDBlog.BusinessLayer.Dtos.Files;
using SDBlog.BusinessLayer.Services.Files;
using SDBlog.Core.Classes;
using SDBlog.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xmera.FL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly FileDirectoryService _fileService;

        public FileController(FileDirectoryService fileApi)
        {
            _fileService = fileApi;
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file, int documentTypeId)
        {
            IOperationResult<long> operationResult = await _fileService.UploadFile(file, documentTypeId);

            if (!operationResult.Success)
            {
                return BadRequest(operationResult.ErrorMessage);
            }

            return Ok(operationResult.Entity);
        }

        [HttpPost("UploadFileList")]
        public async Task<IActionResult> UploadFileList(List<FileUploadDto> fileList)
        {
            IOperationResult<object> operationResult = null; 
            try
            {
                operationResult = await _fileService.UploadFileList(fileList);

                if (!operationResult.Success)
                {
                    return BadRequest(operationResult.ErrorMessage);
                }
            
                return Ok(operationResult.Entity);

            }
            catch (Exception ex)
            {
                return BadRequest(operationResult.Entity);
            }

        }



        [HttpGet]
        public async Task<IActionResult> Download(long id)
        {
            IOperationResult<FileDto> fileResult = await _fileService.DownloadFile(id);

            if (!fileResult.Success)
            {
                return BadRequest(fileResult.ErrorMessage);
            }

            return File(fileResult.Entity.Data, fileResult.Entity.ContentType, fileResult.Entity.DownloadName);
        }
    }
}
