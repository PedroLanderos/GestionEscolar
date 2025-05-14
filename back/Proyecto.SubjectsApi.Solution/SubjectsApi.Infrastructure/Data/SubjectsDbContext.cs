using Microsoft.EntityFrameworkCore;
using SubjectsApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubjectsApi.Infrastructure.Data
{
    public class SubjectsDbContext(DbContextOptions<SubjectsDbContext> options) : DbContext(options)
    {
        public DbSet<Subject> Subjects { get; set; } 
        public DbSet<SubjectAssignment> SubjectAssignments { get; set; }
    }
    
}
