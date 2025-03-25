using CRM.Application.DTO;
using CRM.domain.Model;

namespace CRM.domain.Interface
{
    public interface IMasterAdminRepository : IGenericRepository<MasterAdmin>
    {
        Task<MasterAdmin> Login(LoginMasterAdminDTO entity);
    }
}
