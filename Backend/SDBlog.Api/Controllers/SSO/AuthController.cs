using SDBlog.BusinessLayer.Dtos.Request;
using SDBlog.BusinessLayer.Dtos.Response;
using SDBlog.BusinessLayer.Interfaces.Autenticacion;
using SDBlog.DataModel.Autenticacion;
using SDBlog.Services.Interfaces.SSO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SDBlog.Api.Controllers.SSO
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuth _auth;
        private readonly IAuthService _authService;
        public AuthController(IAuth auth, IAuthService authService)
        {
            _auth = auth;
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequest Usuario)
        {
            try
            {
                var result = await _auth.Login(Usuario);
                LoginInfo loginInfo = await _authService.GuardarLoginInfo(result);
                return Ok(new
                {
                    Message = result.message,
                    AccessToken = _authService.GenerateJwtToken(loginInfo),
                    Usuario = result.data.userInfo.usuario,
                    Persona = result.data.userInfo.persona,
                    Menu = result.data.userInfo.cliente.menu
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error inesperado", exception = ex });
            }

        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                int usuarioId = _authService.GetAuthUserId(Request);
                string tokenAcceso = await _authService.RemoverLoginInfo(usuarioId);

                if (string.IsNullOrEmpty(tokenAcceso))
                    return NotFound(new { Message = "El token no es válido" });

                await _auth.Logout(tokenAcceso);
                return Ok(new { status = "Success", Message = "Usuario deslogueado con éxito" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error inesperado", exception = ex });
            }

        }

        [HttpPost("Refresh")]
        public async Task<IActionResult> Refresh()
        {
            try
            {
                int usuarioId = _authService.GetAuthUserId(Request);
                string tokenAcceso = await _authService.RemoverLoginInfo(usuarioId);

                if (string.IsNullOrEmpty(tokenAcceso))
                    return NotFound(new { Message = "El token no es válido" });

                var result = await _auth.RefreshToken(tokenAcceso);
                LoginInfo loginInfo = await _authService.GuardarLoginInfo(result);
                return Ok(new
                {
                    Message = result.message,
                    AccessToken = _authService.GenerateJwtToken(loginInfo),
                    Usuario = result.data.userInfo.usuario,
                    Persona = result.data.userInfo.persona,
                    Menu = result.data.userInfo.cliente.menu
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error inesperado", exception = ex });
            }

        }

        [HttpGet("GetPermissionsByModuleId/{moduloId}")]
        public async Task<IActionResult> GetPermissionsByModuleId(int moduloId)
        {
            try
            {
                if (moduloId <= 0)
                    return BadRequest(new { Message = "El ModuloId es requerido" });

                int usuarioId = _authService.GetAuthUserId(Request);

                if (usuarioId <= 0)
                    return NotFound(new { Message = "El token no es válido" });

                PermisoDto permisos = await _authService.GetPermissionsAsync(usuarioId, moduloId);

                if (permisos is null)
                    return NotFound(new { Message = "No se encontraron permisos asociados a este módulo" });

                return Ok(permisos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error inesperado", exception = ex });
            }

        }

        [HttpGet("IsAuthorized/{moduloId}/{permiso}")]
        public async Task<IActionResult> IsAuthorized(int moduloId, string permiso)
        {
            try
            {
                if (moduloId <= 0)
                    return BadRequest(new { Message = "El ModuloId es requerido" });

                if (permiso is null)
                    return BadRequest(new { Message = "El permiso es requerido" });

                int usuarioId = _authService.GetAuthUserId(Request);

                if (usuarioId <= 0)
                    return NotFound(new { Message = "El token no es válido" });

                PermisoDto permisos = await _authService.GetPermissionsAsync(usuarioId, moduloId);

                if (!permisos.Acciones.Contains(permiso))
                    return Unauthorized(new { Message = "El usuario no se encuentra autorizado para usar esta ruta." });

                return Ok(new { Message = "El usuario se encuentra autorizado para usar esta ruta" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error inesperado", exception = ex });
            }

        }

        /* Metodo de prueba */
        [HttpGet("PruebaUsuario")]
        public async Task<IActionResult> PruebaUsuario()
        {
            try
            {
                int usuarioId = _authService.GetAuthUserId(Request);

                if (usuarioId <= 0)
                    return NotFound(new { Message = "El token no es válido" });

                var usuarioAutenticado = await _authService.GetAuthUserAsync(usuarioId);
                return Ok(usuarioAutenticado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error inesperado", exception = ex });
            }

        }
    }
}
