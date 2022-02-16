using AutoMapper;
using SDBlog.Api.Controllers.Base;
using SDBlog.BusinessLayer.Dtos.PostTags;
using SDBlog.BusinessLayer.Interfaces;
using SDBlog.DataModel.Entities;

namespace SDBlog.Api.Controllers
{
    public class PostTagController : BaseController<PostTag, PostTagDto>
    {
        public PostTagController(IPostTagService service, IMapper mapper) : base(service, mapper)
        {
        }
    }
}
