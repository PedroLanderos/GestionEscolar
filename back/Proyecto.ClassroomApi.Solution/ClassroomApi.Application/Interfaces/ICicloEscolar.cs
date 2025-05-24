using ClassroomApi.Application.DTOs;
using ClassroomApi.Domain.Entities;
using Llaveremos.SharedLibrary.Responses;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ClassroomApi.Application.Interfaces
{
    public interface ICicloEscolar
    {
        Task<Response> AgregarCicloEscolar(CicloEscolarDTO dto);
        Task<Response> ActualizarCicloEscolar(CicloEscolarDTO dto);
        Task<Response> EliminarCicloEscolar(CicloEscolarDTO dto);
        Task<IEnumerable<CicloEscolarDTO>> ObtenerCiclosEscolares();
        Task<CicloEscolarDTO> ObtenerCicloEscolarPorId(string id);
        Task<IEnumerable<CicloEscolar>> GetManyBy(Expression<Func<CicloEscolar, bool>> predicate);
        Task<CicloEscolar> GetBy(Expression<Func<CicloEscolar, bool>> predicate);
    }
}
