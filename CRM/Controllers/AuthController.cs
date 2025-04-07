using CRM.Application.DTO;
using CRM.Application.Interfaces;
using CRM.domain.Model;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CRM.Service.AuthService;
using Serilog;

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
                        Log.Warning("User Already Exist");
                        return BadRequest(new AuthResponseError { Error = "User Already Exist" });
                    }
                    await _authService.Register(entity.Adapt<User>());
                    Log.Information("User Successfully Registered");
                    return Accepted(new AuthResponseSuccess { Message = "User Successfully Registered" });
                }
                Log.Warning("Invalid Request");
                return BadRequest(new AuthResponseError { Error = "Invalid Request" });
            }
            catch (Exception ex)
            {
                Log.Error("Internal Server Error: ", ex);
                return StatusCode(500, new AuthResponseError { Error = " Internal Server Error" });
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
                    if (token == null)
                    {
                        Log.Warning("Invalid Credentials");
                        return Unauthorized(new AuthResponseError { Error = "Invalid Credentials" });
                    }
                    Log.Information("Login Successfully ");
                    Ok(new AuthResponseToken { Message = "Login Successfull", Token = token });
                }
                Log.Warning("Invalid Request");
                return BadRequest(new AuthResponseError { Error = "Invalid Request" });
            }
            catch (Exception ex)
            {
                Log.Error("Internal Server Error: ", ex);
                return StatusCode(500, new AuthResponseError { Error = "Internal Server Error" });
            }
        }


        [HttpPost("forgot-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ForgotPassword([FromBody] string email)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var User = await _authService.GetByEmail(email);
                    if (User is not null)
                    {
                        var otp = _otpService.GenerateOtp(email);
                        _emailService.Email(email, "Reset Password", $"Your <b>Reset Password OTP</b> is \"{otp}\"");
                        Log.Information("OTP Sent successfully");
                        return Ok(new AuthResponseSuccess { Message = "OTP Sent successfully" });
                    }
                    Log.Warning("User Not Found");
                    return NotFound(new AuthResponseError { Error = "User Not Found" });
                }
                Log.Warning("Email Requied for generate Password");
                return BadRequest(new AuthResponseError { Error = "Email Requied for generate Password" });


            }
            catch (Exception ex)
            {
                Log.Error("Internal Server Error " + ex);
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
                    bool otpentity = _otpService.VerifyOtp(entity);
                    if (otpentity)
                    {
                        Log.Information("OTP verified Successfully");
                        Ok(new AuthResponseSuccess { Message = "OTP verified Successfully" });
                    }
                    Log.Warning("Invalid OTP");
                    BadRequest(new AuthResponseError { Error = "Invalid OTP" });
                }
                Log.Warning("Invalid Request");
                return BadRequest(new AuthResponseError { Error = "Invalid Request" });
            }
            catch (Exception ex)
            {
                Log.Error("Internal Server Error" + ex);
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
                    if (result)
                    {
                        Log.Information("Password Reset Successfully");
                        return Ok(new AuthResponseSuccess { Message = "Password Reset Successfully" });
                    }
                    Log.Warning("Failed to Reset the Password");
                    BadRequest(new AuthResponseError { Error = "Failed to Reset the Password" });
                }
                Log.Warning("Invalid Request");
                return BadRequest(new AuthResponseError { Error = "Invalid Request" });
            }
            catch (Exception ex)
            {
                Log.Error("Internal Server Error" + ex);
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
                    if(result)
                    {
                        Log.Information("Password Changed Successfully");
                        return Ok(new AuthResponseSuccess { Message = "Password Changed Successfully" });
                    }
                    Log.Warning("Invalid User");
                    return BadRequest( new AuthResponseError { Error = "Invalid User" });
                }
                Log.Warning("Invalid Request");
                return BadRequest(new AuthResponseError { Error = "Invalid Request" });
            }
            catch (Exception ex)
            {
                Log.Error("Internal Server Error "+ex);
                return StatusCode(500, new AuthResponseError { Error = "Internal Server Error" });
            }
        }
    }
}
