using Llaveremos.SharedLibrary.Interface;
using SubjectsApi.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SubjectsApi.Application.Interfaces
{
    public interface ISubject : IGenericInterface<SubjectDTO>
    {
        Task<IEnumerable<SubjectDTO>> GetManyByAsync(Expression<Func<SubjectDTO, bool>> predicate);
        Task<SubjectDTO> GetByCode(string code);
    }
}
