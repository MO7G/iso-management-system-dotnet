using System.Collections.Generic;
using iso_management_system.Dto.Stander;
using iso_management_system.DTOs;
using iso_management_system.Helpers;
using iso_management_system.Services;
using iso_management_system.Shared;
using Microsoft.AspNetCore.Mvc;

namespace iso_management_system.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StandardController : ControllerBase
    {
        private readonly StandardService _standardService;

        public StandardController(StandardService standardService)
        {
            _standardService = standardService;
        }

        [HttpGet]
        public ActionResult<ApiResponseWrapper<IEnumerable<StandardResponseDTO>>> GetStandards()
        {
            var standards = _standardService.GetAllStandards();
            return Ok(ApiResponse.Ok(standards, "Standards fetched successfully"));
        }

        [HttpGet("{id}")]
        public ActionResult<ApiResponseWrapper<StandardResponseDTO>> GetStandardById(int id)
        {
            var standard = _standardService.GetStandardById(id);
            return Ok(ApiResponse.Ok(standard, "Standard fetched successfully"));
        }

        [HttpPost("create")]
        public ActionResult<ApiResponseWrapper<StandardResponseDTO>> CreateStandard([FromBody] StandardRequestDTO dto)
        {
            var created = _standardService.CreateStandard(dto);
            return CreatedAtAction(nameof(GetStandardById), new { id = created.StandardID },
                ApiResponse.Created(created, "Standard created successfully"));
        }

        [HttpDelete("delete/{id}")]
        public ActionResult<ApiResponseWrapper<object>> DeleteStandard(int id)
        {
            _standardService.DeleteStandard(id);
            return Ok(ApiResponse.Ok<object>(null, "Standard deleted successfully"));
        }
    }
}