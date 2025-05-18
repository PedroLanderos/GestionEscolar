using ClassroomApi.Application.DTOs;
using ClassroomApi.Domain.Entities;
using Llaveremos.SharedLibrary.Responses;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ClassroomApi.Application.Interfaces
{
    public interface IAsistencia
    {
        Task<Response> CrearAsistencia(AsistenciaDTO dto);
        Task<Response> ActualizarAsistencia(AsistenciaDTO dto);
        Task<Response> EliminarAsistencia(int id);
        Task<AsistenciaDTO> ObtenerAsistenciaPorId(int id);

        Task<IEnumerable<AsistenciaDTO>> ObtenerAsistencias();
        Task<IEnumerable<Asistencia>> GetBy(Expression<Func<Asistencia, bool>> predicate);
    }
}
