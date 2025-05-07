using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassroomApi.Application.DTOs
{
    public record ReporteDTO
    (
        int Id,
        [Required] DateTime Fecha,  
        string Descripcion,  
        [Required] string TipoReporte  
    );
}
