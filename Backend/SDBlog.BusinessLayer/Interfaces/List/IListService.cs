using SDBlog.BusinessLayer.Dtos.Request;
using SDBlog.BusinessLayer.Dtos.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SDBlog.BusinessLayer.Interfaces.List
{
    public interface IListService
    {
        Task<IList<DropdownResponse>> GetDropdownAsync(DropdownRequest model);
    }
}
