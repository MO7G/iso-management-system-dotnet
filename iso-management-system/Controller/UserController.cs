using iso_management_system.Attributes;
using iso_management_system.Dto.General;
using iso_management_system.Dto.User;
using iso_management_system.Helpers;
using iso_management_system.ModelBinders;
using iso_management_system.Service;
using iso_management_system.Shared;
using Microsoft.AspNetCore.Mvc;

namespace iso_management_system.Controller;

[ApiController]
[ValidateModel]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }
    
    /// <summary>
    /// Retrieves a paginated list of all users.
    /// </summary>
    /// <param name="pagination">
    /// Pagination parameters (page number and page size) bound via the custom <see cref="PaginationModelBinder"/>.
    /// </param>
    /// <returns>
    /// A standardized API response containing a paginated list of users.
    /// </returns>
    [HttpGet("users")]
    public ActionResult<ApiResponseWrapper<PagedResponse<UserResponseDTO>>> GetUsers(
        [ModelBinder(BinderType = typeof(PaginationModelBinder))] PaginationParameters pagination
    )
    {
        var usersPaged = _userService.GetAllUsers(pagination.PageNumber, pagination.PageSize);
        return Ok(ApiResponse.Ok(usersPaged, "Users fetched successfully"));
    }
    
    
    
    /// <summary>
    /// Searches for users based on a query string, with pagination and sorting support.
    /// </summary>
    /// <param name="query">Optional search text (e.g., part of username or email).</param>
    /// <param name="pagination">Pagination parameters bound via custom model binder.</param>
    /// <param name="sorting">Sorting parameters bound globally (e.g., sort by name ASC/DESC).</param>
    /// <returns>Filtered, sorted, and paginated list of users.</returns>
    [HttpGet("search")]
    public ActionResult<ApiResponseWrapper<PagedResponse<UserResponseDTO>>> SearchUsers(
        string? query,
        [ModelBinder(BinderType = typeof(PaginationModelBinder))] PaginationParameters pagination,
        [FromQuery] SortingParameters sorting)
    {

        var result = _userService.SearchUsers(
            query,
            pagination.PageNumber,
            pagination.PageSize,
            sorting);

        return Ok(ApiResponse.Ok(result, "Users fetched successfully"));
    }

    
    
    
    /// <summary>
    /// Retrieves a single user by their unique identifier.
    /// </summary>
    /// <param name="userId">Unique ID of the user.</param>
    /// <returns>User details if found, otherwise NotFoundException is thrown.</returns>
    [HttpGet("{userId}")]
    public ActionResult<ApiResponseWrapper<UserResponseDTO>> GetUserById(int userId)
    {
        var user = _userService.GetUserById(userId); // throws NotFoundException if not found
        return Ok(ApiResponse.Ok(user, "User fetched successfully"));
    }

    
    
    /// <summary>
    /// Creates a new user in the system.
    /// </summary>
    /// <param name="userRequest">User data required to create a new record.</param>
    /// <returns>Created user data along with location header.</returns>
    [HttpPost("create")]
    public ActionResult<ApiResponseWrapper<UserResponseDTO>> CreateUser([FromBody] UserRequestDTO userRequest)
    {
        var createdUser = _userService.CreateUser(userRequest);
        
        return CreatedAtAction(
            nameof(GetUserById),
            new { userId = createdUser.Id },
            ApiResponse.Created(createdUser, "User created successfully")
        );
    }

    
    
    /// <summary>
    /// Updates an existing user's data.
    /// </summary>
    /// <param name="userId">ID of the user to update.</param>
    /// <param name="dto">Partial update object containing modified fields.</param>
    /// <returns>Updated user details.</returns>
    [HttpPatch("update/{userId}")]
    [HttpPut("users/{userId}")]
    public async Task<ActionResult<ApiResponseWrapper<UserResponseDTO>>> UpdateUser(
        int userId,
        [FromBody] UserUpdateDTO dto)
    {
        var updated = await _userService.UpdateUserAsync(userId, dto);
        return Ok(ApiResponse.Ok(updated, "User updated successfully"));
    }

    
    
    /// <summary>
    /// Permanently deletes a user by their ID.
    /// </summary>
    /// <param name="userId">ID of the user to delete.</param>
    /// <returns>Success message upon successful deletion.</returns>
    [HttpDelete("delete/{userId}")]
    public async Task<ActionResult<ApiResponseWrapper<object>>> DeleteUser(int userId)
    {
        await _userService.DeleteUserAsync(userId); // ✅ Await the operation
        return Ok(ApiResponse.Ok<object>(null, "User deleted successfully"));
    }

}