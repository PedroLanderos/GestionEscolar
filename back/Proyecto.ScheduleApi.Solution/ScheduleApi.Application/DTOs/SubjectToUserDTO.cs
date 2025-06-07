using System;
using System.ComponentModel.DataAnnotations;

namespace ScheduleApi.Application.DTOs
{
    public class SubjectToUserDTO
    {
        public string? Id { get; set; }

        [Required]
        public string? UserId { get; set; } 

        [Required]
        public string? CourseId { get; set; } 

        [Required]
        public DateTime HoraInicio { get; set; } 
    }
}
