using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleApi.Domain.Entities
{
    public class SubjectToUser
    {
        [Key]
        public string Id { get; set; }
        public string? UserId { get;set; }
        public string? CourseId { get; set; } //combinacion de materia y profesor

        public DateTime HoraInicio { get; set; }
    }
}