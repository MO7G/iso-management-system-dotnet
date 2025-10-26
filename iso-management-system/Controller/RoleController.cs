using System.Collections.Generic;
using iso_management_system.Attributes;
using iso_management_system.Constants;
using iso_management_system.DTOs;
using iso_management_system.Helpers;
using iso_management_system.Services;
using iso_management_system.Shared;
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
    
    // get all roles
    [HttpGet("roles")]
    public ActionResult<ApiResponseWrapper<IEnumerable<RoleResponseDTO>>> GetRoles()
    {
        var roles = _roleService.getAllRoles();
        return Ok(ApiResponse.Ok(roles, "Roles fetched successfully"));
    }

    
    
    // get role by id 
    [HttpGet("{roleId}")]
    public ActionResult<ApiResponseWrapper<RoleResponseDTO>> GetRoleById(int roleId)
    {
        var role = _roleService.GetRoleById(roleId); // throws NotFoundException if not found
        return Ok(ApiResponse.Ok(role, "Role fetched successfully"));
    }

    
    
    // create a role
    [HttpPost("create")]
    public ActionResult<ApiResponseWrapper<RoleResponseDTO>> CreateRole([FromBody] RoleRequestDTO roleRequest)
    {
        var createdRole = _roleService.CreateRole(roleRequest);
        
        return CreatedAtAction(
            nameof(GetRoleById),
            new { roleId = createdRole.Id },
            ApiResponse.Created(createdRole, "Role created successfully")
        );
    }

    
    
    // delete a role
    [HttpDelete("delete/{roleId}")]
    public ActionResult<ApiResponseWrapper<object>> DeleteRole(int roleId)
    {
        _roleService.DeleteRole(roleId); // throws exceptions if something goes wrong
        return Ok(ApiResponse.Ok<object>(null, "Role deleted successfully"));
    }
    
    // === Add permission to role ===
    [HttpPost("permissions/add")]
    public ActionResult<ApiResponseWrapper<object>> AddPermissionToRole([FromBody] RolePermissionRequestDTO dto)
    {
        _roleService.AddPermissionToRole(dto.RoleId.Value, dto.PermissionId.Value);
        return Ok(ApiResponse.Ok<object>(null, "Permission added to role successfully."));
    }

    // === Remove permission from role ===
    [HttpPost("permissions/remove")]
    public ActionResult<ApiResponseWrapper<object>> RemovePermissionFromRole([FromBody] RolePermissionRequestDTO dto)
    {
        _roleService.RemovePermissionFromRole(dto.RoleId.Value, dto.PermissionId.Value);
        return Ok(ApiResponse.Ok<object>(null, "Permission removed from role successfully."));
    }
}
