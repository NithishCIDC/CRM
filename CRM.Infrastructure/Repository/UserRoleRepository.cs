using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM.domain.Interface;
using CRM.domain.Model;
using CRM.Infrastructure.Data;

namespace CRM.Infrastructure.Repository
{
    public class UserRoleRepository : GenericRepository<UserRoles>, IUserRoleRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public UserRoleRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

    }
}
