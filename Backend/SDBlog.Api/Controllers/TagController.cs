using AutoMapper;
using SDBlog.Api.Controllers.Base;
using SDBlog.BusinessLayer.Dtos.Tags;
using SDBlog.BusinessLayer.Interfaces;
using SDBlog.DataModel.Entities;

namespace SDBlog.Api.Controllers
{
    public class TagController : BaseController<Tag, TagDto>
    {
        public TagController(ITagService service, IMapper mapper) : base(service, mapper)
        {
        }
    }
}
