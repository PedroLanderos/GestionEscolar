using AuthenticationApi.Application.DTOs;
using AuthenticationApi.Application.Interfaces;
using Llaveremos.SharedLibrary.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuthenticationApi.Domain.Entities;
using AuthenticationApi.Infrastructure.Repositories;
using Llaveremos.SharedLibrary.Logs;

namespace AuthenticationApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class SolicitudController : ControllerBase
    {
        private readonly ISolicitudRepository _repo;

        public SolicitudController(ISolicitudRepository repo)
        {
            _repo = repo;
        }

        [HttpPost("enviar")]
        public async Task<ActionResult<Response>> EnviarSolicitud([FromBody] SolicitudAltaDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _repo.CrearSolicitud(dto);
            return Ok(response);
        }

        [HttpGet("obtenerSolicitudes")]
        public async Task<ActionResult<IEnumerable<SolicitudAltaDTO>>> ObtenerSolicitudes()
        {
            try
            {
                var solicitudes = await _repo.ObtenerSolicitudes();

                if (!solicitudes.Any() || solicitudes is null)
                    return NotFound("No hay solicitudes registradas.");

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
                var solicitudes = await _repo.GetBy(x => x.Procesado == false);
                if (!solicitudes.Any() || solicitudes is null)
                    return NotFound("No hay solicitudes registradas.");
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

                var result = await _repo.EditarSolicitud(dto);
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
