using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleApi.Domain.Entities
{
    public class Schedule
    {
        //para guardar un elemento del horario se requiere guardar la id, la id del usuario, la id de la materia
        //la hora de inicio, el dia
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public int IdMateria { get; set; }
        public string? HoraInicio { get; set; }
        public string? Dia { get; set; }

    }
}
