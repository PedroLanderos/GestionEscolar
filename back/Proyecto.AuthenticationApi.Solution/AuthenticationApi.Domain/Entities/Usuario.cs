using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationApi.Domain.Entities
{
    public class Usuario
    {
        [Key]
        public string? Id { get; set; }
        public string? NombreCompleto { get; set; }
        public string? Correo { get; set; }
        public string? Contrasena { get; set; }
        public string? Curp { get; set; }
        public bool? CuentaBloqueada { get; set; } = false;//para saber si la cuenta esta bloqueada
        public bool? DadoDeBaja { get; set; } = false;//para saber si el usuario sigue activo en la institucion
        public DateTime? UltimaSesion { get; set; }
        public string? Rol { get; set; }
    }
}
