using ClassroomApi.Application.DTOs;
using ClassroomApi.Application.Interfaces;
using ClassroomApi.Application.Mapper;
using ClassroomApi.Domain.Entities;
using ClassroomApi.Infrastructure.Data;
using Llaveremos.SharedLibrary.Logs;
using Llaveremos.SharedLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ClassroomApi.Infrastructure.Repositories
{
    public class CicloEscolarRepository : ICicloEscolar
    {
        private readonly ClassroomDbContext context;

        public CicloEscolarRepository(ClassroomDbContext context)
        {
            this.context = context;
        }

        public async Task<Response> AgregarCicloEscolar(CicloEscolarDTO dto)
        {
            try
            {
                var entity = CicloEscolarMapper.ToEntity(dto);
                await context.CiclosEscolares.AddAsync(entity);
                await context.SaveChangesAsync();

                return new Response(true, "Ciclo escolar creado exitosamente");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error al crear el ciclo escolar");
            }
        }

        public async Task<Response> ActualizarCicloEscolar(CicloEscolarDTO dto)
        {
            try
            {
                var existing = await context.CiclosEscolares.FindAsync(dto.Id);
                if (existing == null)
                    return new Response(false, "Ciclo escolar no encontrado");

                existing.FechaRegistroCalificaciones = dto.FechaRegistroCalificaciones;
                existing.FechaInicio = dto.FechaInicio;
                existing.FechaFin = dto.FechaFin;
                existing.EsActual = dto.EsActual;

                context.CiclosEscolares.Update(existing);
                await context.SaveChangesAsync();

                return new Response(true, "Ciclo escolar actualizado correctamente");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error al actualizar el ciclo escolar");
            }
        }

        public async Task<Response> EliminarCicloEscolar(CicloEscolarDTO dto)
        {
            try
            {
                var existing = await context.CiclosEscolares.FindAsync(dto.Id);
                if (existing == null)
                    return new Response(false, "Ciclo escolar no encontrado");

                context.CiclosEscolares.Remove(existing);
                await context.SaveChangesAsync();

                return new Response(true, "Ciclo escolar eliminado correctamente");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error al eliminar el ciclo escolar");
            }
        }

        public async Task<IEnumerable<CicloEscolarDTO>> ObtenerCiclosEscolares()
        {
            try
            {
                var entities = await context.CiclosEscolares.ToListAsync();
                return CicloEscolarMapper.FromEntityList(entities);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error al obtener los ciclos escolares");
            }
        }

        public async Task<CicloEscolarDTO> ObtenerCicloEscolarPorId(string id)
        {
            try
            {
                var entity = await context.CiclosEscolares.FindAsync(id);
                if (entity == null)
                    return null!;

                return CicloEscolarMapper.FromEntity(entity);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error al obtener ciclo escolar por id");
            }
        }

        public async Task<IEnumerable<CicloEscolar>> GetManyBy(Expression<Func<CicloEscolar, bool>> predicate)
        {
            try
            {
                return await context.CiclosEscolares.Where(predicate).ToListAsync();
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error al obtener ciclos escolares con filtro");
            }
        }

        public async Task<CicloEscolar> GetBy(Expression<Func<CicloEscolar, bool>> predicate)
        {
            try
            {
                
                var ciclo = await context.CiclosEscolares.FirstOrDefaultAsync(predicate);

                if (ciclo is null) return null!;

                return ciclo;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error al obtener ciclo escolar con filtro");
            }
        }
    }
}
