using CRM.Application.DTO;
using CRM.domain.Model;

namespace CRM.domain.Interface
{
    public interface ISuperAdminRepository : IGenericRepository<SuperAdmin>
    {
        Task<SuperAdmin> Login(LoginSuperAdminDTO entity);
    }
}
