using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleApi.Domain.Entities
{
    public class Schedule
    {
        public int Id { get; set; }
        public int? Grado { get; set; }
        public string? Grupo { get; set; }

    }
}
