using CRM.Application.DTO;
using CRM.Service.MasterAdminService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterAdminController : ControllerBase
    {
        private readonly IMasterAdminService _MasterAdminService;
        public MasterAdminController(IMasterAdminService MasterAdminService)
        {
            _MasterAdminService = MasterAdminService;
        }
        [HttpPost("MasterAdminLogin")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> MasterAdminLogin([FromBody] LoginMasterAdminDTO entity)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = await _MasterAdminService.MasterAdminLogin(entity);
                    if (response != null)
                    {
                        return Accepted(new AuthResponseToken { Message = "MasterAdmin Login Successful", Token = response });
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
