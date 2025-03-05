using CRMuser.Application.DTO;
using CRMUser.domain.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChangePasswordController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public ChangePasswordController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

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
                    var _result = await _userRepository.ChangePassword(entity);
                    if (_result)
                        return Ok(new { Message = "Password Changed Successfully" });
                    return BadRequest(new { Message = "Password Change is failesd due to invalid Credentials;" });
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
