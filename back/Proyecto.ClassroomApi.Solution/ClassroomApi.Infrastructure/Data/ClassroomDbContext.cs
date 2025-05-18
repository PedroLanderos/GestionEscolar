using Microsoft.EntityFrameworkCore;
using ClassroomApi.Domain.Entities;
using System;

namespace ClassroomApi.Infrastructure.Data
{
    public class ClassroomDbContext(DbContextOptions<ClassroomDbContext> options) : DbContext(options)
    {
        
        public DbSet<Asistencia> Asistencias { get; set; }
        //public DbSet<Calificacion> Calificaciones { get; set; }
        public DbSet<Sancion> Sanciones { get; set; }
        public DbSet<Reporte> Reportes { get; set; }
    }
}
