using _01.FlightBookingSystem.Core.DTO_s.Identity;
using _03.FlightBookingSystem.API.Helper;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _03.FlightBookingSystem.API.Controllers
{
    [Authorize(Roles = "Admin")] // Only Admins can access this controller
    public class RoleController : BaseController
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(IMapper _mapper, RoleManager<IdentityRole> roleManager) : base(_mapper)
        {
            _roleManager = roleManager;
        }

        // Get all roles from the database
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetRoles()
        {
            try
            {
                var roles = await _roleManager.Roles.ToListAsync();
                if (roles == null || roles.Count == 0)
                    return NotFound(new ResponseAPI(404, "No roles found."));

                var rolesDTO = _mapper.Map<List<RoleDTO>>(roles);
                return Ok(rolesDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseAPI(500, $"Internal server error: {ex.Message}"));
            }
        }

        // Add a new role to the system
        [HttpPost("AddRole")]
        public async Task<IActionResult> AddRole(RoleDTO RoleDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResponseAPI(400, "Invalid role data."));

            try
            {
                var role = _mapper.Map<IdentityRole>(RoleDTO);
                var result = await _roleManager.CreateAsync(role);

                if (result.Succeeded)
                    return Ok(new ResponseAPI(200, "Add Role is Done."));

                return BadRequest(new ResponseAPI(400, "Add Role failed.", result.Errors.Select(e => e.Description)));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseAPI(500, $"Internal server error: {ex.Message}"));
            }
        }
    }
}
