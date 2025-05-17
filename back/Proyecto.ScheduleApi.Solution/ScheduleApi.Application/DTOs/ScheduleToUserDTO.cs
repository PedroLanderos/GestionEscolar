using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleApi.Application.DTOs
{
    public class ScheduleToUserDTO
    {
        public int Id { get; set; }
        [Required]
        public string? IdUser { get; set; }
        [Required]
        public int? IdSchedule { get; set; }
    }
}
