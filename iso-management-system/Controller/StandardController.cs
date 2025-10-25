using System.Collections.Generic;
using iso_management_system.Attributes;
using iso_management_system.Dto.FileStorage;
using iso_management_system.Dto.Stander;
using iso_management_system.DTOs;
using iso_management_system.Helpers;
using iso_management_system.Services;
using iso_management_system.Shared;
using Microsoft.AspNetCore.Mvc;



namespace iso_management_system.Controllers
{
    [ApiController]
    [ValidateModel]
    [Route("api/[controller]")]
    public class StandardController : ControllerBase
    {
        private readonly StandardService _standardService;


        public StandardController(StandardService standardService)
        {
            _standardService = standardService;
        }

        
        // get all standards
        [HttpGet]
        public ActionResult<ApiResponseWrapper<IEnumerable<StandardResponseDTO>>> GetStandards()
        {
            var standards = _standardService.GetAllStandards();
            return Ok(ApiResponse.Ok(standards, "Standards fetched successfully"));
        }

        
        
        // get a given standard
        [HttpGet("{id}")]
        public ActionResult<ApiResponseWrapper<StandardResponseDTO>> GetStandardById(int id)
        {
            var standard = _standardService.GetStandardById(id);
            return Ok(ApiResponse.Ok(standard, "Standard fetched successfully"));
        }
        
        

        
        // create a standard
        [HttpPost("create")]
        public ActionResult<ApiResponseWrapper<StandardResponseDTO>> CreateStandard([FromBody] StandardRequestDTO dto)
        {
            var created = _standardService.CreateStandard(dto);
            return CreatedAtAction(nameof(GetStandardById), new { id = created.StandardID },
                ApiResponse.Created(created, "Standard created successfully"));
        }

        
        
        // delete a standard
        [HttpDelete("delete/{id}")]
        public ActionResult<ApiResponseWrapper<object>> DeleteStandard(int id)
        {
            _standardService.DeleteStandard(id);
            return Ok(ApiResponse.Ok<object>(null, "Standard deleted successfully"));
        }
        
        
        // create section in standard
        [HttpPost("section/create")]
        public ActionResult<ApiResponseWrapper<StandardSectionResponseDTO>> CreateSection([FromBody] StandardSectionRequestDTO dto)
        {
            var created = _standardService.CreateSection(dto);
            return CreatedAtAction(nameof(GetSectionsByStandard), new { standardId = dto.StandardID },
                ApiResponse.Created(created, "Section created successfully"));
        }
        
        
        // get a section in a standard
        [HttpGet("section/{standardId}")]
        public ActionResult<ApiResponseWrapper<IEnumerable<StandardSectionResponseDTO>>> GetSectionsByStandard(int standardId)
        {
            var sections = _standardService.GetSectionsByStandard(standardId);
            return Ok(ApiResponse.Ok(sections, "Sections fetched successfully"));
        }
        
        
        // -----------------------------
        // Upload file as a User
        // -----------------------------
        [HttpPost("{standardId}/sections/{sectionId}/upload/user")]
        public async Task<ActionResult<ApiResponseWrapper<FileStorageResponseDTO>>> UploadFileForUser(
            int standardId,
            int sectionId,
            [FromForm] FileUploadRequestDTO dto)
        {
            FileStorageResponseDTO result = await _standardService.UploadFileForUser(standardId, sectionId, dto);
            return CreatedAtAction(nameof(UploadFileForUser),
                ApiResponse.Created(result, "File uploaded for user successfully"));
        }

        // -----------------------------
        // Upload file as a Customer
        // -----------------------------
        [HttpPost("{standardId}/sections/{sectionId}/upload/customer")]
        public ActionResult<ApiResponseWrapper<FileStorageResponseDTO>> UploadFileForCustomer(
            int standardId,
            int sectionId,
            [FromForm] FileUploadRequestDTO dto)
        {
            var result = _standardService.UploadFileForCustomer(standardId, sectionId, dto);
            return CreatedAtAction(nameof(UploadFileForCustomer),
                ApiResponse.Created(result, "File uploaded for customer successfully"));
        }
        
    }
}