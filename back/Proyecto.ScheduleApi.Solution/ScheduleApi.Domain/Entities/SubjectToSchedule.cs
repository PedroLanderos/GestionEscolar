using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleApi.Domain.Entities
{
    public class SubjectToSchedule
    {
        public int Id { get;set; }
        public string? IdMateria { get; set; }
        public int? IdHorario { get; set; }
        public string? HoraInicio { get; set; }
        public string? Dia { get; set; } 
    }
}
