using AuthenticationApi.Application.DTOs;
using AuthenticationApi.Application.Interfaces;
using Llaveremos.SharedLibrary.Logs;
using Llaveremos.SharedLibrary.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class UsuarioController(IUser userService) : ControllerBase
    {
        [HttpPost("registrarAlumno")]
        public async Task<ActionResult<Response>> RegistrarAlumno([FromBody] UsuarioDTO usuarioDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await userService.RegistrarAlumno(usuarioDTO);
                return result.Flag ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error al registrar un alumno desde el endpoint");
            }
        }

        [HttpPost("registrarAdministrador")]
        public async Task<ActionResult<Response>> RegistrarAdministrador([FromBody] UsuarioDTO usuarioDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await userService.RegistrarAdministrador(usuarioDTO);
                return result.Flag ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error en el controlador al registrar un administrador");
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<Response>> Login([FromBody] IniciarSesionDTO loginDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await userService.Login(loginDTO);
                return result.Flag ? Ok(result) : Unauthorized(result);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(500, new Response(false, "Error al iniciar sesión desde el controlador"));
            }
        }
    }
}
