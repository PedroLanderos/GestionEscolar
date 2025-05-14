using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubjectsApi.Application.DTOs
{
    public class SubjectAssignmentDTO
    {
        [Required]
        public string? Id { get; set; }
        [Required]
        public string? SubjectId { get; set; }
        [Required]
        public string? UserId { get; set; }
    }
}
