using Llaveremos.SharedLibrary.Responses;
using Microsoft.AspNetCore.Mvc;
using SubjectsApi.Application.DTOs;
using SubjectsApi.Application.Interfaces;

namespace SubjectsApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController(ISubject subjectService) : ControllerBase
    {
        [HttpGet("{id:int}")]
        public async Task<ActionResult<SubjectDTO>> GetSubject(int id)
        {
            if (id <= 0)
                return BadRequest("ID inválido");

            var subject = await subjectService.FindByIdAsync(id);
            return subject is not null ? Ok(subject) : NotFound("Materia no encontrada");
        }

        [HttpGet("obtenerMaterias")]
        public async Task<ActionResult<IEnumerable<SubjectDTO>>> GetAll()
        {
            var subjects = await subjectService.GetAllAsync();
            return subjects.Any() ? Ok(subjects) : NotFound("No se encontraron materias");
        }

        [HttpPost("crearMateria")]
        public async Task<ActionResult<Response>> Create([FromBody] SubjectDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await subjectService.CreateAsync(dto);
            return result.Flag ? Ok(result) : BadRequest(result);
        }

        [HttpPut("editarMateria")]
        public async Task<ActionResult<Response>> Update([FromBody] SubjectDTO dto)
        {
            if (dto.Id <= 0)
                return BadRequest("ID inválido");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await subjectService.UpdateAsync(dto);
            return result.Flag ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("eliminarMateria")]
        public async Task<ActionResult<Response>> Delete([FromBody] SubjectDTO dto)
        {
            if (dto.Id <= 0)
                return BadRequest("ID inválido");

            var result = await subjectService.DeleteAsync(dto);
            return result.Flag ? Ok(result) : BadRequest(result);
        }

        [HttpGet("materiasPorGrado/{grado:int}")]
        public async Task<ActionResult<IEnumerable<SubjectDTO>>> GetSubjectsByGrade(int grado)
        {
            if (grado < 1 || grado > 3)
                return BadRequest("El grado debe ser 1, 2 o 3");

            var materias = await subjectService.GetManyByAsync(s => s.Grado == grado);

            return materias.Any() ? Ok(materias) : NotFound($"No se encontraron materias para el grado {grado}");
        }

        [HttpGet("obtenerPorCodigo/{codigo}")]
        public async Task<ActionResult<SubjectDTO>> GetByCode(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                return BadRequest("Código inválido");

            var subject = await subjectService.GetByCode(codigo);
            return subject is not null ? Ok(subject) : NotFound("Materia no encontrada con el código proporcionado");
        }

        [HttpGet("talleresPorGrado/{grado}")]
        public async Task<ActionResult<IEnumerable<SubjectDTO>>> GetWorkshopsByGrade(int grado)
        {
            if (grado < 1 || grado > 3)
                return BadRequest("El grado debe ser 1, 2 o 3");
            var workshops = await subjectService.GetWorkshopsByGradeAsync(grado);
            return workshops.Any() ? Ok(workshops) : NotFound($"No se encontraron talleres para el grado {grado}");

        }
    }
}
