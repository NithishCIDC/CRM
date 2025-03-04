using CRMuser.Application.DTO;
using CRMuser.Application.Interfaces;
using CRMuser.Infrastructure.Data;
using CRMUser.domain.Interface;
using CRMUser.domain.Model;
using Microsoft.EntityFrameworkCore;

namespace CRMuser.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext _dbcontext;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;

        public UserRepository(UserDbContext dbContext, ITokenService tokenService, IEmailService emailService)
        {
            _dbcontext = dbContext;
            _tokenService = tokenService;
            _emailService = emailService;
        }

        public async Task<bool> ChangePassword(ChangePasswordDTO entity)
        {
            var newPassworddata = await _dbcontext.Users.FirstOrDefaultAsync(x => x.Email == entity.Email && x.Password == entity.Password);

            if (newPassworddata is not null)
            {
                newPassworddata.Password = entity.NewPassword;
                _dbcontext.Users.Update(newPassworddata);
                await _dbcontext.SaveChangesAsync();

                return true;
            }
            return false;
        }

        public async Task<string> Login(LoginDTO entity)
        {
            var user = await _dbcontext.Users.FirstOrDefaultAsync(x => x.Email == entity.Email && x.Password == entity.Password);

            if (user is not null)
            {
                return _tokenService.GenerateToken(user.Email!, user.Name!);
            }
            return null!;
        }

        public async Task Register(User entity)
        {
            await _dbcontext.Users.AddAsync(entity);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task<bool> ResetPassword(ResetPasswordDTO entity)
        {
            var resetdata = await _dbcontext.Users.FirstOrDefaultAsync(x => x.Email == entity.Email);

            if (resetdata is not null)
            {
                resetdata.Password = entity.NewPassword;
                 _dbcontext.Users.Update(resetdata);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
