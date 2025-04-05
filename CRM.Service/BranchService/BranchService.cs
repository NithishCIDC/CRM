using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CRM.Application.DTO;
using CRM.domain.Interface;
using CRM.domain.Model;
using Mapster;
using Microsoft.AspNetCore.Http;

namespace CRM.Service.BranchService
{
    public class BranchService : IBranchService
    {
        private readonly IUnitOfWork _unitOfwork;
        private readonly string _email;
        public BranchService(IUnitOfWork unitOfwork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfwork = unitOfwork;
            _email = httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.Email)!;
        }
        public async Task AddBranch(AddBranchDTO branchEntity)
        {
            Branch branch = branchEntity.Adapt<Branch>();
            branch.Created_By = _email;
            branch.Created_At = DateTime.UtcNow.AddHours(5).AddMinutes(30);
            await _unitOfwork.Branch.Add(branch);
        }

        public async Task DeleteBranch(Branch entity)
        {
            await _unitOfwork.Branch.Delete(entity.Id);
        }

        public async Task<IEnumerable<Branch>> GetAll()
        {
            return await _unitOfwork.Branch.GetAll();
        }

        public async Task<Branch?> GetByEmail(string email)
        {
           return await _unitOfwork.Branch.GetByEmail(email);
        }

        public Task<Branch?> GetById(Guid id)
        {
            return _unitOfwork.Branch.GetById(id);
        }

        public async Task UpdateBranch(UpdateBranchDTO entity)
        {
            Branch branch = entity.Adapt<Branch>();
            branch.Updated_By = _email;
            await _unitOfwork.Branch.Update(branch);
        }
    }
}
