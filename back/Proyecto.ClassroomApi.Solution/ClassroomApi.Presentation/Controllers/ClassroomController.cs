using ClassroomApi.Application.DTOs;
using ClassroomApi.Application.Interfaces;
using ClassroomApi.Application.Services;
using ClassroomApi.Domain.Entities;
using Llaveremos.SharedLibrary.Responses;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ClassroomApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassroomController : ControllerBase
    {
        private readonly IClassroom<Asistencia> _asistenciaRepository;
        private readonly IClassroom<Calificacion> _calificacionRepository;
        private readonly IClassroom<Sancion> _sancionRepository;
        private readonly IClassroom<Reporte> _reporteRepository;

        // Inyección de dependencias de los repositorios
        public ClassroomController(
            IClassroom<Asistencia> asistenciaRepository,
            IClassroom<Calificacion> calificacionRepository,
            IClassroom<Sancion> sancionRepository,
            IClassroom<Reporte> reporteRepository)
        {
            _asistenciaRepository = asistenciaRepository;
            _calificacionRepository = calificacionRepository;
            _sancionRepository = sancionRepository;
            _reporteRepository = reporteRepository;
        }
        [HttpPost("register-attendance")]
        public async Task<IActionResult> RegisterAttendance([FromBody] AsistenciaDTO asistenciaDTO)
        {
            if (asistenciaDTO == null)
            {
                return BadRequest("Datos de asistencia inválidos.");
            }

            var response = await _asistenciaRepository.RegisterAttendance(asistenciaDTO);
            if (response.Flag)
            {
                return Ok(response.Message);
            }

            return BadRequest(response.Message);
        }

        [HttpPost("register-grade")]
        public async Task<IActionResult> RegisterGrade([FromBody] CalificacionDTO calificacionDTO)
        {
            if (calificacionDTO == null)
            {
                return BadRequest("Datos de calificación inválidos.");
            }

            var response = await _calificacionRepository.RegisterGrade(calificacionDTO);
            if (response.Flag)
            {
                return Ok(response.Message);
            }

            return BadRequest(response.Message);
        }

        [HttpPost("generate-report")]
        public async Task<IActionResult> GenerateReport([FromBody] ReporteDTO reporteDTO)
        {
            if (reporteDTO == null)
            {
                return BadRequest("Datos de reporte inválidos.");
            }

            var response = await _reporteRepository.GenerateReport(reporteDTO);
            if (response.Flag)
            {
                return Ok(response.Message);
            }

            return BadRequest(response.Message);
        }

        [HttpPost("register-sanction")]
        public async Task<IActionResult> RegisterSanction([FromBody] SancionDTO sancionDTO)
        {
            if (sancionDTO == null)
            {
                return BadRequest("Datos de sanción inválidos.");
            }

            var response = await _sancionRepository.RegisterSanction(sancionDTO);
            if (response.Flag)
            {
                return Ok(response.Message);
            }

            return BadRequest(response.Message);
        }

        [HttpGet("get-reports/{studentId}")]
        public async Task<IActionResult> GetReportsByStudent(int studentId)
        {
            var reports = await _reporteRepository.GetReportsByStudent(studentId);
            if (reports == null || !reports.Any())
            {
                return NotFound("No se encontraron reportes para este estudiante.");
            }

            return Ok(reports);
        }
    }
}
