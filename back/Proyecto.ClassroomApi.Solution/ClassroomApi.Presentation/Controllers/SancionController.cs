using ClassroomApi.Application.DTOs;
using ClassroomApi.Application.Interfaces;
using ClassroomApi.Application.Mapper;
using Llaveremos.SharedLibrary.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ClassroomApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SancionController(ISancion sancionService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CrearSancion([FromBody] SancionDTO dto)
        {
            var result = await sancionService.CrearSancion(dto);
            return StatusCode(result.Flag ? 200 : 400, result);
        }

        [HttpPut]
        public async Task<IActionResult> ActualizarSancion([FromBody] SancionDTO dto)
        {
            var result = await sancionService.ActualizarSancion(dto);
            return StatusCode(result.Flag ? 200 : 400, result);
        }

        [HttpDelete]
        public async Task<IActionResult> EliminarSancion([FromBody] SancionDTO dto)
        {
            var result = await sancionService.EliminarSancion(dto);
            return StatusCode(result.Flag ? 200 : 400, result);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerSanciones()
        {
            try
            {
                var result = await sancionService.ObtenerSanciones();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener sanciones: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerSancionPorId(int id)
        {
            try
            {
                var result = await sancionService.ObtenerSancionPorId(id);
                if (result == null)
                    return NotFound("Sanción no encontrada");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener la sanción: {ex.Message}");
            }
        }

        [HttpGet("tipo/{tipoSancion}")]
        public async Task<IActionResult> ObtenerPorTipoSancion(string tipoSancion)
        {
            try
            {
                var result = await sancionService.GetBy(s => s.TipoSancion!.ToLower() == tipoSancion.ToLower());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener sanciones por tipo: {ex.Message}");
            }
        }

        [HttpGet("alumno/{idAlumno}")]
        public async Task<IActionResult> ObtenerPorAlumno(int idAlumno)
        {
            try
            {
                var sanciones = await sancionService.GetBy(s => s.IdAlumno == idAlumno);
                var dtoList = sanciones.Select(SancionMapper.FromEntity).ToList();
                return Ok(dtoList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener sanciones del alumno: {ex.Message}");
            }
        }

        [HttpGet("profesor/{idProfesor}")]
        public async Task<IActionResult> ObtenerPorProfesor(int idProfesor)
        {
            try
            {
                var sanciones = await sancionService.GetBy(s => s.IdProfesor == idProfesor);
                var dtoList = sanciones.Select(SancionMapper.FromEntity).ToList();
                return Ok(dtoList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener sanciones del profesor: {ex.Message}");
            }
        }

        [HttpGet("fecha")]
        public async Task<IActionResult> ObtenerPorRangoFecha([FromQuery] DateTime fechaInicio, [FromQuery] DateTime fechaFin)
        {
            try
            {
                var sanciones = await sancionService.GetBy(s => s.Fecha >= fechaInicio && s.Fecha <= fechaFin);
                var dtoList = sanciones.Select(SancionMapper.FromEntity).ToList();
                return Ok(dtoList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener sanciones por rango de fechas: {ex.Message}");
            }
        }

    }
}
