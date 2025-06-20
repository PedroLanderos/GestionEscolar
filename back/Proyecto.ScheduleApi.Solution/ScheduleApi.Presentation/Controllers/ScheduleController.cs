﻿using Microsoft.AspNetCore.Mvc;
using ScheduleApi.Application.DTOs;
using ScheduleApi.Application.Interfaces;
using Llaveremos.SharedLibrary.Responses;
using System.Threading.Tasks;
using Llaveremos.SharedLibrary.Logs;
using ScheduleApi.Infrastructure.Repositories;

namespace ScheduleApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly ISchedule scheduleService;

        public ScheduleController(ISchedule scheduleService)
        {
            this.scheduleService = scheduleService;
        }

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

        // Nuevos Métodos de Talleres (Workshops)
        [HttpPost("crearTaller")]
        public async Task<IActionResult> CreateWorkshop([FromBody] SubjectToUserDTO workshop)
        {
            var result = await scheduleService.CreateWorkshopAsync(workshop);
            return result.Flag ? Ok(result) : BadRequest(result);
        }

        [HttpPut("actualizarTaller")]
        public async Task<IActionResult> UpdateWorkshop([FromBody] SubjectToUserDTO workshop)
        {
            var result = await scheduleService.UpdateWorkshopAsync(workshop);
            return result.Flag ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("eliminarTaller/{id}")]
        public async Task<IActionResult> DeleteWorkshop(int id)
        {
            var result = await scheduleService.DeleteWorkshopAsync(id);
            return result.Flag ? Ok(result) : BadRequest(result);
        }

        [HttpGet("obtenerTalleres")]
        public async Task<IActionResult> GetWorkshops()
        {
            var result = await scheduleService.GetWorkshopsAsync();
            return Ok(result);
        }

        [HttpGet("obtenerTallerPorId/{id}")]
        public async Task<IActionResult> GetWorkshopById(int id)
        {
            var result = await scheduleService.GetWorkshopByIdAsync(id);
            return result is not null ? Ok(result) : NotFound("Taller no encontrado");
        }

        [HttpGet("obtenerTalleresPorUsuario/{userId}")]
        public async Task<IActionResult> GetWorkshopsByUserId(string userId)
        {
            var result = await scheduleService.GetWorkShopBy(u => u.UserId == userId);
            if (result != null && result.Any())
            {
                return Ok(result);
            }
            return NotFound("No se encontraron talleres asignados para este usuario.");
        }

        [HttpPost("asignarTallerEspaciosLibres/{userId}/{courseId}")]
        public async Task<IActionResult> AssignWorkshopToFreeSpaces(string userId, string courseId)
        {
            var result = await scheduleService.AsignarTallerEnEspaciosLibresAsync(userId, courseId);
            return result.Flag ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("eliminarTalleresDeAlumno/{userId}")]
        public async Task<IActionResult> DeleteStudentWorkshops(string userId)
        {
            var result = await scheduleService.DeleteWorkshopsByStudentAsync(userId);
            return result.Flag ? Ok(result) : BadRequest(result);
        }

        [HttpGet("alumnosPorTaller/{materiaProfesor}")]
        public async Task<IActionResult> GetAlumnosPorTaller(string materiaProfesor)
        {
            try
            {
                var alumnos = await scheduleService.GetStudentIdsByWorkshopAsync(materiaProfesor);
                return Ok(alumnos);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return StatusCode(500, "Error al obtener alumnos del taller.");
            }
        }
    }
}
