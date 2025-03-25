using CRM.Application.DTO;

namespace CRM.Service.MasterAdminService
{
    public interface IMasterAdminService
    {
        Task<string?> MasterAdminLogin(LoginMasterAdminDTO entity);
    }
}
