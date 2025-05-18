using Llaveremos.SharedLibrary.Responses;
using ScheduleApi.Application.DTOs;
using ScheduleApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleApi.Application.Interfaces
{
    public interface ISchedule
    {
        Task<Response> CreateSchedule(ScheduleDTO schedule);
        Task<Response> UpdateScheduleAsync(ScheduleDTO schedule);
        Task<Response> AsignSubjectToScheduleAsync(SubjectToScheduleDTO subjectToScheduleDTO);

        Task<Response> UpdateAsignmentAsync(SubjectToScheduleDTO subjectToScheduleDTO);
        Task<Response> DeleteScheduleAsync(int id);
        Task<Response> DeleteAsignSubjectToScheduleAsync(int id);
        Task<ScheduleDTO> GetSchedule(int id);
        Task<IEnumerable<ScheduleDTO>> GetSchedules();
        Task<IEnumerable<SubjectToSchedule>> GetFullSchedule(int id);
        Task<IEnumerable<SubjectToSchedule>> GetBy(Expression<Func<SubjectToSchedule, bool>> predicate);

        //apartado para asignar horarios a alumnos
        Task<Response> AsignStudentToScheduleAsync(ScheduleToUserDTO dto);
        Task<Response> UpdateAsignStudentToScheduleAsync(ScheduleToUserDTO dto);
        Task<Response> DeleteAsignStudentToScheduleAsync(int id);
        //apartado para obtener los horarios de los profesores y de los alumnos
        Task<IEnumerable<SubjectToScheduleDTO>> GetScheduleForTeacherAsync(string idUsuario);
        Task<IEnumerable<SubjectToScheduleDTO>> GetScheduleForStudentAsync(string idUsuario);
    }
}