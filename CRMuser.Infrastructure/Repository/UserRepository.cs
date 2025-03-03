using CRMuser.Application.DTO;
using CRMuser.Infrastructure.Data;
using CRMUser.domain.Interface;
using CRMUser.domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMuser.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext _dbcontext;
        private readonly ITokenGeneration _tokenGeneration;

        public UserRepository(UserDbContext dbContext,ITokenGeneration tokengeneration)
        {
            _dbcontext = dbContext;
            _tokenGeneration = tokengeneration;
        }
        public async Task<string> Login(User entity)
        {
            var user = await _dbcontext.Users.FirstOrDefaultAsync(x => x.Email == entity.Email && x.Password == entity.Password);

            if (user is not null)
            {
                if (user.Email == entity.Email && user.Password == entity.Password)
                {
                    return _tokenGeneration.GenerateToken(user.Email!);
                }
                else
                {
                    Console.WriteLine("Invalid Email or Password");
                }
            }
            return null!;
        }

        public async Task Register(User entity)
        {
            await _dbcontext.Users.AddAsync(entity);
            await _dbcontext.SaveChangesAsync();
        }

        
    }
}
