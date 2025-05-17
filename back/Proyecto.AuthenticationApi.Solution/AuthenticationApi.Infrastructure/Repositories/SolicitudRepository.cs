using AuthenticationApi.Application.DTOs;
using AuthenticationApi.Application.Interfaces;
using AuthenticationApi.Domain.Entities;
using AuthenticationApi.Infrastructure.Data;
using Llaveremos.SharedLibrary.Responses;
using Llaveremos.SharedLibrary.Logs;
using Microsoft.EntityFrameworkCore;
using AuthenticationApi.Application.Mappers;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationApi.Infrastructure.Repositories
{
    public class SolicitudRepository(AuthenticationDbContext context) : ISolicitudRepository
    {
        public async Task<Response> MarcarSolicitudComoProcesada(string curpAlumno)
        {
            try
            {
                var solicitud = await context.SolicitudesAltas
                    .FirstOrDefaultAsync(s => s.CurpAlumno.ToUpper() == curpAlumno.ToUpper());

                if (solicitud is null)
                    return new Response(false, "Solicitud no encontrada");

                solicitud.Procesado = true;
                context.SolicitudesAltas.Update(solicitud);
                await context.SaveChangesAsync();

                return new Response(true, "Solicitud marcada como procesada");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error al marcar la solicitud como procesada");
            }
        }
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

        public async Task<Response> EditarSolicitud(EditarSolicitudDTO dto)
        {
            try
            {
                var solicitud = await context.SolicitudesAltas.FindAsync(dto.Id);
                if (solicitud is null)
                    return new Response(false, "Solicitud no encontrada");

                
                if (!string.IsNullOrWhiteSpace(dto.NombreAlumno))
                    solicitud.NombreAlumno = dto.NombreAlumno;

                if (!string.IsNullOrWhiteSpace(dto.CurpAlumno))
                    solicitud.CurpAlumno = dto.CurpAlumno.ToUpper();

                if (dto.Grado.HasValue)
                    solicitud.Grado = dto.Grado.Value;

                if (!string.IsNullOrWhiteSpace(dto.NombrePadre))
                    solicitud.NombrePadre = dto.NombrePadre;

                if (!string.IsNullOrWhiteSpace(dto.Telefono))
                    solicitud.Telefono = dto.Telefono;

                if (!string.IsNullOrWhiteSpace(dto.CorreoPadre))
                    solicitud.CorreoPadre = dto.CorreoPadre;

                if (dto.Procesado.HasValue)
                    solicitud.Procesado = dto.Procesado.Value;

                context.SolicitudesAltas.Update(solicitud);
                await context.SaveChangesAsync();

                return new Response(true, "Solicitud editada correctamente");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error al editar la solicitud");
            }
        }



        public async Task<IEnumerable<SolicitudAlta>> GetBy(Expression<Func<SolicitudAlta, bool>> predicate)
        {
            try
            {
                var solicitudes = await context.SolicitudesAltas.Where(predicate).ToListAsync()!;
                return solicitudes is not null ? solicitudes:null!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw;
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
