using ClassroomApi.Application.DTOs;
using Llaveremos.SharedLibrary.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ClassroomApi.Application.Interfaces
{
    public interface IClassroom<T> where T : class
    {
        Task<Response> RegisterAttendance(AsistenciaDTO asistenciaDTO);
        Task<Response> RegisterGrade(CalificacionDTO calificacionDTO);
        Task<Response> GenerateReport(ReporteDTO reporteDTO);
        Task<Response> RegisterSanction(SancionDTO sancionDTO);
        Task<IEnumerable<ReporteDTO>> GetReportsByStudent(int studentId);
        Task<T> GetByAsync(Expression<Func<T, bool>> predicate);
    }
}
