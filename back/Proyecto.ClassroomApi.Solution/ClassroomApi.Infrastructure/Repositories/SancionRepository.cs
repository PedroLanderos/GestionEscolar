using ClassroomApi.Application.DTOs;
using ClassroomApi.Application.Interfaces;
using ClassroomApi.Application.Mapper;
using ClassroomApi.Application.Services;
using ClassroomApi.Domain.Entities;
using ClassroomApi.Infrastructure.Data;
using Llaveremos.SharedLibrary.Logs;
using Llaveremos.SharedLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using Proyecto.SharedLibrary.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ClassroomApi.Infrastructure.Repositories
{
    public class SancionRepository(ClassroomDbContext context, IAuthentication authentication, IEmail emailService) : ISancion
    {
        private static readonly HashSet<string> TiposPermitidos = new()
        {
            "Riña",
            "Asistencia",
            "No trabajo",
            "Vandalismo",
            "Exhibicionismo",
            "Acoso"
        };

        public async Task<Response> CrearSancion(SancionDTO dto)
        {
            if (!TipoValido(dto.TipoSancion))
                return new Response(false, $"Tipo de sanción inválido. Los valores permitidos son: {string.Join(", ", TiposPermitidos)}");

            try
            {
                var entity = SancionMapper.ToEntity(dto);
                await context.Sanciones.AddAsync(entity);
                await context.SaveChangesAsync();

                // Enviar al correo del padre la sanción
                try
                {
                    // Obtener el alumno para obtener correo del padre/tutor
                    var alumno = await authentication.ValidateUser(dto.IdAlumno);
                    if (alumno == null)
                        return new Response(false, "Alumno no encontrado para enviar correo");

                    string asunto = $"Notificación de Sanción: {dto.TipoSancion}";
                    string cuerpo = $@"
                    <p>Estimado padre/tutor,</p>
                    <p>Se ha registrado una nueva sanción para su hijo(a) <strong>{alumno.NombreCompleto}</strong> con los siguientes detalles:</p>
                    <ul>
                        <li><strong>Tipo de Sanción:</strong> {dto.TipoSancion}</li>
                        <li><strong>Descripción:</strong> {dto.Descripcion}</li>
                        <li><strong>Fecha:</strong> {dto.Fecha:dd/MM/yyyy}</li>
                        <li><strong>Profesor:</strong> {dto.IdProfesor}</li>
                    </ul>
                    <p>Por favor, tome las medidas necesarias.</p>
                    <p>Atentamente,<br/>Sistema Escolar</p>";

                    bool correoEnviado = await emailService.EnviarCorreoAsync(alumno.Correo!, asunto, cuerpo);
                    if (!correoEnviado)
                    {
                        LogException.LogExceptions(new Exception("Error enviando correo de sanción"));
                        return new Response(false, "Sanción creada pero no se pudo enviar el correo.");
                    }
                }
                catch (Exception ex)
                {
                    LogException.LogExceptions(ex);
                    return new Response(true, "Sanción creada, pero hubo un error enviando el correo.");
                }

                return new Response(true, "Sanción creada exitosamente y correo enviado.");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error al crear la sanción");
            }
        }


        public async Task<Response> ActualizarSancion(SancionDTO dto)
        {
            if (!TipoValido(dto.TipoSancion))
                return new Response(false, $"Tipo de sanción inválido. Los valores permitidos son: {string.Join(", ", TiposPermitidos)}");

            try
            {
                var existing = await context.Sanciones.FindAsync(dto.Id);
                if (existing == null)
                    return new Response(false, "Sanción no encontrada");

                existing.TipoSancion = dto.TipoSancion;
                existing.Descripcion = dto.Descripcion;
                existing.Fecha = dto.Fecha;
                existing.IdProfesor = dto.IdProfesor;
                existing.IdAlumno = dto.IdAlumno;

                context.Sanciones.Update(existing);
                await context.SaveChangesAsync();

                // Enviar al correo del padre la sanción actualizada
                try
                {
                    var alumno = await authentication.ValidateUser(dto.IdAlumno);
                    if (alumno == null)
                        return new Response(false, "Alumno no encontrado para enviar correo");

                    string asunto = $"Actualización de Sanción: {dto.TipoSancion}";
                    string cuerpo = $@"
                    <p>Estimado padre/tutor,</p>
                    <p>Se ha actualizado la sanción para su hijo(a) <strong>{alumno.NombreCompleto}</strong> con los siguientes detalles nuevos:</p>
                    <ul>
                        <li><strong>Tipo de Sanción:</strong> {dto.TipoSancion}</li>
                        <li><strong>Descripción:</strong> {dto.Descripcion}</li>
                        <li><strong>Fecha:</strong> {dto.Fecha:dd/MM/yyyy}</li>
                        <li><strong>Profesor:</strong> {dto.IdProfesor}</li>
                    </ul>
                    <p>Por favor, tome las medidas necesarias.</p>
                    <p>Atentamente,<br/>Sistema Escolar</p>";

                    bool correoEnviado = await emailService.EnviarCorreoAsync(alumno.Correo!, asunto, cuerpo);
                    if (!correoEnviado)
                    {
                        LogException.LogExceptions(new Exception("Error enviando correo de actualización de sanción"));
                        return new Response(true, "Sanción actualizada, pero no se pudo enviar el correo.");
                    }
                }
                catch (Exception ex)
                {
                    LogException.LogExceptions(ex);
                    return new Response(true, "Sanción actualizada, pero hubo un error enviando el correo.");
                }

                return new Response(true, "Sanción actualizada correctamente y correo enviado.");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error al actualizar la sanción");
            }
        }


        public async Task<Response> EliminarSancion(SancionDTO dto)
        {
            try
            {
                var sancion = await context.Sanciones.FindAsync(dto.Id);
                if (sancion == null)
                    return new Response(false, "Sanción no encontrada");

                context.Sanciones.Remove(sancion);
                await context.SaveChangesAsync();

                return new Response(true, "Sanción eliminada correctamente");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error al eliminar la sanción");
            }
        }

        public async Task<IEnumerable<SancionDTO>> ObtenerSanciones()
        {
            try
            {
                var sanciones = await context.Sanciones.ToListAsync();
                return SancionMapper.FromEntityList(sanciones);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error al obtener las sanciones");
            }
        }

        public async Task<SancionDTO> ObtenerSancionPorId(int id)
        {
            try
            {
                var sancion = await context.Sanciones.FindAsync(id);
                if (sancion == null)
                    return null!;

                return SancionMapper.FromEntity(sancion);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error al obtener la sanción por ID");
            }
        }

        public async Task<IEnumerable<Sancion>> GetBy(Expression<Func<Sancion, bool>> predicate)
        {
            try
            {
                return await context.Sanciones.Where(predicate).ToListAsync();
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error al filtrar las sanciones");
            }
        }

        private bool TipoValido(string tipo)
        {
            return TiposPermitidos.Contains(tipo.Trim(), StringComparer.OrdinalIgnoreCase);
        }
    }
}
