using AuthenticationApi.Domain.Entities;
using Llaveremos.SharedLibrary.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationApi.Application.Interfaces
{
    public interface ISolicitudContrasena
    {
        Task<Response> CrearSolicitud(string userId);
        Task<Response> ProcesarSolicitud(string userId);
        Task<Response> EliminarSolicitud(string userId);
        Task<IEnumerable<SolicitudContrasena>> GetBy(Expression<Func<SolicitudContrasena, bool>> predicate);
    }
}
