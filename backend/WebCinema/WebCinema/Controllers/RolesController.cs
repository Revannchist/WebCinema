using Microsoft.AspNetCore.Mvc;
using WebCinema.Interfaces;

namespace WebCinema.Controllers
{
    public class RolesController : ControllerBase
    {
        [ApiController]
        [Route("api/[controller]/[action]")]
        public class RoleController : ControllerBase
        {
            private readonly IRolesService _roleService;

            public RoleController(IRolesService roleService)
            {
                _roleService = roleService;
            }

            [HttpGet]
            public async Task<IActionResult> GetRoleById(int id)
            {
                var role = await _roleService.GetRoleByIdAsync(id);
                if (role == null)
                {
                    return BadRequest("Error | Bad Request!");
                }
                return Ok(role);
            }

            [HttpGet]
            public async Task<IActionResult> GetAllRoles()
            {
                var roles = await _roleService.GetAllRolesAsync();
                if (roles == null)
                {
                    return BadRequest("Error | Bad Request!");
                }
                return Ok(roles);
            }
        }
    }
}
