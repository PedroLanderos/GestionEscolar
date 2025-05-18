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
    public interface ISancion
    {
        Task<Response> CrearSancion(SancionDTO dto);
        Task<Response> EliminarSancion(SancionDTO dto);
        Task<Response> ActualizarSancion(SancionDTO dto);
        Task<IEnumerable<SancionDTO>> ObtenerSanciones();
        Task<SancionDTO> ObtenerSancionPorId(int id);
        Task<IEnumerable<Sancion>> GetBy(Expression<Func<Sancion, bool>> predicate);

    }
}
