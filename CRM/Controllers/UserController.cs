using CRMuser.Application.DTO;
using CRMUser.domain.Interface;
using CRMUser.domain.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserDTO entity)
        {
            if (ModelState.IsValid)
            {
                await _userRepository.Register(entity);
            }
           
            return Ok(entity);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(User entity)
        {
            await _userRepository.Login(entity);
            return Ok("User Logged in Successfully");
        }
    }
}
