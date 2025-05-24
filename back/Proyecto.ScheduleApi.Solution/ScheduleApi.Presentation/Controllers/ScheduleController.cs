using Microsoft.AspNetCore.Mvc;
using ScheduleApi.Application.DTOs;
using ScheduleApi.Application.Interfaces;
using Llaveremos.SharedLibrary.Responses;
using System.Threading.Tasks;

namespace ScheduleApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController(ISchedule scheduleService) : ControllerBase
    {
        [HttpPost("crearHorario")]
        public async Task<IActionResult> CreateSchedule([FromBody] ScheduleDTO schedule)
        {
            var result = await scheduleService.CreateSchedule(schedule);
            return result.Flag ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSchedule(int id)
        {
            var result = await scheduleService.GetSchedule(id);
            return result is not null ? Ok(result) : NotFound("Horario no encontrado");
        }

        [HttpGet("obtenerHorarios")]
        public async Task<IActionResult> GetSchedules()
        {
            var result = await scheduleService.GetSchedules();
            return Ok(result);
        }

        [HttpPut("actualizarHorario")]
        public async Task<IActionResult> UpdateSchedule([FromBody] ScheduleDTO schedule)
        {
            var result = await scheduleService.UpdateScheduleAsync(schedule);
            return result.Flag ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSchedule(int id)
        {
            var result = await scheduleService.DeleteScheduleAsync(id);
            return result.Flag ? Ok(result) : BadRequest(result);
        }

        [HttpPost("asginarMateriaHorario")]
        public async Task<IActionResult> AssignSubject([FromBody] SubjectToScheduleDTO dto)
        {
            var result = await scheduleService.AsignSubjectToScheduleAsync(dto);
            return result.Flag ? Ok(result) : BadRequest(result);
        }

        [HttpPut("actualizarAsignacion")]
        public async Task<IActionResult> UpdateAssignment([FromBody] SubjectToScheduleDTO dto)
        {
            var result = await scheduleService.UpdateAsignmentAsync(dto);
            return result.Flag ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("eliminarAsignacion/{id}")]
        public async Task<IActionResult> DeleteAssignment(int id)
        {
            var result = await scheduleService.DeleteAsignSubjectToScheduleAsync(id);
            return result.Flag ? Ok(result) : BadRequest(result);
        }

        [HttpGet("obtenerHorarioCompleto/{scheduleId}")]
        public async Task<IActionResult> GetFullSchedule(int scheduleId)
        {
            var result = await scheduleService.GetFullSchedule(scheduleId);
            return Ok(result);
        }

        [HttpPost("asignarAlumnoHorario")]
        public async Task<IActionResult> AssignStudentToSchedule([FromBody] ScheduleToUserDTO dto)
        {
            var result = await scheduleService.AsignStudentToScheduleAsync(dto);
            return result.Flag ? Ok(result) : BadRequest(result);
        }

        [HttpPut("actualizarAsignacionAlumno")]
        public async Task<IActionResult> UpdateStudentAssignment([FromBody] ScheduleToUserDTO dto)
        {
            var result = await scheduleService.UpdateAsignStudentToScheduleAsync(dto);
            return result.Flag ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("eliminarAsignacionAlumno/{id}")]
        public async Task<IActionResult> DeleteStudentAssignment(int id)
        {
            var result = await scheduleService.DeleteAsignStudentToScheduleAsync(id);
            return result.Flag ? Ok(result) : BadRequest(result);
        }

        [HttpGet("horarioDocente/{idUsuario}")]
        public async Task<IActionResult> GetTeacherSchedule(string idUsuario)
        {
            var result = await scheduleService.GetScheduleForTeacherAsync(idUsuario);
            return Ok(result);
        }
        [HttpGet("horarioAlumno/{idUsuario}")]
        public async Task<IActionResult> GetStudentSchedule(string idUsuario)
        {
            var result = await scheduleService.GetScheduleForStudentAsync(idUsuario);
            return Ok(result);
        }

        [HttpGet("alumnosPorMateriaHorario")]
        public async Task<IActionResult> GetStudentsBySubjectAndSchedule([FromQuery] string materiaProfesor, [FromQuery] string horario)
        {
            if (string.IsNullOrEmpty(materiaProfesor) || string.IsNullOrEmpty(horario))
                return BadRequest("Se requiere materiaProfesor y horario.");

            var studentIds = await scheduleService.GetStudentIdsBySubjectAndScheduleAsync(materiaProfesor, horario);
            return Ok(studentIds);
        }

        [HttpGet("horarioPorUsuario/{idUsuario}")]
        public async Task<IActionResult> GetScheduleByUserId(string idUsuario)
        {
            var schedule = await scheduleService.GetScheduleByUserIdAsync(idUsuario);
            if (schedule == null)
                return NotFound($"No se encontró horario asignado para el usuario {idUsuario}");

            return Ok(schedule);
        }

    }
}
