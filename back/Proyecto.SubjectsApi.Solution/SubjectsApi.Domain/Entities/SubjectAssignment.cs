using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubjectsApi.Domain.Entities
{
    public class SubjectAssignment
    {
        [Key]
        public string? Id { get; set; }
        public string? SubjectId { get; set; }
        public string? UserId { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime? FechaActualizacion { get; set; }
    }
}
