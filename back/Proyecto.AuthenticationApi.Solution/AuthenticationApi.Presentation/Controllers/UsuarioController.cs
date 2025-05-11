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
        [HttpGet("obtenerUsuarios")]
        public async Task<ActionResult<IEnumerable<ObtenerUsuarioDTO>>> ObtenerUsuarios()
        {
            try
            {
                var usuarios = await userService.GetAllUsers();
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(500, "Error al obtener los usuarios desde el controlador");
            }
        }
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

        [HttpGet("obtenerAlumnos")]
        public async Task<ActionResult<ObtenerUsuarioDTO>> ObtenerAlumnos()
        {
            try
            {
                var alumnos = await userService.GetAllUsers();
                var filtrados = alumnos.Where(u => u.Rol == "Alumno").ToList();
                return Ok(filtrados);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(500, "Error al obtener los alumnos");
            }
        }

        [HttpGet("obtenerTutores")]
        public async Task<ActionResult<ObtenerUsuarioDTO>> ObtenerTutores()
        {
            try
            {
                var tutores = await userService.GetAllUsers();
                var filtrados = tutores.Where(u => u.Rol == "Padre").ToList();
                return Ok(filtrados);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(500, "Error al obtener los tutores");
            }
        }

        [HttpGet("obtenerAdministradores")]
        public async Task<ActionResult<ObtenerUsuarioDTO>> ObtenerAdministradores()
        {
            try
            {
                var admins = await userService.GetAllUsers();
                var filtrados = admins.Where(u => u.Rol == "Administrador").ToList();
                return Ok(filtrados);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(500, "Error al obtener los administradores");
            }
        }

        [HttpPut("editarUsuario")]
        public async Task<ActionResult<Response>> EditarUsuario([FromBody] EditarUsuarioDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await userService.EditarUsuario(dto);
                return result.Flag ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(500, new Response(false, "Error al editar el usuario desde el controlador"));
            }
        }
    }
}
