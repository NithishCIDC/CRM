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
    public class MasterAdminRepository : GenericRepository<MasterAdmin>, IMasterAdminRepository
    {
        private readonly ApplicationDbContext _dbcontext;
        public MasterAdminRepository(ApplicationDbContext dbcontext) : base(dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<MasterAdmin> Login(LoginMasterAdminDTO entity)
        {
            var MasterAdmin = await _dbcontext.MasterAdmin.FirstOrDefaultAsync(x => x.MasterAdminEmail == entity.MasterAdminEmail && x.MasterAdminPassword == entity.MasterAdminPassword);
            return MasterAdmin!;
        }
    }
}
