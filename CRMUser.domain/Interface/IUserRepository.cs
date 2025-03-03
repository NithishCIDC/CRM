using CRMUser.domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMUser.domain.Interface
{
    public interface IUserRepository
    {
        Task Register(User entity);

        Task<string> Login(User entity);
    }
}
