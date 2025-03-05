using CRMuser.Application.DTO;
using CRMUser.domain.Model;

namespace CRMUser.domain.Interface
{
    public interface IUserRepository
    {
        Task Register(User entity);

        Task<string> Login(LoginDTO entity);

        Task<bool> ChangePassword(ChangePasswordDTO entity);

        Task<bool> ResetPassword(ResetPasswordDTO entity);

        Task<User> GetByEmail(string email);
    }
}
