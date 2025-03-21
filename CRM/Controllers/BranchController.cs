using CRM.Application.DTO;
using CRM.domain.Model;
using CRM.Service.BranchService;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchController : ControllerBase
    {
        private readonly IBranchService _branchService;
        public BranchController(IBranchService branchService)
        {
            _branchService = branchService;
        }
        [HttpPost("CreateBranch")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateBranch([FromBody] AddBranchDTO branch)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = await _branchService.GetByEmail(branch.Email!);
                    if (response == null)
                    {
                        await _branchService.AddBranch(branch);
                        return Accepted(new AuthResponseSuccess { Message = "Branch is Created"});
                    }
                    return BadRequest(new AuthResponseError { Error = "Branch already exists" });
                }
                return BadRequest(new AuthResponseError { Error = "Invalid Data" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new AuthResponseError { Error = ex + " Internal Server Error" });
            }
        }
        [HttpGet("GetBranch")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBranch()
        {
            try
            {
                var response = await _branchService.GetAll();
                if (response != null)
                {
                    return Ok(response);
                }
                return BadRequest(new AuthResponseError { Error = "No Branch Found" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new AuthResponseError { Error = ex + " Internal Server Error" });
            }
        }
        [HttpGet("GetBranchById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBranchById(Guid id)
        {
            try
            {
                var response = await _branchService.GetById(id);
                if (response != null)
                {
                    return Ok(response);
                }
                return BadRequest(new AuthResponseError { Error = "No Branch Found" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new AuthResponseError { Error = ex + " Internal Server Error" });
            }
        }
        [HttpPut("UpdateBranch")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateBranch([FromBody] Branch branch)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _branchService.UpdateBranch(branch);
                    return Accepted(new AuthResponseSuccess { Message = "Branch is Updated" });
                }
                return BadRequest(new AuthResponseError { Error = "Invalid Data" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new AuthResponseError { Error = ex + " Internal Server Error" });
            }
        }
        [HttpDelete("DeleteBranch")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteBranch(Guid id)
        {
            try
            {
                var response = await _branchService.GetById(id);
                if (response != null)
                {
                    await _branchService.DeleteBranch(response);
                    return Ok(new AuthResponseSuccess { Message = "Branch is Deleted" });
                }
                return BadRequest(new AuthResponseError { Error = "No Branch Found" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new AuthResponseError { Error = ex + " Internal Server Error" });
            }
        }
    }
}
