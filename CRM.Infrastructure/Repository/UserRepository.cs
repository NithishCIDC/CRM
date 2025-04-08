using CRM.Infrastructure.Data;
using CRM.Application.DTO;
using CRM.domain.Interface;
using CRM.domain.Model;
using Microsoft.EntityFrameworkCore;

namespace CRM.Infrastructure.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly ApplicationDbContext _dbcontext;

        public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbcontext = dbContext;
        }

        public async Task<Branch?> GetBranch(Guid id)
        {
            return await _dbcontext.Branches.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
