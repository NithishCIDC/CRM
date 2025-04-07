﻿using CRM.domain.Model;
using CRM.Application.DTO;

namespace CRM.domain.Interface
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task Register(User entity);
        Task<User?> Login(LoginDTO entity);
        Task<bool> ChangePassword(ChangePasswordDTO entity,Guid userId);
        Task ResetPassword(ResetPasswordDTO entity);
        Task<Branch?> GetBranch(Guid Id);
    }
}
