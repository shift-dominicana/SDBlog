using SDBlog.BusinessLayer.Dtos.Response;
using SDBlog.DataModel.Autenticacion;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace SDBlog.BusinessLayer.Interfaces.Autenticacion
{
    public interface IAuthService
    {
        Task<LoginInfo> GuardarLoginInfo(LoginResponse loginInfo);
        object GenerateJwtToken(LoginInfo model);
        bool ValidateToken(string key, string issuer, string token);
        Task<string> RemoverLoginInfo(int usuarioId);
        int GetAuthUserId(HttpRequest Request);
        Task<LoginInfo> GetAuthUserAsync(int usuarioId);
        Task<PermisoDto> GetPermissionsAsync(int usuarioId, int moduloId);
    }
}
