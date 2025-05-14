using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubjectsApi.Application.DTOs
{
    public class UserDTO
    {
        [Required]
        public string? Id { get; set; }

        [Required]
        public string NombreCompleto { get; set; } = null!;

        [Required, EmailAddress]
        public string Correo { get; set; } = null!;

        [Required]
        public string Curp { get; set; } = null!;

        [Required]
        public bool CuentaBloqueada { get; set; }

        [Required]
        public bool DadoDeBaja { get; set; }

        [Required]
        public DateTime UltimaSesion { get; set; }

        [Required]
        public string Rol { get; set; } = null!;
    }
}
