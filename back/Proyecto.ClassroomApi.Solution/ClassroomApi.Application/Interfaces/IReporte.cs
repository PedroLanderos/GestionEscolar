using ClassroomApi.Application.DTOs;
using ClassroomApi.Domain.Entities;
using Llaveremos.SharedLibrary.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ClassroomApi.Application.Interfaces
{
    public interface IReporte
    {
        Task<Response> CrearReporte(ReporteDTO dto);
        Task<Response> ActualizarReporte(ReporteDTO dto);
        Task<Response> EliminarReporte(int id);
        Task<ReporteDTO> ObtenerReportePorId(int id);

        Task<IEnumerable<ReporteDTO>> ObtenerReportes();
        Task<IEnumerable<Reporte>> GetBy(Expression<Func<Reporte, bool>> predicate);
        Task<ReporteDTO?> ObtenerReporteInasistenciaPorAlumnoFechaGrupoAsync(string idAlumno, DateTime fecha, string grupo);
    }
}
