using CRM.Infrastructure.Data;
using CRM.Application.DTO;
using CRM.Application.Interfaces;
using CRM.domain.Interface;
using CRM.domain.Model;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Ocsp;
using Org.BouncyCastle.Crypto.Generators;

namespace CRM.Infrastructure.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly ApplicationDbContext _dbcontext;

        public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbcontext = dbContext;
        }

        public async Task<bool> ChangePassword(ChangePasswordDTO entity, Guid userId)
        {
            var userData = await _dbcontext.Users.FirstOrDefaultAsync(x => x.Id == userId);

            if (userData is not null && BCrypt.Net.BCrypt.Verify(entity.OldPassword, userData.Password))
            {
                userData.Password = BCrypt.Net.BCrypt.HashPassword(entity.NewPassword);
                _dbcontext.Users.Update(userData);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<User?> GetByEmail(string email)
        {
            return await _dbcontext.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<User?> Login(LoginDTO entity)
        {
            var user = await _dbcontext.Users.FirstOrDefaultAsync(x => x.Email == entity.Email);
            return(user != null && BCrypt.Net.BCrypt.Verify(entity.Password, user.Password)) ? user : null;
        }

        public async Task Register(User entity)
        {
            entity.Password = BCrypt.Net.BCrypt.HashPassword(entity.Password);
            await _dbcontext.Users.AddAsync(entity);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task ResetPassword(ResetPasswordDTO entity)
        {
            var userData = await _dbcontext.Users.FirstOrDefaultAsync(x => x.Email == entity.email);
            if (userData is not null)
            {
                userData.Password = BCrypt.Net.BCrypt.HashPassword(entity.NewPassword);
                _dbcontext.Users.Update(userData);
                await _dbcontext.SaveChangesAsync();
            }
        }
    }
}
