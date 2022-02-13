using AutoMapper;
using SDBlog.BusinessLayer.Dtos.Request;
using SDBlog.BusinessLayer.Dtos.Response;
using SDBlog.BusinessLayer.Interfaces.List;
using SDBlog.DataModel.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDBlog.BusinessLayer.Services.Listas
{
    public class ListService : IListService
    {
        private readonly MainDbContext _context;
        private readonly IMapper _mapper;

        public ListService(MainDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IList<DropdownResponse>> GetDropdownAsync(DropdownRequest model)
        {
            switch (model.NombreLista)
            {
                case "REGIONES":
                    //var regiones = await _context.Regiones.ToListAsync();
                    //return _mapper.Map<IList<DropdownResponse>>(regiones);
                case "PROVINCIAS":
                    //var provincias = await _context.Provincias.ToListAsync();
                    //if (model.PadreId != null)
                    //    provincias = provincias.Where(x => x.RegionId == model.PadreId).ToList();
                    //return _mapper.Map<IList<DropdownResponse>>(provincias);
                
                default:
                    return null;
            }
        }
    }
}
