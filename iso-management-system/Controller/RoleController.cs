using iso_management_system.DTOs;
using iso_management_system.Services;
using Microsoft.AspNetCore.Mvc;

namespace iso_management_system.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoleController : ControllerBase
{
    private readonly RoleService _roleService;
    public RoleController(RoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpGet("roles")]
    public IActionResult GetRoles()
    {
        Console.WriteLine("Hello world");
        var roles = _roleService.getAllRoles();
        return Ok(roles);
    }
    
    
    [HttpGet("{roleId}")]
    public IActionResult GetRolePermissions(int roleId)
    {
        
        Console.WriteLine("Hello world from get role by id !!");
        RoleResponseDTO role = _roleService.GetRoleById(roleId);
        if (role == null)
            return NotFound($"No role found with ID {roleId}");
        //  var permissions = role.RolePermissionMappings.Select(rp => rp.Permission);
        return Ok(role);
    }
    
    
    [HttpPost("create")]
    public IActionResult CreateRole([FromBody] RoleRequestDTO roleRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState); // Returns all validation errors
        }

        var createdRole = _roleService.CreateRole(roleRequest);

        return CreatedAtAction(nameof(GetRolePermissions),
            new { roleId = createdRole.Id },
            createdRole);
    }
    
    
    [HttpDelete("delete/{roleId}")]
    public IActionResult DeleteRole(int roleId)
    {
        try
        {
            _roleService.DeleteRole(roleId);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    
}