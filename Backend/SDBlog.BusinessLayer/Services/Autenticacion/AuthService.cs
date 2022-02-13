using AutoMapper;
using SDBlog.BusinessLayer.Dtos.Response;
using SDBlog.BusinessLayer.Interfaces.Autenticacion;
using SDBlog.DataModel.Autenticacion;
using SDBlog.DataModel.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SDBlog.BusinessLayer.Services.Autenticacion
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly MainDbContext _context;
        private readonly IMapper _mapper;
        public AuthService(MainDbContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<LoginInfo> GuardarLoginInfo(LoginResponse loginInfo)
        {
            var oldLogins = await _context.LoginInfos.Where(x => x.UsuarioId == loginInfo.data.userInfo.usuario.id).ToListAsync();
            _context.LoginInfos.RemoveRange(oldLogins);
            await _context.SaveChangesAsync();

            LoginInfo model = new()
            {
                UsuarioId = loginInfo.data.userInfo.usuario.id,
                TokenAcceso = loginInfo.data.accessToken.token,
                ValidoHasta = DateTime.Parse(loginInfo.data.accessToken.expireAt),
                UsuarioJson = _mapper.Map<Usuario>(loginInfo.data.userInfo.usuario),
                PersonaJson = _mapper.Map<Persona>(loginInfo.data.userInfo.persona),
                PermisosJson = _mapper.Map<IList<Permiso>>(loginInfo.data.userInfo.cliente.permisos)
            };

            await _context.LoginInfos.AddAsync(model);
            await _context.SaveChangesAsync();

            return model;
        }

        public async Task<string> RemoverLoginInfo(int usuarioId)
        {
            var model = _context.LoginInfos.FirstOrDefault(x => x.UsuarioId == usuarioId);

            if (model == null)
                return string.Empty;

            string token = model.TokenAcceso;
            _context.LoginInfos.Remove(model);
            await _context.SaveChangesAsync();
            return token;
        }

        public object GenerateJwtToken(LoginInfo model)
        {
            var expires = model.ValidoHasta;
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, $"{model.UsuarioId}"),
                new Claim(JwtRegisteredClaimNames.GivenName, model.UsuarioJson.NombreUsuario),
                new Claim(JwtRegisteredClaimNames.UniqueName, model.UsuarioJson.NombreUsuario),
                new Claim(JwtRegisteredClaimNames.NameId,  $"{model.UsuarioId}"),
                new Claim("new",""),
                new Claim(JwtRegisteredClaimNames.Sub, ""),
                new Claim(JwtRegisteredClaimNames.Jti, model.TokenAcceso)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            //expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["Jwt:ExpiryMinutes"])).ToUniversalTime();

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool ValidateToken(string key, string issuer, string token)
        {
            var mySecret = Encoding.UTF8.GetBytes(key);
            var mySecurityKey = new SymmetricSecurityKey(mySecret);
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token,
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidIssuer = issuer,
                    ValidAudience = issuer,
                    IssuerSigningKey = mySecurityKey,
                }, out SecurityToken validatedToken);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public int GetAuthUserId(HttpRequest Request)
        {
            var jwt = Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwt);
            token.Payload.TryGetValue("nameid", out var id);
            return int.Parse(id.ToString());
        }

        public async Task<LoginInfo> GetAuthUserAsync(int usuarioId)
        {
            var usuario = await _context.LoginInfos.FirstOrDefaultAsync(x => x.UsuarioId == usuarioId);
            return usuario;
        }

        public async Task<PermisoDto> GetPermissionsAsync(int usuarioId, int moduloId)
        {
            var usuario = await _context.LoginInfos.FirstOrDefaultAsync(x => x.UsuarioId == usuarioId);
            return _mapper.Map<PermisoDto>(usuario.PermisosJson.FirstOrDefault(x => x.ModuloId == moduloId));
        }
    }
}
