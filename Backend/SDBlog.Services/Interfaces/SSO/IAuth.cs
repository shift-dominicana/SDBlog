using SDBlog.BusinessLayer.Dtos.Request;
using SDBlog.BusinessLayer.Dtos.Response;
using System.Threading.Tasks;

namespace SDBlog.Services.Interfaces.SSO
{
    public interface IAuth
    {
        Task<LoginResponse> Login(LoginRequest userLogin);
        Task<string> Logout(string token);
        Task<LoginResponse> RefreshToken(string token);
    }
}
