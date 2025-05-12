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
    }
}
