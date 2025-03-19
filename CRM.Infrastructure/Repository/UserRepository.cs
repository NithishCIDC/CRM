using CRM.Infrastructure.Data;
using CRM.Application.DTO;
using CRM.Application.Interfaces;
using CRM.domain.Interface;
using CRM.domain.Model;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Ocsp;

namespace CRM.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbcontext;

        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbcontext = dbContext;
        }


        public async Task<bool> ChangePassword(ChangePasswordDTO entity, string email)
        {
            var userData = await _dbcontext.Users.FirstOrDefaultAsync(x => x.Email == email && x.Password == entity.OldPassword);

            if (userData is not null)
            {
                userData.Password = entity.NewPassword;
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
            return await _dbcontext.Users.FirstOrDefaultAsync(x => x.Email == entity.Email && x.Password == entity.Password);
        }

        public async Task Register(User entity)
        {
            await _dbcontext.Users.AddAsync(entity);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task ResetPassword(User entity)
        {
            _dbcontext.Users.Update(entity);
            await _dbcontext.SaveChangesAsync();
        }
    }
}
