using AuthenticationApi.Application.DTOs;
using AuthenticationApi.Application.Interfaces;
using AuthenticationApi.Application.Services;
using Llaveremos.SharedLibrary.Responses;
using Llaveremos.SharedLibrary.Logs; 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuthenticationApi.Domain.Entities;
using AuthenticationApi.Infrastructure.Repositories;

namespace AuthenticationApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class SolicitudController(ISolicitudRepository repo, IEmail emailService) : ControllerBase
    {
        [HttpPost("enviar")]
        public async Task<ActionResult<Response>> EnviarSolicitud([FromBody] SolicitudAltaDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await repo.CrearSolicitud(dto);

            string subject = "📥 Solicitud de registro recibida";
            string body = $@"
                <p>Hola <strong>{dto.NombrePadre}</strong>,</p>
                <p>Tu solicitud para registrar al alumno <strong>{dto.NombreAlumno}</strong> (CURP: {dto.CurpAlumno}) en <strong>{dto.Grado}° grado</strong> ha sido recibida exitosamente.</p>
                <p>Nos pondremos en contacto contigo en breve.</p>
                <br/>
                <p>Atentamente,<br/>Sistema Escolar</p>";

            try
            {
                await emailService.EnviarCorreoAsync(dto.CorreoPadre, subject, body);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex); // Aquí se guarda el error
                return Ok(new Response(true, "Solicitud enviada, pero el correo no pudo enviarse."));
            }

            return Ok(response);
        }

        [HttpGet("obtenerSolicitudes")]
        public async Task<ActionResult<IEnumerable<SolicitudAltaDTO>>> ObtenerSolicitudes()
        {
            try
            {
                var solicitudes = await repo.ObtenerSolicitudes();

                if (!solicitudes.Any() || solicitudes is null) return NotFound("No hay solicitudes registradas.");

                return Ok(solicitudes);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(500, $"Error desde el endpoint para obtener las solicitudes: {ex.Message}");
            }
        }

        [HttpGet("obtenerSolicitudesSinProcesar")]
        public async Task<ActionResult<IEnumerable<SolicitudAlta>>> ObtenerSolicitudesSinProcesar()
        {
            try
            {
                var solicitudes = await repo.GetBy(x => x.Procesado == false);
                if (!solicitudes.Any() || solicitudes is null) return NotFound("No hay solicitudes registradas.");
                return Ok(solicitudes);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(500, $"Error desde el endpoint para obtener las solicitudes: {ex.Message}");
            }
        }

        [HttpPut("editarSolicitud")]
        public async Task<ActionResult<Response>> EditarSolicitud([FromBody] EditarSolicitudDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await repo.EditarSolicitud(dto);
                return result.Flag ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(500, "Error al editar la solicitud desde el controlador");
            }
        }
    }
}
