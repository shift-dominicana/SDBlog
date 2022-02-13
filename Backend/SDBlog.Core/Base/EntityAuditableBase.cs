using System;

namespace SDBlog.Core.Base
{
    public class EntityAuditableBase : IEntityBase
    {
        public DateTimeOffset FechaRegistro { get; set; }
        public DateTimeOffset? FechaModificacion { get; set; }
        public int CreadoPor { get; set; }
        public int ModificadoPor { get; set; }
        public string Estatus { get; set; }
        public int Id { get; set; }
        public bool Borrado { get; set; }
    }
}
