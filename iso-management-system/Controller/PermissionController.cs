using System.Collections.Generic;
using iso_management_system.Attributes;
using iso_management_system.Dto.Permission;
using iso_management_system.DTOs;
using iso_management_system.Helpers;
using iso_management_system.Services;
using iso_management_system.Shared;
using Microsoft.AspNetCore.Mvc;

namespace iso_management_system.Controllers;

[ApiController]
[ValidateModel]
[Route("api/[controller]")]
public class PermissionController : ControllerBase
{
    private readonly PermissionService _permissionService;

    public PermissionController(PermissionService permissionService)
    {
        _permissionService = permissionService;
    }

    // Get all permissions
    [HttpGet("permissions")]
    public ActionResult<ApiResponseWrapper<IEnumerable<PermissionResponseDTO>>> GetPermissions()
    {
        var permissions = _permissionService.GetAllPermissions();
        return Ok(ApiResponse.Ok(permissions, "Permissions fetched successfully"));
    }

    // Get permission by ID
    [HttpGet("{permissionId}")]
    public ActionResult<ApiResponseWrapper<PermissionResponseDTO>> GetPermissionById(int permissionId)
    {
        var permission = _permissionService.GetPermissionById(permissionId);
        return Ok(ApiResponse.Ok(permission, "Permission fetched successfully"));
    }

    // Create a new permission
    [HttpPost("create")]
    public ActionResult<ApiResponseWrapper<PermissionResponseDTO>> CreatePermission([FromBody] PermissionRequestDTO dto)
    {
        var created = _permissionService.CreatePermission(dto);
        return CreatedAtAction(
            nameof(GetPermissionById),
            new { permissionId = created.PermissionID },
            ApiResponse.Created(created, "Permission created successfully")
        );
    }

    // Delete a permission
    [HttpDelete("delete/{permissionId}")]
    public ActionResult<ApiResponseWrapper<object>> DeletePermission(int permissionId)
    {
        _permissionService.DeletePermission(permissionId);
        return Ok(ApiResponse.Ok<object>(null, "Permission deleted successfully"));
    }
}
