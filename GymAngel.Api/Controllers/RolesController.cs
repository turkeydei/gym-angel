using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GymAngel.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace GymAngel.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<Role> _roleManager;
        public RolesController(RoleManager<Role> roleManager) { _roleManager = roleManager; }
        [HttpGet] public IActionResult GetRoles() { return Ok(_roleManager.Roles.ToList()); }
    }
}