using System.Collections.Generic;
using iso_management_system.Dto.Status;
using iso_management_system.DTOs;
using iso_management_system.Helpers;
using iso_management_system.Services;
using iso_management_system.Shared;
using Microsoft.AspNetCore.Mvc;

namespace iso_management_system.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatusController : ControllerBase
{
    private readonly StatusService _statusService;

    public StatusController(StatusService statusService)
    {
        _statusService = statusService;
    }

    // ========================
    // 🔹 PROJECT STATUS
    // ========================

    [HttpGet("project")]
    public ActionResult<ApiResponseWrapper<IEnumerable<StatusResponseDTO>>> GetProjectStatuses()
    {
        var statuses = _statusService.GetAllProjectStatuses();
        return Ok(ApiResponse.Ok(statuses, "Project statuses fetched successfully"));
    }

    [HttpPost("project/create")]
    public ActionResult<ApiResponseWrapper<StatusResponseDTO>> CreateProjectStatus([FromBody] StatusRequestDTO dto)
    {
        var status = _statusService.CreateProjectStatus(dto);
        return CreatedAtAction(nameof(GetProjectStatuses), ApiResponse.Created(status, "Project status created successfully"));
    }

    [HttpDelete("project/delete/{id}")]
    public ActionResult<ApiResponseWrapper<object>> DeleteProjectStatus(int id)
    {
        _statusService.DeleteProjectStatus(id);
        return Ok(ApiResponse.Ok<object>(null, "Project status deleted successfully"));
    }

    // ========================
    // 🔹 DOCUMENT STATUS
    // ========================

    [HttpGet("document")]
    public ActionResult<ApiResponseWrapper<IEnumerable<StatusResponseDTO>>> GetDocumentStatuses()
    {
        var statuses = _statusService.GetAllDocumentStatuses();
        return Ok(ApiResponse.Ok(statuses, "Document statuses fetched successfully"));
    }

    [HttpPost("document/create")]
    public ActionResult<ApiResponseWrapper<StatusResponseDTO>> CreateDocumentStatus([FromBody] StatusRequestDTO dto)
    {
        var status = _statusService.CreateDocumentStatus(dto);
        return CreatedAtAction(nameof(GetDocumentStatuses), ApiResponse.Created(status, "Document status created successfully"));
    }

    [HttpDelete("document/delete/{id}")]
    public ActionResult<ApiResponseWrapper<object>> DeleteDocumentStatus(int id)
    {
        _statusService.DeleteDocumentStatus(id);
        return Ok(ApiResponse.Ok<object>(null, "Document status deleted successfully"));
    }
}
