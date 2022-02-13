using SDBlog.BusinessLayer.Dtos.Request;
using SDBlog.BusinessLayer.Dtos.Response;
using SDBlog.Services.Interfaces.SSO;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SDBlog.Services.Implementations.SSO
{
    public class Auth : IAuth
    {
        private readonly IConfiguration _config;
        public Auth(IConfiguration configuration)
        {
            _config = configuration;
        }

        public async Task<LoginResponse> Login(LoginRequest userLogin)
        {

            var client = new HttpClient();
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri($"{_config["SsoSettings:Url"]}v1/auth/login");
            request.Method = HttpMethod.Post;
            string body = JsonSerializer.Serialize(new
            {
                SecretoAplicacion = _config["SsoSettings:ClientSecret"],
                NombreUsuario = userLogin.NombreUsuario,
                Clave = userLogin.Clave
            });

            var content = new StringContent(body, Encoding.UTF8, "application/json");
            request.Content = content;

            request.Headers.Add("Accept", "application/json");

            var response = await client.SendAsync(request);
            var result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<LoginResponse>(result);

        }

        public async Task<string> Logout(string token)
        {

            var client = new HttpClient();
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri($"{_config["SsoSettings:Url"]}v1/auth/logout");
            request.Method = HttpMethod.Post;

            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Authorization", $"Bearer {token}");

            var response = await client.SendAsync(request);
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }

        public async Task<LoginResponse> RefreshToken(string token)
        {

            var client = new HttpClient();
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri($"{_config["SsoSettings:Url"]}v1/auth/refresh");
            request.Method = HttpMethod.Put;

            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Authorization", $"Bearer {token}");
            request.Headers.Add("App-Secret", _config["SsoSettings:ClientSecret"]);

            var response = await client.SendAsync(request);
            var result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<LoginResponse>(result);
        }
    }
}
