using System.Collections.Generic;

namespace SDBlog.BusinessLayer.Dtos.Response
{
    public class LoginResponse
    {
        public string status { get; set; }
        public string message { get; set; }
        public Data data { get; set; }
    }

    public class Data
    {
        public AccessToken accessToken { get; set; }
        public UserInfo userInfo { get; set; }
    }

    public class UserInfo
    {
        public UsuarioDto usuario { get; set; }
        public PersonaDto persona { get; set; }
        public Cliente cliente { get; set; }
    }

    public class Cliente
    {
        public List<Modulo> menu { get; set; }
        public List<PermisoDto> permisos { get; set; }
    }

    public class AccessToken
    {
        public string type { get; set; }
        public string token { get; set; }
        public string expireAt { get; set; }
    }

    public class PersonaDto
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
    }

    public class UsuarioDto
    {
        public int id { get; set; }
        public string NombreUsuario { get; set; }
    }

    public class PermisoDto
    {
        public int ModuloId { get; set; }
        public string[] Acciones { get; set; }
    }

    public class Modulo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public string Class { get; set; }
        public string Path { get; set; }
        public string IsEternalLink { get; set; }
        public List<Modulo> submenu { get; set; }
    }
}
