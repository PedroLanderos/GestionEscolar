using AuthenticationApi.Application.DTOs;
using AuthenticationApi.Application.Interfaces;
using AuthenticationApi.Domain.Entities;
using AuthenticationApi.Infrastructure.Data;
using Llaveremos.SharedLibrary.Responses;
using Llaveremos.SharedLibrary.Logs;
using Microsoft.EntityFrameworkCore;
using AuthenticationApi.Application.Mappers;

namespace AuthenticationApi.Infrastructure.Repositories
{
    public class SolicitudRepository(AuthenticationDbContext context) : ISolicitudRepository
    {
        public async Task<Response> CrearSolicitud(SolicitudAltaDTO dto)
        {
            try
            {
                var solicitud = new SolicitudAlta
                {
                    NombreAlumno = dto.NombreAlumno,
                    CurpAlumno = dto.CurpAlumno.ToUpper(),
                    Grado = dto.Grado,
                    NombrePadre = dto.NombrePadre,
                    Telefono = dto.Telefono,
                    CorreoPadre = dto.CorreoPadre
                };

                await context.SolicitudesAltas.AddAsync(solicitud);
                await context.SaveChangesAsync();

                return new Response(true, "Solicitud enviada correctamente");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error al guardar la solicitud");
            }
        }

        public async Task<IEnumerable<SolicitudAltaDTO>> ObtenerSolicitudes()
        {
            try
            {
                var solicitudes = await context.SolicitudesAltas.ToListAsync();
                if (!solicitudes.Any()) return null!;

                var mapper = SolicitudAltaMapper.ToDTO(solicitudes);

                return mapper;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error al obtener las solicitudes");
            }
        }
    }

    
}
