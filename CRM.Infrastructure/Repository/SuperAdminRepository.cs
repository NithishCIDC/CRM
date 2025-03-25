using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM.Application.DTO;
using CRM.domain.Interface;
using CRM.domain.Model;
using CRM.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CRM.Infrastructure.Repository
{
    public class SuperAdminRepository : GenericRepository<SuperAdmin>, ISuperAdminRepository
    {
        private readonly ApplicationDbContext _dbcontext;
        public SuperAdminRepository(ApplicationDbContext dbcontext) : base(dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<SuperAdmin> Login(LoginSuperAdminDTO entity)
        {
            var superadmin = await _dbcontext.SuperAdmin.FirstOrDefaultAsync(x => x.SuperAdminEmail == entity.SuperAdminEmail && x.SuperAdminPassword == entity.SuperAdminPassword);
            return superadmin!;
        }
    }
}
