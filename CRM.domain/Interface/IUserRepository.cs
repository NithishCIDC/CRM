using CRM.domain.Model;
using CRM.Application.DTO;

namespace CRM.domain.Interface
{
    public interface IUserRepository
    {
        Task Register(User entity);

        Task<User?> Login(LoginDTO entity);

        Task<bool> ChangePassword(ChangePasswordDTO entity,string email);

        Task ResetPassword(User entity);

        Task<User?> GetByEmail(string email);
    }
}
