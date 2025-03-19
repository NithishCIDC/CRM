using CRM.Application.DTO;
using CRM.domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Service.AuthService
{
    public interface IAuthService
    {
        Task Register(User entity);
        Task<string?> Login(LoginDTO entity);
        Task<bool> ChangePassword(ChangePasswordDTO entity);
        Task<bool> ResetPassword(ResetPasswordDTO entity);
        Task<User?> GetByEmail(string email);
    }
}
