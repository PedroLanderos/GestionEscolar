using ScheduleApi.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleApi.Application.Services
{
    public interface ISubject 
    {
        Task<SubjectDTO> ObtenerMateriaPorCodigo(string codigo);
    }
}
