using SubjectsApi.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassroomApi.Application.Services
{
    public interface ISubject
    {
        Task<SubjectDTO> GetSubject(int id);
    }
}
