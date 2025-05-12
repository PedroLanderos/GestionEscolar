using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleApi.Application.DTOs
{
    public class SubjectToScheduleDTO
    {
        public int Id { get; set; }
        [Required]
        public string? IdMateria { get; set; }
        [Required]
        public int? IdHorario { get; set; }
        [Required]
        public string? HoraInicio { get; set; }
        [Required]
        public string? Dia { get; set; }
    }
}
