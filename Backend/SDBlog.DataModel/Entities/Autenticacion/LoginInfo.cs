using System;
using System.Collections.Generic;

namespace SDBlog.DataModel.Entities.Autenticacion
{
    public class LoginInfo
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string TokenAcceso { get; set; }
        public DateTime ValidoHasta { get; set; }

        public Usuario UsuarioJson { get; set; }
        public Persona PersonaJson { get; set; }

        public IList<Permiso> PermisosJson { get; set; }
    }
}
