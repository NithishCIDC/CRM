using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.domain.Interface
{
    public interface IUnitOfWork
    {
        IUserRepository User { get; }
    }
}
