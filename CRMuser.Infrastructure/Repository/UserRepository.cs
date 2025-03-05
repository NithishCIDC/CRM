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
        private readonly IOtpService _otpService;

        public UserRepository(UserDbContext dbContext, ITokenService tokenService, IEmailService emailService, IOtpService otpService)
        {
            _dbcontext = dbContext;
            _tokenService = tokenService;
            _emailService = emailService;
            _otpService = otpService;
        }


        public async Task<bool> ChangePassword(ChangePasswordDTO entity)
        {
            var oldPassworddata = await _dbcontext.Users.FirstOrDefaultAsync(x => x.Email == entity.Email && x.Password == entity.OldPassword);

            if (oldPassworddata is not null)
            {
                oldPassworddata.Password = entity.NewPassword;
                _dbcontext.Users.Update(oldPassworddata);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<User> GetByEmail(string email)
        {
            var data = await _dbcontext.Users.FirstOrDefaultAsync(x => x.Email == email);
            return data!;
            
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
            _emailService.Email(entity.Email!, "Welcome to CRM", "You have successfully registered");
        }

        public async Task<bool> ResetPassword(ResetPasswordDTO entity)
        {
            var email = _otpService.GetVerifiedEmail();

            var resetdata = await GetByEmail(email);

            if (resetdata is not null)
            {
                if (entity.NewPassword == entity.ConfirmPassword)
                {
                    resetdata.Password = entity.NewPassword;
                    _dbcontext.Users.Update(resetdata);
                    await _dbcontext.SaveChangesAsync();
                    return true;
                }
                return false;

            }
            return false;
        }
    }
}
