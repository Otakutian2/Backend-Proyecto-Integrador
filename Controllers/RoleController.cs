using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using proyecto_backend.Interfaces;
using proyecto_backend.Models;
using proyecto_backend.Schemas;

namespace proyecto_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RoleController : ControllerBase
    {
        public readonly IRole _roleService;

        public RoleController(IRole roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleGet>>> GetRole()
        {
            return Ok((await _roleService.GetAll()).Adapt<List<RoleGet>>());
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<RoleGet>> GetRole(int id)
        {
            var role = await _roleService.GetById(id);

            if (role == null)
            {
                return NotFound("Rol no encontrado");
            }

            var roleGet = role.Adapt<RoleGet>();

            return Ok(roleGet);
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<RoleGet>> CreateRole([FromBody] RolePrincipal role)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newRole = role.Adapt<Role>();

            await _roleService.CreateRole(newRole);

            var getRole = (await _roleService.GetById(newRole.Id)).Adapt<RoleGet>();

            return CreatedAtAction(nameof(GetRole), new { id = getRole.Id }, getRole);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<RoleGet>> UpdateRole(int id, [FromBody] RolePrincipal roleUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var role = await _roleService.GetById(id);

            if (role == null)
            {
                return NotFound("Rol no encontrado");
            }

            role.Name = roleUpdate.Name;

            await _roleService.UpdateRole(role);

            var getRole = (await _roleService.GetById(id)).Adapt<RoleGet>();

            return Ok(getRole);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> DeleteRole(int id)
        {
            var role = await _roleService.GetById(id);

            if (role == null)
            {
                return NotFound("Rol no encontrado");
            }

            await _roleService.DeleteRole(role);

            return NoContent();
        }
    }
}
