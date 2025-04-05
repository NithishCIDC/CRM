using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM.Application.DTO;
using CRM.domain.Model;

namespace CRM.Service.OrganizationService
{
    public interface IOrganizationService 
    {
        Task AddOrganization(AddOrganizationDTO organization); 
        Task<Organization?> GetByEmail(string email);
        Task<IEnumerable<Organization>> GetAll();
        Task<Organization?> GetById(Guid id);
        Task UpdateOrganization(UpdateOraganizationDTO organization);
        Task DeleteOrganization(Organization entity);
    }
}
