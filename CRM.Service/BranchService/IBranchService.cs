using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM.Application.DTO;
using CRM.domain.Model;

namespace CRM.Service.BranchService
{
    public interface IBranchService
    {
        Task AddBranch(AddBranchDTO branch);
        Task<Branch> GetByEmail(string email);
        Task<IEnumerable<Branch>> GetAll();
        Task<Branch> GetById(Guid id);
        Task UpdateBranch(Branch entity);
        Task DeleteBranch(Branch entity);
    }
}
