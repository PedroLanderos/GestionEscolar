using AuthenticationApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationApi.Infrastructure.Data
{
    public class AuthenticationDbContext(DbContextOptions<AuthenticationDbContext> options) : DbContext(options)
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<SolicitudAlta> SolicitudesAltas { get; set; }
        public DbSet<SolicitudContrasena> SolicitudesContrasena { get; set; }

    }
}
