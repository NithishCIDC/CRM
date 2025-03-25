using CRM.Application.DTO;
using CRM.Service.SuperAdminService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperAdminController : ControllerBase
    {
        private readonly ISuperAdminService _superAdminService;
        public SuperAdminController(ISuperAdminService superAdminService)
        {
            _superAdminService = superAdminService;
        }
        [HttpPost("SuperAdminLogin")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SuperAdminLogin([FromBody] LoginSuperAdminDTO entity)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = await _superAdminService.SuperAdminLogin(entity);
                    if (response != null)
                    {
                        return Accepted(new AuthResponseToken { Message = "SuperAdmin Login Successful", Token = response });
                    }
                    return BadRequest(new AuthResponseError { Error = "Invalid Email or Password" });
                }
                return BadRequest(new AuthResponseError { Error = "Invalid Data" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new AuthResponseError { Error = ex + " Internal Server Error" });
            }
        }
    }
}
