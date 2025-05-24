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

        [HttpGet("obtenerUsuarioPorId/{id}")]
        public async Task<ActionResult<ObtenerUsuarioDTO>> ObtenerUsuarioPorId(string id)
        {
            try
            {
                var usuario = await userService.ObtenerUsuarioPorId(id);

                if (usuario == null)
                    return NotFound($"No se encontró un usuario con el ID: {id}");

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(500, "Error al obtener el usuario por ID desde el controlador");
            }
        }

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
        [HttpPost("registrarDocente")]
        public async Task<ActionResult<Response>> RegistrarDocente([FromBody] UsuarioDTO usuarioDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await userService.RegistrarProfesor(usuarioDTO);
                return result.Flag ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error en el controlador al registrar un docente");
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

        [HttpGet("obtenerDocentes")]
        public async Task<ActionResult<ObtenerUsuarioDTO>> ObtenerDocentes()
        {
            try
            {
                var admins = await userService.GetAllUsers();
                var filtrados = admins.Where(u => u.Rol == "Docente").ToList();
                return Ok(filtrados);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(500, "Error al obtener los docentes");
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

        [HttpDelete("eliminarUsuario/{id}")]
        public async Task<ActionResult<Response>> EliminarUsuario(string id)
        {
            try
            {
                var result = await userService.EliminarUsuario(id);
                return result.Flag ? Ok(result) : NotFound(result);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(500, new Response(false, "Error al eliminar el usuario desde el controlador"));
            }
        }

        [HttpGet("filtrarPorGrado/{grado}")]
        public async Task<ActionResult<IEnumerable<UsuarioDTO>>> FiltrarPorGrado(int grado)
        {
            try
            {
                var alumnos = await userService.FiltrarPorGrado(grado);

                if (!alumnos.Any())
                    return NotFound($"No se encontraron alumnos para el grado {grado}");

                return Ok(alumnos);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(500, $"Error al filtrar los alumnos por grado {grado}");
            }
        }

        [HttpPost("obtenerUsuariosPorIds")]
        public async Task<ActionResult<IEnumerable<ObtenerUsuarioDTO>>> ObtenerUsuariosPorIds([FromBody] List<string> ids)
        {
            try
            {
                if (ids == null || !ids.Any())
                    return BadRequest("Se debe proporcionar al menos un ID.");

                var usuarios = await userService.GetByAsync(u => ids.Contains(u.Id!));

                var resultado = usuarios.Select(u => new ObtenerUsuarioDTO
                {
                    Id = u.Id!,
                    NombreCompleto = u.NombreCompleto!,
                    Correo = u.Correo!,
                    Curp = u.Curp!,
                    CuentaBloqueada = u.CuentaBloqueada ?? false,
                    DadoDeBaja = u.DadoDeBaja ?? false,
                    UltimaSesion = u.UltimaSesion ?? DateTime.MinValue,
                    Rol = u.Rol!
                });

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(500, "Error al obtener usuarios por IDs desde el controlador.");
            }
        }

        [HttpPost("recuperarContrasena")]
        public async Task<ActionResult<Response>> RecuperarContrasena([FromBody] string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                    return BadRequest("Se requiere un ID válido.");

                var resultado = await userService.RecuperarContrasenaPorId(id);
                return resultado.Flag ? Ok(resultado) : BadRequest(resultado);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(500, new Response(false, "Error al recuperar la contraseña desde el controlador."));
            }
        }

        [HttpGet("obtenerAlumnoPorTutor/{tutorId}")]
        public async Task<ActionResult<ObtenerUsuarioDTO>> ObtenerAlumnoPorTutor(string tutorId)
        {
            try
            {
                var alumno = await userService.ObtenerAlumnoPorTutor(tutorId);

                if (alumno == null)
                    return NotFound($"No se encontró un alumno asociado al tutor con ID: {tutorId}");

                return Ok(alumno);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(500, "Error al obtener el alumno asociado al tutor desde el controlador");
            }
        }
    }
}
