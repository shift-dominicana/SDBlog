using AutoMapper;

namespace SDBlog.BusinessLayer.Mappers
{
    public class ListaGenericaMap : Profile
    {
        public ListaGenericaMap()
        {
            /*CreateMap<FuenteDemanda, DropdownResponse>()
                .ForMember(x => x.Id, m => m.MapFrom(p => p.Id.ToString()))
                .ForMember(x => x.Name, m => m.MapFrom(p => p.Nombre));
            CreateMap<PoliticaPNPSP, DropdownResponse>()
                .ForMember(x => x.Id, m => m.MapFrom(p => p.PoliticaPNPSPId.ToString()))
                .ForMember(x => x.Name, m => m.MapFrom(p => p.Nombre));*/
        }
    }
}
