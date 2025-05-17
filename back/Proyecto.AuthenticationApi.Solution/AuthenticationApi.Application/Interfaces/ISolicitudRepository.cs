using AuthenticationApi.Application.DTOs;
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
    public interface ISolicitudRepository
    {
        Task<Response> CrearSolicitud(SolicitudAltaDTO dto);
        Task<IEnumerable<SolicitudAltaDTO>> ObtenerSolicitudes();
        Task<IEnumerable<SolicitudAlta>> GetBy(Expression<Func<SolicitudAlta, bool>> predicate);
        Task<Response> EditarSolicitud(EditarSolicitudDTO dto);
        Task<Response> MarcarSolicitudComoProcesada(string curpAlumno);

    }
}
