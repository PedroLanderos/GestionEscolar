using Llaveremos.SharedLibrary.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SubjectsApi.Application.DTOs;
using SubjectsApi.Application.Interfaces;

namespace SubjectsApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubjectDTO>>> GetAll()
        {
            var subjects = await subjectService.GetAllAsync();
            return subjects.Any() ? Ok(subjects) : NotFound("No se encontraron materias");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Response>> Create([FromBody] SubjectDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await subjectService.CreateAsync(dto);
            return result.Flag ? Ok(result) : BadRequest(result);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Response>> Update([FromBody] SubjectDTO dto)
        {
            if (dto.Id <= 0)
                return BadRequest("ID inválido");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await subjectService.UpdateAsync(dto);
            return result.Flag ? Ok(result) : BadRequest(result);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Response>> Delete([FromBody] SubjectDTO dto)
        {
            if (dto.Id <= 0)
                return BadRequest("ID inválido");

            var result = await subjectService.DeleteAsync(dto);
            return result.Flag ? Ok(result) : BadRequest(result);
        }
    }
}
