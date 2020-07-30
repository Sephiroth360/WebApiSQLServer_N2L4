using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApiSQLServer_N2L4.Models;

namespace WebApiSQLServer_N2L4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [Route("CrearRol")]
        public async Task<ActionResult> CreateRole(IdentityRole role)
        {
            var roleFind = await _roleManager.FindByNameAsync(role.Name);

            if (roleFind == null)
            {
                var result = await _roleManager.CreateAsync(role);

                if (result.Succeeded) { return Ok(); }
                else { return BadRequest(); }
            }

            return BadRequest("El rol ya existe");
        }

        [Route("AsignarRol")]
        public async Task<ActionResult> AsignarRolUsuario(EditRoleDTO editarRolDTO)
        {
            var usuario = await _userManager.FindByIdAsync(editarRolDTO.UserID);
            await _userManager.AddClaimAsync(usuario, new Claim(ClaimTypes.Role, editarRolDTO.RoleName));
            await _userManager.AddToRoleAsync(usuario, editarRolDTO.RoleName);
            return Ok();
        }

        [Route("RemoverRol")]
        public async Task<ActionResult> RemoverRolUsuario(EditRoleDTO editarRolDTO)
        {
            var usuario = await _userManager.FindByIdAsync(editarRolDTO.UserID);
            await _userManager.RemoveClaimAsync(usuario, new Claim(ClaimTypes.Role, editarRolDTO.RoleName));
            await _userManager.RemoveFromRoleAsync(usuario, editarRolDTO.RoleName);
            return Ok();
        }
    }
}