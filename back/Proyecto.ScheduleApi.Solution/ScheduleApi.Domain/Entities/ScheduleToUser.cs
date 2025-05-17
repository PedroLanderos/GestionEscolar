using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleApi.Domain.Entities
{
    public class ScheduleToUser
    {
        [Key]
        public int Id { get; set; }
        public int IdSchedule { get; set; }
        public string? IdUser { get; set; }

    }
}
