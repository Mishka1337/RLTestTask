using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RLTestTask.Exceptions;
using RLTestTask.Models;
using RLTestTask.Services;

namespace RLTestTask.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "admin")]
    public class UserController : Controller
    {
        private IUserService userService;
        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsersOnPage(
            [FromQuery]int page = 0,
            [FromQuery]int num = 10)
        {
            IEnumerable<UserDto> userDtos = await userService.GetAllUsersOnPage(page, num);
            return Ok(userDtos);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserById(int id)
        {
            UserDto userDto = await userService.GetUserById(id);
            return userDto != null 
                ? Ok(userDto)
                : NotFound();
        }

        [HttpGet("role/{roleId}")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsersByRoleId(int roleId,
                                                                                [FromQuery]int page = 0,
                                                                                [FromQuery]int num = 10)
        {
            IEnumerable<UserDto> userDtos = await userService.GetAllUsersByRole(roleId, page, num);
            if (userDtos == null)
            {
                return NotFound();
            }
            return Ok(userDtos);
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> PostUser([FromBody] UserDto userDto)
        {
            if(userDto == null)
            {
                return BadRequest();
            }
            try
            {
                userDto = await userService.CreateUser(userDto);
            } 
            catch (UserServiceException e)
            {
                return BadRequest(new { errorMessage = e.Message });
            }
            return Ok(userDto);
        }

        [HttpPatch]
        public async Task<ActionResult<UserDto>> PatchUser([FromBody] UserDto userDto)
        {
            if (userDto == null)
            {
                return BadRequest();
            }
            try
            {
                userDto = await userService.UpdateUser(userDto);
            }
            catch (UserServiceException e) 
            {
                return BadRequest(new { errorMessage = e.Message });
            }
            return Ok(userDto);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await userService.DeleteUser(id);
            return Ok();
        }
   }
}