using AutoMapper;
using SDBlog.BusinessLayer.Dtos.Response;
using SDBlog.DataModel.Autenticacion;

namespace SDBlog.BusinessLayer.Mappers.Autenticacion
{
    public class PermisoMap : Profile
    {
        public PermisoMap()
        {
            CreateMap<Permiso, PermisoDto>().ReverseMap();
        }
    }
}
