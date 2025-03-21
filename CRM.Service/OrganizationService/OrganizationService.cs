using CRM.Application.DTO;
using CRM.domain.Interface;
using CRM.domain.Model;
using Mapster;

namespace CRM.Service.OrganizationService
{
    public class OrganizationService : IOrganizationService
    {
        private readonly IUnitOfWork _unitOfwork;
        public OrganizationService(IUnitOfWork unitOfwork)
        {
            _unitOfwork = unitOfwork;
        }
        public async Task AddOrganization(AddOrganizationDTO org)
        {
            await _unitOfwork.Organization.Add(org.Adapt<Organization>());
            await _unitOfwork.Save();
        }

        public async Task DeleteOrganization(Organization entity)
        {
            await _unitOfwork.Organization.Delete(entity.Id);
            await _unitOfwork.Save();    
        }

        public async Task<IEnumerable<Organization>> GetAll()
        {
            return await _unitOfwork.Organization.GetAll();
        }

        public async Task<Organization> GetByEmail(string email)
        {
           return await _unitOfwork.Organization.GetbyEmail(email);
        }

        public async Task<Organization> GetById(Guid id)
        {
           return await _unitOfwork.Organization.GetById(id);
        }

        public async Task UpdateOrganization(Organization organization)
        {
           _unitOfwork.Organization.Update(organization);
           await _unitOfwork.Save();
        }
    }
}
