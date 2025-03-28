﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM.domain.Interface;
using CRM.Infrastructure.Data;

namespace CRM.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbcontext;
        public UnitOfWork(ApplicationDbContext dbcontext) 
        {
            _dbcontext = dbcontext;
            Branch = new BranchRepository(_dbcontext);
            Organization = new OrganizationRepository(_dbcontext);
            User = new UserRepository(_dbcontext);
            MasterAdmin = new MasterAdminRepository(_dbcontext);
        }
        public IBranchRepository Branch { get; private set; }

        public IOrganizationRepository Organization { get; private set; }

        public IUserRepository User { get; private set; }

        public IMasterAdminRepository MasterAdmin { get; private set; }

        public void Dispose()
        {
           _dbcontext.Dispose();
        }

        public async Task Save()
        {
            await _dbcontext.SaveChangesAsync();
        }
    }
}
