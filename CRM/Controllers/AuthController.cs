using CRM.Application.DTO;
using CRM.Application.Interfaces;
using CRM.domain.Model;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CRM.Service.AuthService;

namespace CRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IEmailService _emailService;
        private readonly IOtpService _otpService;

        public AuthController(IAuthService authService, IOtpService otpService, IEmailService emailService)
        {
            _authService = authService;
            _otpService = otpService;
            _emailService = emailService;
        }

        [HttpPost("Register")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] RegisterDTO entity)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var User = _authService.GetByEmail(entity.Email!);
                    if (User is not null)
                    {
                        return BadRequest(new AuthResponseError { Error = "User Already Exist" });
                    }
                    await _authService.Register(entity.Adapt<User>());
                    return Accepted(new AuthResponseSuccess { Message = "User Successfully Registered" });
                }
                return BadRequest(new AuthResponseError { Error = "Invalid Request" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new AuthResponseError { Error = ex + " Internal Server Error" });
            }
        }

        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login(LoginDTO entity)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var token = await _authService.Login(entity);
                    return token == null
                        ? Unauthorized(new AuthResponseError { Error = "Invalid Credentials" })
                        : Ok(new AuthResponseToken { Message = "Login Successfull", Token = token});
                }
                return BadRequest(new AuthResponseError { Error = "Invalid Request" });
            }
            catch (Exception)
            {
                return StatusCode(500, new AuthResponseError { Error = "Internal Server Error" });
            }
        }


        [HttpPost("forgot-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ForgotPassword([FromBody] string email)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var User = _authService.GetByEmail(email);
                    if (User is not null)
                    {
                        var otp = _otpService.GenerateOtp(email);
                        _emailService.Email(email, "Reset Password", $"Your <b>Reset Password OTP</b> is \"{otp}\"");
                        return Ok(new AuthResponseSuccess { Message = "OTP Sent successfully" });
                    }
                    return NotFound(new AuthResponseError { Error = "User Not Found" });
                }
                return BadRequest(new AuthResponseError { Error = "Email Requied for generate Password" });


            }
            catch (Exception)
            {
                return StatusCode(500, new AuthResponseError { Error = "Internal server Error" });
            }

        }

        [HttpPost("verify-otp")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult VerifyOtp([FromBody] VerifyOtpDTO entity)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return _otpService.VerifyOtp(entity)
                    ? Ok(new AuthResponseSuccess { Message = "OTP verified Successfully" })
                    : BadRequest(new AuthResponseError { Error = "Invalid OTP" });
                }
                return BadRequest(new AuthResponseError { Error = "Invalid Request" });
            }
            catch (Exception)
            {
                return StatusCode(500, new AuthResponseError { Error = "Internal Server Error" });
            }

        }

        [HttpPost("reset-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO entity)
        {
            try
            {
                if (ModelState.IsValid && _otpService.GetVerifiedEmail(entity.email))
                {
                    var result = await _authService.ResetPassword(entity);
                    return result
                        ? Ok(new AuthResponseSuccess { Message = "Password Reset Successfully" })
                        : BadRequest(new AuthResponseError { Error = "Failed to Reset the Password" });
                }
                return BadRequest(new AuthResponseError { Error = "Invalid Request" });
            }
            catch (Exception)
            {
                return StatusCode(500, new AuthResponseError { Error = "Internal Server Error" });
            }
        }

        [Authorize]
        [HttpPost("ChangePassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO entity)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _authService.ChangePassword(entity);
                    return result
                        ? Ok(new AuthResponseSuccess { Message = "Password Changed Successfully" })
                        : StatusCode(500, new AuthResponseError { Error = "An Error Occurred" });
                }
                return BadRequest(new AuthResponseError { Error = "Invalid Request" });
            }
            catch (Exception)
            {
                return StatusCode(500, new AuthResponseError { Error = "Internal Server Error" });
            }
        }
    }
}
