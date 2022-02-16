using AutoMapper;
using SDBlog.Api.Controllers.Base;
using SDBlog.BusinessLayer.Dtos.Posts;
using SDBlog.BusinessLayer.Interfaces;
using SDBlog.DataModel.Entities;

namespace SDBlog.Api.Controllers
{
    public class PostController : BaseController<Post, PostDto>
    {
        public PostController(IPostService service, IMapper mapper) : base(service, mapper)
        {
        }
    }
}
