using AuthenticationApi.Application.DTOs;
using AuthenticationApi.Application.Interfaces;
using AuthenticationApi.Application.Services;
using AuthenticationApi.Domain.Entities;
using AuthenticationApi.Infrastructure.Data;
using Llaveremos.SharedLibrary.Logs;
using Llaveremos.SharedLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationApi.Infrastructure.Repositories
{
    public class SolicitudContrasenaRepository(AuthenticationDbContext _context, INotificationPublisher notificationPublisher, IRandomService _random) : ISolicitudContrasena
    {
        public async Task<Response> CrearSolicitud(string userId)
        {
            try
            {
                var solicitud = new SolicitudContrasena
                {
                    UserId = userId,
                    Procesada = false
                };

                var response = await _context.SolicitudesContrasena.AddAsync(solicitud);

                if (response is null) return new Response(false, "Error creando la solicitud de recuperacion de contrasena");

                return new Response(true, "Solicitud de recuperacion de contrasena creada correctamente");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error al crear la solicitud para recuprar la contrasena del usuario desde repository");
            }
        }

        public async Task<Response> EliminarSolicitud(string userId)
        {
            try
            {
                var solicitud = await _context.SolicitudesContrasena.FindAsync(userId);
                if (solicitud is null)
                    return new Response(false, "Solicitud no encontrada");

                _context.SolicitudesContrasena.Remove(solicitud);
                await _context.SaveChangesAsync();

                return new Response(true, "Solicitud eliminada correctamente");


            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error en eliminar desde repo"); 
            }
        }

        public async Task<IEnumerable<SolicitudContrasena>> GetBy(Expression<Func<SolicitudContrasena, bool>> predicate)
        {
            try
            {
                var solicitudes = await _context.SolicitudesContrasena.Where(predicate).ToListAsync()!;
                return solicitudes is not null ? solicitudes : null!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error en el metodo Get by en el repositorio de solicitudes de contrasena");
            }
        }

        public async Task<Response> ProcesarSolicitud(string userId)
        {
            try
            {
                var solicitud = await _context.SolicitudesContrasena.FindAsync(userId);

                if (solicitud is null)
                    return new Response(false, "Solicitud no encontrada");

                //obtener al usuario a editar
                var usuario = await _context.Usuarios.FindAsync(userId);

                if (usuario is null)
                    return new Response(false, "Usuario no encontrado");

                //generar la nueva contrasena del usuario
                var password = _random.GenerateRandomPassword();

                //actualizar la contrasena del usuario
                usuario.Contrasena = BCrypt.Net.BCrypt.HashPassword(password);
                _context.Usuarios.Update(usuario);
                await _context.SaveChangesAsync();

                //enviar correo al usuario con la nueva contrasena
                string subject = "Datos de acceso - Sistema Escolar";
                string body = $@"
        <p>Hola <strong>{usuario.NombreCompleto}</strong>,</p>
        <p>Tu solicitud ha sido procesada exitosamente. A continuación, se encuentra la nueva contrasena de acceso:</p>
        <hr/>
        <p><strong>Alumno:</strong><br/>
        Usuario: {usuario.Id}<br/>
        Contraseña: {password}</p>

        <p>⚠️ Por seguridad, te recomendamos iniciar sesión y cambiar la contraseña en cuanto sea posible.</p>
        <br/>
        <p>Atentamente,<br/>Sistema Escolar</p>";

                var notification = new EmailNotificationDTO(usuario.Correo!, subject, body);

                try
                {
                    // Enviar la notificación a través de RabbitMQ usando NotificationPublisher
                    notificationPublisher.PublishEmailNotification(notification);
                }
                catch (Exception ex)
                {
                    LogException.LogExceptions(ex);
                    return new Response(true, "contrasena modificada, pero no se pudo enviar el correo.");
                }

                solicitud.Procesada = true;

                return new Response(true, "Solicitud procesada y correo enviado con la nueva contrasena.");

            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error al procesar la solicitud para recuprar la contrasena del usuario desde repository");
            }
        }
    }
}
