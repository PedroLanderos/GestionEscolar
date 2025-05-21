using ClassroomApi.Application.DTOs;
using ClassroomApi.Domain.Entities;
using Llaveremos.SharedLibrary.Interface;
using Llaveremos.SharedLibrary.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ClassroomApi.Application.Interfaces
{
    public interface ICalificacion 
    {
        Task<Response> AgregarCalificacion(CalificacionDTO dto);
        Task<Response> ActualizarCalificacion(CalificacionDTO dto);
        Task<Response> EliminarCalificacion(CalificacionDTO dto);
        Task<IEnumerable<CalificacionDTO>> ObtenerCalificaciones();
        Task<CalificacionDTO> ObtenerCalificacionPorId(int id);
        Task<IEnumerable<Calificacion>> GetManyBy(Expression<Func<Calificacion, bool>> predicate);
    }
}
