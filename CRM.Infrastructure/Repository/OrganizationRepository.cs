using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM.domain.Interface;
using CRM.domain.Model;
using CRM.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CRM.Infrastructure.Repository
{
    public class OrganizationRepository : GenericRepository<Organization>, IOrganizationRepository
    {
        private readonly ApplicationDbContext _dbcontext;
        public OrganizationRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbcontext = dbContext;
        }

        public async Task<Organization> GetbyEmail(string email)
        {
           var response = await _dbcontext.Organization.FirstOrDefaultAsync(x => x.Email == email);
           return response!;
        }
    }
}
