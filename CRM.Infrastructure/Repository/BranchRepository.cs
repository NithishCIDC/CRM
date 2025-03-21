using CRM.domain.Interface;
using CRM.domain.Model;
using CRM.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CRM.Infrastructure.Repository
{
    public class BranchRepository : GenericRepository<Branch>, IBranchRepository
    {
        private readonly ApplicationDbContext _dbcontext;
        public BranchRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbcontext = dbContext;
        }

        public async Task<Branch> GetbyEmail(string email)
        {
            var response = await _dbcontext.Branches.FirstOrDefaultAsync(x => x.Email == email);
            return response!;
        }
    }
}
