using Llaveremos.SharedLibrary.Responses;
using Microsoft.AspNetCore.Mvc;
using SubjectsApi.Application.DTOs;
using SubjectsApi.Application.Interfaces;

namespace SubjectsApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectAssignmentController(ISubjectAssignment service) : ControllerBase
    {
        [HttpGet("obtenerAsignaciones")]
        public async Task<ActionResult<IEnumerable<SubjectAssignmentDTO>>> GetAll()
        {
            var asignaciones = await service.GetAsignmnets();
            return asignaciones.Any() ? Ok(asignaciones) : NotFound("No hay asignaciones registradas");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SubjectAssignmentDTO>> GetById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("ID inválido");

            var asignacion = await service.GetById(id);
            return asignacion is not null ? Ok(asignacion) : NotFound("Asignación no encontrada");
        }

        [HttpPost("crearAsignacion")]
        public async Task<ActionResult<Response>> Create([FromBody] SubjectAssignmentDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await service.CreateAsignmnet(dto);
            return result.Flag ? Ok(result) : BadRequest(result);
        }

        [HttpPut("editarAsignacion")]
        public async Task<ActionResult<Response>> Update([FromBody] SubjectAssignmentDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Id))
                return BadRequest("ID inválido");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await service.UpdateAsignmnet(dto);
            return result.Flag ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("eliminarAsignacion/{id}")]
        public async Task<ActionResult<Response>> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("ID inválido");

            var result = await service.DeleteAsignmnet(id);
            return result.Flag ? Ok(result) : BadRequest(result);
        }

        [HttpGet("obtenerAsignacionesPorGrado/{grado}")]
        public async Task<ActionResult<IEnumerable<SubjectAssignmentDTO>>> GetByGrado(int grado)
        {
            if (grado < 1 || grado > 3)
                return BadRequest("El grado debe estar entre 1 y 3");

            var asignaciones = await service.GetAssignmentByGrade(grado);
            return asignaciones.Any() ? Ok(asignaciones) : NotFound($"No se encontraron asignaciones para el grado {grado}");
        }
    }
}
