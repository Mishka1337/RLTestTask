using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RLTestTask.Models;
using RLTestTask.Services;

namespace RLTestTask.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "admin")]
    public class RoleController : Controller
    {

        private IRoleService roleServie;

        public RoleController(IRoleService roleService)
        {
            this.roleServie = roleService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleDto>>> GetAllRoles()
        {
            return Ok(await roleServie.GetAllRoles());
        }
    }
}