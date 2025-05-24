using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassroomApi.Application.DTOs
{
    public record ScheduleDTO(int Id,
        [Required] int Grado,
        [Required] string Grupo);
}
