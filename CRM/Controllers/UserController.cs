using CRMuser.Application.DTO;
using CRMuser.Application.Interfaces;
using CRMuser.Application.Service;
using CRMUser.domain.Interface;
using CRMUser.domain.Model;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace CRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly IOtpService _otpService;

        public UserController(IUserRepository userRepository, IOtpService otpService, IEmailService emailService)
        {
            _userRepository = userRepository;
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
                    await _userRepository.Register(entity.Adapt<User>());
                    return Accepted(entity);
                }
                return BadRequest(new { Message = "Invalid Request" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { ex.Message });
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
                    var _token = await _userRepository.Login(entity);
                    if (_token is null)
                        return Unauthorized(new { Message = "Invalid Credentials" });
                    return Ok(new { token = _token });
                }
                return BadRequest(new { Message = "Invalid Request" });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { ex.Message });
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("GenerateOtp")]

        public IActionResult GenerateOtp([FromBody] GenerateOtp generateotp)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var otp = _otpService.GenerateOtp(generateotp.Email!);
                    _emailService.Email(generateotp.Email!, "Reset Password", $"Your <b>Reset Password OTP</b> is \"{otp}\"");
                    return Accepted(otp);
                }
                return NotFound(new { message="Email Requied for generate Password"});
            
               
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { ex.Message });
            }
           
        }

        [HttpPost("VerifyOtp")]
        public IActionResult VerifyOtp([FromBody] VerifyOtpDTO verifyOtp)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = _otpService.VerifyOtp(verifyOtp.Otp!);
                    return Ok(result);
                }
                return BadRequest(new { message = "Invalid Request" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { ex.Message });
            }

        }

        [HttpPost("ResetPassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO entity)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var _result = await _userRepository.ResetPassword(entity);
                    if (_result)
                        return Ok(new { Message = "Password Reset Successfully" });
                    return BadRequest(new { Message = "Reset the Password is failed Due to Invalid Credential" });
                }
                return BadRequest(new { Message = "Invalid Request" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { ex.Message });
            }
        }


    }
}
