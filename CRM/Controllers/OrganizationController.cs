using CRM.Application.DTO;
using CRM.domain.Interface;
using CRM.domain.Model;
using CRM.Service.OrganizationService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace CRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationController : ControllerBase
    {
        private readonly IOrganizationService _organizationservice;

        public OrganizationController(IOrganizationService organizationservice)
        {
            _organizationservice = organizationservice;
        }

        [HttpPost("CreateOrganization")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateOrganization(AddOrganizationDTO entity)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var organization = _organizationservice.GetByEmail(entity.Email!);
                    if (organization is not null)
                    {
                        return BadRequest(new AuthResponseError { Error = "Organization Already Exist" });
                    }
                    else
                    {
                        await _organizationservice.AddOrganization(entity);
                        return Accepted(new AuthResponseSuccess { Message = "Organization Successfully Created" });
                    }

                }
                return BadRequest(new AuthResponseError { Error = "Invalid Request" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new AuthResponseError { Error = ex + " Internal Server Error" });
            }
        }

        [HttpPost("AllOrganization")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AllOrganization()
        {
            try
            {
                var organization = await _organizationservice.GetAll();
                if(organization != null)
                {
                    return Ok(organization);
                }
                return BadRequest(new AuthResponseError { Error = "No Organization Found" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new AuthResponseError { Error = ex + " Internal Server Error" });
            }
        }

        [HttpGet("GetOrganization/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOrganization(Guid id)
        {
            try
            {
                var organization = await _organizationservice.GetById(id);
                if (organization != null)
                {
                    return Ok(organization);
                }
                return BadRequest(new AuthResponseError { Error = "Organization Not Found" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new AuthResponseError { Error = ex + " Internal Server Error" });
            }
        }

        [HttpPut("UpdateOrganization")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateOrganization(UpdateOraganizationDTO entity)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var organization = await _organizationservice.GetById(entity.Id);
                    if (organization is not null)
                    {
                        await _organizationservice.UpdateOrganization(entity);
                        return Accepted(new AuthResponseSuccess { Message = "Organization Successfully Updated" });
                    }
                    return BadRequest(new AuthResponseError { Error = "Organization Not Found" });
                }
                return BadRequest(new AuthResponseError { Error = "Invalid Request" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new AuthResponseError { Error = ex + " Internal Server Error" });
            }
        }

        [HttpDelete("DeleteOrganization")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteOrganization(Guid Id)
        {
            try
            {
                var organization = await _organizationservice.GetById(Id);
                if (organization is not null)
                {
                    await _organizationservice.DeleteOrganization(organization);
                    return Ok(new AuthResponseSuccess { Message = "Organization Successfully Deleted" });
                }
                return BadRequest(new AuthResponseError { Error = "Organization Not Found" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new AuthResponseError { Error = ex + " Internal Server Error" });
            }
        }
    }
}
