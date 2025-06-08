using AuthenticationApi.Application.DTOs;
using AuthenticationApi.Application.Interfaces;
using AuthenticationApi.Application.Services;
using Llaveremos.SharedLibrary.Responses;
using Llaveremos.SharedLibrary.Logs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AuthenticationApi.Application.Mappers;
using AuthenticationApi.Domain.Entities;
using AuthenticationApi.Infrastructure.Data;

namespace AuthenticationApi.Infrastructure.Repositories
{
    public class SolicitudRepository(AuthenticationDbContext context, INotificationPublisher notificationPublisher) : ISolicitudRepository
    {

        // Método para crear la solicitud y publicar la notificación
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

                // Guardar la solicitud en la base de datos
                await context.SolicitudesAltas.AddAsync(solicitud);
                await context.SaveChangesAsync();

                // Crear la notificación para publicar en RabbitMQ
                string subject = "📥 Solicitud de registro recibida";
                string body = $@"
                    <p>Hola <strong>{dto.NombrePadre}</strong>,</p>
                    <p>Tu solicitud para registrar al alumno <strong>{dto.NombreAlumno}</strong> (CURP: {dto.CurpAlumno}) en <strong>{dto.Grado}° grado</strong> ha sido recibida exitosamente.</p>
                    <p>Nos pondremos en contacto contigo en breve.</p>
                    <br/>
                    <p>Atentamente,<br/>Sistema Escolar</p>";

                // Crear el objeto de notificación
                var notification = new EmailNotificationDTO(dto.CorreoPadre, subject, body);

                try
                {
                    // Publicamos la notificación a RabbitMQ
                    notificationPublisher.PublishEmailNotification(notification);
                }
                catch (Exception ex)
                {
                    LogException.LogExceptions(ex);
                    return new Response(false, "La solicitud fue procesada, pero hubo un error al enviar la notificación.");
                }

                return new Response(true, "Solicitud enviada correctamente y notificación publicada.");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error al guardar la solicitud");
            }
        }

        // Método para marcar la solicitud como procesada
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

        // Método para editar una solicitud
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

        // Método para obtener solicitudes por predicado
        public async Task<IEnumerable<SolicitudAlta>> GetBy(Expression<Func<SolicitudAlta, bool>> predicate)
        {
            try
            {
                var solicitudes = await context.SolicitudesAltas.Where(predicate).ToListAsync()!;
                return solicitudes is not null ? solicitudes : null!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw;
            }
        }

        // Método para obtener todas las solicitudes
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
