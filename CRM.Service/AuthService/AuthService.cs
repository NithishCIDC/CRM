using CRM.Application.DTO;
using CRM.Application.Interfaces;
using CRM.domain.Interface;
using CRM.domain.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Generators;
using System.Security.Claims;

namespace CRM.Service.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfwork;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly string? _userEmail;
        private readonly Guid _userId;
        public AuthService(IUnitOfWork unitOfwork, ITokenService tokenService, IEmailService emailService, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfwork = unitOfwork;
            _tokenService = tokenService;
            _emailService = emailService;
            _userEmail = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Email)?.Value;
            var userIdString = httpContextAccessor.HttpContext?.User.FindFirst("UserId")?.Value;
            _userId = userIdString != null ? Guid.Parse(userIdString) : Guid.Empty;
        }

        public async Task<bool> ChangePassword(ChangePasswordDTO entity)
        {
            var userData = await _unitOfwork.User.GetById(_userId);
            if (userData is not null && BCrypt.Net.BCrypt.Verify(entity.OldPassword, userData.Password))
            {
                userData.Password = BCrypt.Net.BCrypt.HashPassword(entity.NewPassword);
                _unitOfwork.User.Update(userData);
                await _unitOfwork.Save();
                return true;
            }
            return false;
        }

        public async Task<User?> GetByEmail(string email)
        {
            return await _unitOfwork.User.GetByEmail(email);
        }

        public async Task<string?> Login(LoginDTO entity)
        {
            var user = await _unitOfwork.User.GetByEmail(entity.Email!);
            if (user == null || !BCrypt.Net.BCrypt.Verify(entity.Password, user.Password))
            {
                return null;
            }
            else
            {
                var branch = await _unitOfwork.User.GetBranch(user.BranchId);
                var Permissions = await _unitOfwork.RolePermission.GetPermissionsByRoleId(user.RoleId);
                return branch != null ? _tokenService.GenerateToken(user!.Id, user.Email!, branch.OrganizationId, user.RoleId, Permissions) : null;
            }

        }

        public async Task Register(User entity)
        {
            entity.Created_At = DateTime.UtcNow.AddHours(5).AddMinutes(30);
            entity.Created_By = _userEmail;
            entity.Password = BCrypt.Net.BCrypt.HashPassword(entity.Password);
            await _unitOfwork.User.Add(entity);
            await _unitOfwork.Save();
            _emailService.Email(entity.Email!, "Welcome to CRM", "You have successfully registered");
        }

        public async Task<bool> ResetPassword(ResetPasswordDTO entity)
        {

            if (entity.NewPassword == entity.ConfirmPassword)
            {
                var userdata = await _unitOfwork.User.GetByEmail(entity.email);
                if (userdata is not null)
                {
                    userdata.Password = BCrypt.Net.BCrypt.HashPassword(entity.NewPassword);
                    _unitOfwork.User.Update(userdata);
                }
                await _unitOfwork.Save();
                return true;
            }
            return false;
        }
    }
}
