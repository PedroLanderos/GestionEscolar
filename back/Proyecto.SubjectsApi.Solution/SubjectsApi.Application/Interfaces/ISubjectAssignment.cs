using Llaveremos.SharedLibrary.Responses;
using SubjectsApi.Application.DTOs;
using SubjectsApi.Domain.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SubjectsApi.Application.Interfaces
{
    public interface ISubjectAssignment
    {
        Task<Response> CreateAsignmnet(SubjectAssignmentDTO dto);
        Task<Response> UpdateAsignmnet(SubjectAssignmentDTO dto);
        Task<Response> DeleteAsignmnet(string id);
        Task<SubjectAssignmentDTO> GetById(string id);
        Task<IEnumerable<SubjectAssignmentDTO>> GetAsignmnets();
        Task<IEnumerable<SubjectAssignment>> GetBy(Expression<Func<SubjectAssignment, bool>> predicate);
        Task<IEnumerable<SubjectAssignmentDTO>> GetAssignmentByGrade(int grado);
        Task<IEnumerable<SubjectAssignmentDTO>> GetWorkShopsAssingmentByGradeAsync(int grado);

    }
}
