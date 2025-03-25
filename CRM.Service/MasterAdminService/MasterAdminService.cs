using CRM.Application.DTO;
using CRM.Application.Interfaces;
using CRM.domain.Interface;

namespace CRM.Service.MasterAdminService
{
    public class MasterAdminService : IMasterAdminService
    {
        private readonly IUnitOfWork _unitofwork;
        private readonly ITokenService _tokenservice;
        public MasterAdminService(IUnitOfWork unitofwork, ITokenService tokenservice) 
        { 
            _unitofwork = unitofwork;
            _tokenservice = tokenservice;
        }
        public async Task<string?> MasterAdminLogin(LoginMasterAdminDTO entity)
        {
            var MasterAdmin = await _unitofwork.MasterAdmin.Login(entity);
            return MasterAdmin != null ? _tokenservice.MasterAdminGenerateToken(MasterAdmin.MasterAdminEmail!, MasterAdmin.RoleId) : null;
        }
    }
}
