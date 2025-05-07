using Microsoft.EntityFrameworkCore;
using ClassroomApi.Domain.Entities;
using System;

namespace ClassroomApi.Infrastructure.Data
{
    public class ClassroomDbContext : DbContext
    {
        public ClassroomDbContext(DbContextOptions<ClassroomDbContext> options) : base(options)
        {
        }

        public DbSet<Asistencia> Asistencias { get; set; }
        public DbSet<Calificacion> Calificaciones { get; set; }
        public DbSet<Sancion> Sanciones { get; set; }
        public DbSet<Reporte> Reportes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Asistencia>()
                .HasKey(a => a.Id); 
            modelBuilder.Entity<Calificacion>()
                .HasKey(c => c.Id); 
            modelBuilder.Entity<Sancion>()
                .HasKey(s => s.Id); 
            modelBuilder.Entity<Reporte>()
                .HasKey(r => r.Id); 
        }
    }
}
