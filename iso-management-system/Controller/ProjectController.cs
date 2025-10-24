using iso_management_system.Attributes;
using iso_management_system.Dto.Project;
using iso_management_system.Helpers;
using Microsoft.AspNetCore.Mvc;
using iso_management_system.Services;
using iso_management_system.Shared;

namespace iso_management_system.Controllers
{
    [ApiController]
    [ValidateModel]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly ProjectService _projectService;

        public ProjectController(ProjectService projectService)
        {
            _projectService = projectService;
        }

        
        [HttpPost("create")]
        public ActionResult<ApiResponseWrapper<object>> CreateProject([FromBody] ProjectRequestDTO dto)
        {
            var projectId = _projectService.CreateProject(dto);
            return Ok(ApiResponse.Ok(new { ProjectID = projectId }, "Project created successfully"));
        }

        
        [HttpDelete("delete/{projectId}")]
        public ActionResult<ApiResponseWrapper<object>> DeleteProject(int projectId)
        {
            // Call the service layer to handle deletion and validations
            _projectService.DeleteProject(projectId);

            return Ok(ApiResponse.Ok<object>(null, $"Project with ID {projectId} deleted successfully"));
        }
        
        
        [HttpPost("assign-role")]
        public ActionResult<ApiResponseWrapper<object>> AssignRole([FromBody] ProjectRoleAssignmentDTO dto)
        {
            _projectService.AssignRoleToProject(dto);
            return Ok(ApiResponse.Ok<object>(null, "Role assigned successfully to user for the project"));
        }

        

        
    }
}