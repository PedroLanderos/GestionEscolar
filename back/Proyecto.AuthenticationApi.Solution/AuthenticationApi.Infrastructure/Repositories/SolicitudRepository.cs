using AuthenticationApi.Application.DTOs;
using AuthenticationApi.Application.Interfaces;
using AuthenticationApi.Domain.Entities;
using AuthenticationApi.Infrastructure.Data;
using Llaveremos.SharedLibrary.Responses;
using Llaveremos.SharedLibrary.Logs; // <-- Asegúrate de tener este using

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
    }
}
