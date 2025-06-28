using AuthenticationApi.Application.DTOs;
using AuthenticationApi.Application.Interfaces;
using AuthenticationApi.Domain.Entities;
using Llaveremos.SharedLibrary.Logs;
using Llaveremos.SharedLibrary.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolicitudContrasenaController : ControllerBase
    {
        private readonly ISolicitudContrasena _solicitudContrasenaRepository;

        public SolicitudContrasenaController(ISolicitudContrasena s)
        {
            _solicitudContrasenaRepository = s;
        }

        [HttpPost("CrearSolicitud/{userId}")]
        public async Task<ActionResult<Response>> EnviarSolicitud(string userId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _solicitudContrasenaRepository.CrearSolicitud(userId);
            return Ok(response);
        }

        [HttpPost("ProcesarSolicitud/{userId}")]
        public async Task<ActionResult<Response>> ProcesarSolicitud(string userId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _solicitudContrasenaRepository.ProcesarSolicitud(userId);
            return Ok(response);
        }
        [HttpGet("MostrarSolicitudesSinProcesar")]
        public async Task<ActionResult<IEnumerable<SolicitudContrasena>>> ObtenerSolicitudesSinProcesar()
        {
            try
            {
                var solicitudes = await _solicitudContrasenaRepository.GetBy(x => x.Procesada == false);
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


    }
}
