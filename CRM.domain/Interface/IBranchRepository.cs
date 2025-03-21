using CRM.domain.Model;

namespace CRM.domain.Interface
{
    public interface IBranchRepository : IGenericRepository<Branch>
    {
        Task<Branch> GetbyEmail(string email);
    }
}
