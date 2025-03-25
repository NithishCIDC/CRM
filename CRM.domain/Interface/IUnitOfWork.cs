using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.domain.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IBranchRepository Branch { get; }
        IOrganizationRepository Organization { get; }
        IUserRepository User { get; }
        ISuperAdminRepository SuperAdmin { get; }  
        Task Save();
    }
}
