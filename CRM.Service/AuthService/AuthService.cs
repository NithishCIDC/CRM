using CRM.Application.DTO;
using CRM.Application.Interfaces;
using CRM.domain.Interface;
using CRM.domain.Model;
using Microsoft.AspNetCore.Http;
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
            bool changePass = await _unitOfwork.User.ChangePassword(entity, _userId);
            await _unitOfwork.Save();
            return changePass;
        }

        public async Task<User?> GetByEmail(string email)
        {
            return await _unitOfwork.User.GetByEmail(email);
        }

        public async Task<string?> Login(LoginDTO entity)
        {
            var user = await _unitOfwork.User.Login(entity);
            var branch = user!=null? await _unitOfwork.User.GetBranch(user.BranchId):null;
            return branch!=null? _tokenService.GenerateToken(user!.Id, user.Email!, branch.OrganizationId, user.Role) : null;
        }

        public async Task Register(User entity)
        {
            entity.Created_At = DateTime.UtcNow.AddHours(5).AddMinutes(30);
            entity.Created_By = _userEmail;
            await _unitOfwork.User.Register(entity);
            await _unitOfwork.Save();
            _emailService.Email(entity.Email!, "Welcome to CRM", "You have successfully registered");
        }

        public async Task<bool> ResetPassword(ResetPasswordDTO entity)
        {
           
            if (entity.NewPassword == entity.ConfirmPassword)
            {
                await _unitOfwork.User.ResetPassword(entity);
                await _unitOfwork.Save();
                return true;
            }
            return false;
        }
    }
}
