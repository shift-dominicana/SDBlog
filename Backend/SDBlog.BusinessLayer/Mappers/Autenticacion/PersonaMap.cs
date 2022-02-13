using AutoMapper;
using SDBlog.BusinessLayer.Dtos.Response;
using SDBlog.DataModel.Autenticacion;

namespace SDBlog.BusinessLayer.Mappers.Autenticacion
{
    public class PersonaMap : Profile
    {
        public PersonaMap()
        {
            CreateMap<Persona, PersonaDto>().ReverseMap();
        }
    }
}
