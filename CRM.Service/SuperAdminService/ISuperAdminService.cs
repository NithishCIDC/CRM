using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM.Application.DTO;

namespace CRM.Service.SuperAdminService
{
    public interface ISuperAdminService
    {
        Task<string?> SuperAdminLogin(LoginSuperAdminDTO entity);
    }
}
