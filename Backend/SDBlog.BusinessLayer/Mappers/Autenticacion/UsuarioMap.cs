using AutoMapper;
using SDBlog.BusinessLayer.Dtos.Response;
using SDBlog.DataModel.Autenticacion;

namespace SDBlog.BusinessLayer.Mappers.Autenticacion
{
    public class UsuarioMap : Profile
    {
        public UsuarioMap()
        {
            CreateMap<Usuario, UsuarioDto>().ReverseMap();
        }
    }
}
