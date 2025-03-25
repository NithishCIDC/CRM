using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM.Application.DTO;
using CRM.Application.Interfaces;
using CRM.domain.Interface;

namespace CRM.Service.SuperAdminService
{
    public class SuperAdminService : ISuperAdminService
    {
        private readonly IUnitOfWork _unitofwork;
        private readonly ITokenService _tokenservice;
        public SuperAdminService(IUnitOfWork unitofwork, ITokenService tokenservice) 
        { 
            _unitofwork = unitofwork;
            _tokenservice = tokenservice;
        }
        public async Task<string?> SuperAdminLogin(LoginSuperAdminDTO entity)
        {
            var superAdmin = await _unitofwork.SuperAdmin.Login(entity);
            return superAdmin != null ? _tokenservice.SuperAdminGenerateToken(superAdmin.SuperAdminEmail!, superAdmin.RoleId) : null;
        }
    }
}
