using Microsoft.EntityFrameworkCore;
using ScheduleApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleApi.Infrastructure.Data
{
    public class ScheduleDbContext(DbContextOptions<ScheduleDbContext> options) : DbContext(options)
    {
        public DbSet<Schedule> Schedules { get; set; } 
        public DbSet<SubjectToSchedule> SubjectToSchedules { get; set; }
        public DbSet<ScheduleToUser> ScheduleToUsers { get; set; } 
    }
}
