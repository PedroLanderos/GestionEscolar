using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassroomApi.Domain.Entities
{
    public class CicloEscolar
    {
        [Key]
        public string? Id { get; set; } //ej 2025/1-2025/2
        public DateTime FechaRegistroCalificaciones { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public bool EsActual { get; set; } //para saber si es el ciclo actual 
    }
}
