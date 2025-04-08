using CRM.domain.Model;
using CRM.Application.DTO;

namespace CRM.domain.Interface
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<Branch?> GetBranch(Guid Id);
    }
}
