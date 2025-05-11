using AuthenticationApi.Application.DTOs;
using Llaveremos.SharedLibrary.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationApi.Application.Interfaces
{
    public interface ISolicitudRepository
    {
        Task<Response> CrearSolicitud(SolicitudAltaDTO dto);
    }
}
