using CRM.Application.DTO;
using CRM.domain.Interface;
using CRM.domain.Model;
using Mapster;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CRM.Service.OrganizationService
{
    public class OrganizationService : IOrganizationService
    {
        private readonly IUnitOfWork _unitOfwork;
        private readonly string _email;
        public OrganizationService(IUnitOfWork unitOfwork,IHttpContextAccessor httpContextAccessor)
        {
            _unitOfwork = unitOfwork;
            _email = httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.Email)!;
        }
        public async Task AddOrganization(AddOrganizationDTO org)
        {
            Organization organization = org.Adapt<Organization>();
            organization.Created_By = _email;
            organization.Created_At = DateTime.UtcNow.AddHours(5).AddMinutes(30);
            await _unitOfwork.Organization.Add(organization);
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

        public async Task UpdateOrganization(UpdateOraganizationDTO organization)
        {
            Organization org = organization.Adapt<Organization>();
            org.Updated_By = _email;
            _unitOfwork.Organization.Update(org);
           await _unitOfwork.Save();
        }
    }
}
