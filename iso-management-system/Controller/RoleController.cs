using iso_management_system.Attributes;
using iso_management_system.DTOs;
using iso_management_system.Services;
using iso_management_system.Shared; // <-- for ApiResponseWrapper
using Microsoft.AspNetCore.Mvc;

namespace iso_management_system.Controllers;

[ApiController]
[ValidateModel]
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
        IEnumerable<RoleResponseDTO> roles = _roleService.getAllRoles();
        var response = new ApiResponseWrapper<IEnumerable<RoleResponseDTO>>(
            200,
            "Roles fetched successfully",
            roles
        );
        return Ok(response);
    }

    [HttpGet("{roleId}")]
    public IActionResult GetRoleByID(int roleId)
    {
        var role = _roleService.GetRoleById(roleId);
        if (role == null)
        {
            var errorResponse = new ApiResponseWrapper<object>(
                404,
                $"No role found with ID {roleId}"
            );
            return NotFound(errorResponse);
        }

        var response = new ApiResponseWrapper<RoleResponseDTO>(
            200,
            "Role fetched successfully",
            role
        );
        return Ok(response);
    }

    [HttpPost("create")]
    public IActionResult CreateRole([FromBody] RoleRequestDTO roleRequest)
    {
        // if (!ModelState.IsValid)
        // {
        //     var errors = ModelState.ToDictionary(
        //         kvp => kvp.Key,
        //         kvp => string.Join(", ", kvp.Value.Errors.Select(e => e.ErrorMessage))
        //     );
        //
        //     var errorResponse = new ApiResponseWrapper<Dictionary<string, string>>(
        //         400,
        //         "Validation failed",
        //         errors
        //     );
        //     return BadRequest(errorResponse);
        // }

        var createdRole = _roleService.CreateRole(roleRequest);
        var response = new ApiResponseWrapper<RoleResponseDTO>(
            201,
            "Role created successfully",
            createdRole
        );

        return CreatedAtAction(nameof(GetRoleByID),
            new { roleId = createdRole.Id },
            response);
    }

    [HttpDelete("delete/{roleId}")]
    public IActionResult DeleteRole(int roleId)
    {
        try
        {
            _roleService.DeleteRole(roleId);
            var response = new ApiResponseWrapper<object>(
                204,
                "Role deleted successfully"
            );
            return NoContent(); // NoContent does not allow a body, so you can skip the wrapper here
        }
        catch (KeyNotFoundException ex)
        {
            var errorResponse = new ApiResponseWrapper<object>(404, ex.Message);
            return NotFound(errorResponse);
        }
        catch (InvalidOperationException ex)
        {
            var errorResponse = new ApiResponseWrapper<object>(400, ex.Message);
            return BadRequest(errorResponse);
        }
    }
}
