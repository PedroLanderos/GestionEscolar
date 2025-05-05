using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubjectsApi.Application.DTOs
{
    public class SubjectDTO
    {
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; } = null!;

        [Required]
        public string Codigo { get; set; } = null!;

        [Required]
        public string Tipo { get; set; } = null!;

        [Required]
        public int Grado { get; set; }

        [Required]
        public bool EsObligatoria { get; set; }

        [Required]
        public bool EstaActiva { get; set; }

        [Required]
        public DateTime FechaCreacion { get; set; }

        public DateTime? FechaActualizacion { get; set; }
    }
}
