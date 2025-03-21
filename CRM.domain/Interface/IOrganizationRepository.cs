
using CRM.domain.Model;

namespace CRM.domain.Interface
{
    public interface IOrganizationRepository : IGenericRepository<Organization>
    {
        Task<Organization> GetbyEmail(string email);
    }
}
