using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM.Application.DTO;
using CRM.domain.Interface;
using CRM.domain.Model;
using Mapster;

namespace CRM.Service.BranchService
{
    public class BranchService : IBranchService
    {
        private readonly IUnitOfWork _unitOfwork;
        public BranchService(IUnitOfWork unitOfwork)
        {
            _unitOfwork = unitOfwork;
        }
        public async Task AddBranch(AddBranchDTO branch)
        {
            await _unitOfwork.Branch.Add(branch.Adapt<Branch>());
            await _unitOfwork.Save();
        }

        public async Task DeleteBranch(Branch entity)
        {
            await _unitOfwork.Branch.Delete(entity.Id);
            await _unitOfwork.Save();
        }

        public async Task<IEnumerable<Branch>> GetAll()
        {
            return await _unitOfwork.Branch.GetAll();
        }

        public async Task<Branch> GetByEmail(string email)
        {
           return await _unitOfwork.Branch.GetbyEmail(email);
        }

        public Task<Branch> GetById(Guid id)
        {
            return _unitOfwork.Branch.GetById(id);
        }

        public async Task UpdateBranch(Branch entity)
        {
            _unitOfwork.Branch.Update(entity);
            await _unitOfwork.Save();
        }
    }
}
