using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SDBlog.Core.Base;
using System.Threading.Tasks;

namespace SDBlog.Api.Controllers.Base
{
    public interface IBaseController<TDto> where TDto : DtoBase
    {
        Task<IActionResult> Get();
        Task<IActionResult> GetById(int id);
        Task<IActionResult> Post([FromBody] TDto dto);
        Task<IActionResult> Put(int key, [FromBody] TDto dto);
        Task<IActionResult> Delete(int id);
        Task<IActionResult> Patch(int id, [FromBody] JsonPatchDocument<TDto> jsonPatch);
        Task<IActionResult> Paginator([FromQuery] PaginatorBase paginFilter);
    }
}