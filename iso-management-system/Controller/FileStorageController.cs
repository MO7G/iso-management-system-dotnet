using iso_management_system.Dto.FileStorage;
using iso_management_system.Helpers;
using iso_management_system.Services;
using iso_management_system.Shared;
using Microsoft.AspNetCore.Mvc;

namespace iso_management_system.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FileStorageController : ControllerBase
{
    private readonly FileStorageService _service;

    public FileStorageController(FileStorageService service)
    {
        _service = service;
    }

    [HttpPost("upload/user")]
    public ActionResult<ApiResponseWrapper<FileStorageResponseDTO>> UploadUserFile([FromForm] FileUploadRequestDTO dto)
    {
        var result = _service.UploadUserFile(dto);
        return CreatedAtAction(nameof(UploadUserFile), ApiResponse.Created(result, "User file metadata saved successfully"));
    }
    
}