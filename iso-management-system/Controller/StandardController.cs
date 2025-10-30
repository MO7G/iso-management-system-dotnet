using System.Collections.Generic;
using System.Threading.Tasks;
using iso_management_system.Attributes;
using iso_management_system.Dto.FileStorage;
using iso_management_system.Dto.General;
using iso_management_system.Dto.Stander;
using iso_management_system.DTOs;
using iso_management_system.Helpers;
using iso_management_system.ModelBinders;
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
        [HttpGet("standards")]
        public ActionResult<ApiResponseWrapper<PagedResponse<StandardResponseDTO>>> GetStandards(
            [ModelBinder(BinderType = typeof(PaginationModelBinder))] PaginationParameters pagination
            
        )
        {
            var standardsPaged = _standardService.GetAllStandards(pagination.PageNumber, pagination.PageSize);
            return Ok(ApiResponse.Ok(standardsPaged, "Standards fetched successfully"));
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

        
        [HttpGet("search")]
        public ActionResult<ApiResponseWrapper<PagedResponse<StandardResponseDTO>>> SearchStandards(
            string? query,
            [ModelBinder(BinderType = typeof(PaginationModelBinder))] PaginationParameters pagination,
            [FromQuery] SortingParameters sorting)
        {
            var result = _standardService.SearchStandards(
                query,
                pagination.PageNumber,
                pagination.PageSize,
                sorting);

            return Ok(ApiResponse.Ok(result, "Standards fetched successfully"));
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
        [HttpGet("fullsections/{standardId}")]
        public ActionResult<ApiResponseWrapper<IEnumerable<StandardSectionResponseDTO>>> GetSectionsByStandard(int standardId)
        {
            var sections = _standardService.GetSectionsByStandard(standardId);
            return Ok(ApiResponse.Ok(sections, "Sections fetched successfully"));
        }
        
        
        // DELETE: api/standard/sections/{sectionId}
        [HttpDelete("{standardId}/sections/{sectionId}")]
        public async Task<ActionResult<ApiResponseWrapper<object>>> DeleteSection(int standardId, int sectionId)
        {
            await _standardService.DeleteSectionAsync(sectionId);
            return Ok(ApiResponse.Ok<object>(null, "Section deleted successfully"));
        }

        
        // get a single section by ID
        [HttpGet("section/{sectionId}")]
        public ActionResult<ApiResponseWrapper<StandardSectionResponseDTO>> GetSectionById(int sectionId)
        {
            var section = _standardService.GetSectionById(sectionId);
            return Ok(ApiResponse.Ok(section, "Section fetched successfully"));
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

       
        
    }
}