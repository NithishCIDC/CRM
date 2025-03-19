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
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly IOtpService _otpService;
        private readonly string _userEmail;
        public AuthService(IUserRepository userRepository, ITokenService tokenService, IEmailService emailService, IOtpService otpService,IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _emailService = emailService;
            _otpService = otpService;
            _userEmail = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Email)?.Value!;
        }

        public async Task<bool> ChangePassword(ChangePasswordDTO entity)
        {
            return await _userRepository.ChangePassword(entity, _userEmail);
        }

        public async Task<User?> GetByEmail(string email)
        {
            return await _userRepository.GetByEmail(email);
        }

        public async Task<string?> Login(LoginDTO entity)
        {
            var user = await _userRepository.Login(entity);
            return user != null ? _tokenService.GenerateToken(user.Email!, user.FirstName!) : null;
        }

        public async Task Register(User entity)
        {
            entity.Created_At = DateTime.UtcNow.AddHours(5).AddMinutes(30);
            entity.Created_By = _userEmail;
            await _userRepository.Register(entity);
            _emailService.Email(entity.Email!, "Welcome to CRM", "You have successfully registered");
        }

        public async Task<bool> ResetPassword(ResetPasswordDTO entity)
        {
            var email = _otpService.GetVerifiedEmail();
            var resetdata = await GetByEmail(email);
            if (resetdata is not null && entity.NewPassword == entity.ConfirmPassword)
            {
                resetdata.Password = entity.NewPassword;
                await _userRepository.ResetPassword(resetdata);
                return true;
            }
            return false;
        }
    }
}
