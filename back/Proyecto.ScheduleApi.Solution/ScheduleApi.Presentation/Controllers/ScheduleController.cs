using Microsoft.AspNetCore.Mvc;
using ScheduleApi.Application.DTOs;
using ScheduleApi.Application.Interfaces;
using Llaveremos.SharedLibrary.Responses;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ScheduleApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController(ISchedule scheduleService) : ControllerBase
    {
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] ScheduleDTO dto)
        {
            Response response = await scheduleService.Create(dto);
            if (!response.Flag) return BadRequest(response);
            return Ok(response);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] ScheduleDTO dto)
        {
            Response response = await scheduleService.Update(dto);
            if (!response.Flag) return BadRequest(response);
            return Ok(response);
        }

        [HttpGet("getByUser/{userId}")]
        public async Task<IActionResult> GetByUser(int userId)
        {
            Expression<Func<ScheduleDTO, bool>> predicate = s => s.IdUsuario == userId;
            var results = await scheduleService.GetBy(predicate);
            return Ok(results);
        }
    }
}
