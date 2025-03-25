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
                return StatusCode(500, new AuthResponseError { Error = ex+" Internal Server Error" });
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
                        : Ok(new AuthResponseToken { Message = "Login Successfull", Token = token });
                }
                return BadRequest(new AuthResponseError { Error = "Invalid Request" });
            }
            catch (Exception)
            {
                return StatusCode(500, new AuthResponseError { Error = "Internal Server Error" });
            }
        }

        [HttpPost("password-reset")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GenerateOtp([FromBody] GenerateOtp generateotp)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var User = _authService.GetByEmail(generateotp.Email!);
                    if (User is not null)
                    {
                        var otp = _otpService.GenerateOtp(generateotp.Email!);
                        _emailService.Email(generateotp.Email!, "Reset Password", $"Your <b>Reset Password OTP</b> is \"{otp}\"");
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

        [HttpPost("password-reset/verify")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult VerifyOtp([FromBody] VerifyOtpDTO verifyOtp)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = _otpService.VerifyOtp(verifyOtp.Otp!);
                    return Ok(new AuthResponseSuccess { Message = "OTP verified Successfully" });
                }
                return BadRequest(new AuthResponseError { Error = "Invalid Request" });
            }
            catch (Exception)
            {
                return StatusCode(500, new AuthResponseError { Error = "Internal Server Error" });
            }

        }

        [HttpPost("password-reset/new-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO entity)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _authService.ResetPassword(entity);
                    return result
                        ? Ok(new AuthResponseSuccess { Message = "Password Reset Successfully" })
                        : BadRequest(new AuthResponseError { Error = "Reset the Password is failed Due to Invalid Credential" });
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
