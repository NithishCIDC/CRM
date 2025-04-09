using CRM.domain.Interface;
using CRM.domain.Model;
using CRM.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CRM.Infrastructure.Repository
{
    public class RolePermissionRepository : GenericRepository<RolePermission>, IRolePermissionRepository
    {
        private readonly ApplicationDbContext _dbcontext;

        public RolePermissionRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbcontext = dbContext;
        }

        public async Task<IEnumerable<string>> GetPermissionsByRoleId(Guid roleId)
        {
           var permissions = await _dbcontext.RolePermissions
                .Where(x => x.RoleId == roleId)
                .Include(x => x.Permission)
                .Select(x => x.Permission!.Permission)
                .ToListAsync();
            return permissions;
        }
    }
}
