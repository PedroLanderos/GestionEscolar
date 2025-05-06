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
        Task<Response> Create(ScheduleDTO schedule);
        Task<Response> Update(ScheduleDTO schedule);
        Task<IEnumerable<Schedule>> GetBy(Expression<Func<ScheduleDTO, bool>> predicate); 
    }
}
