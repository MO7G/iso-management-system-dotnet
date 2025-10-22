using System.Collections.Generic;
using iso_management_system.Attributes;
using iso_management_system.Constants;
using iso_management_system.Dto.User;
using iso_management_system.DTOs;
using iso_management_system.Helpers;
using iso_management_system.Services;
using iso_management_system.Shared;
using Microsoft.AspNetCore.Mvc;

namespace iso_management_system.Controllers;

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
    
    // get all users
    [HttpGet("users")]
    public ActionResult<ApiResponseWrapper<IEnumerable<UserResponseDTO>>> GetUsers()
    {
        var users = _userService.GetAllUsers();
        return Ok(ApiResponse.Ok(users, "Users fetched successfully"));
    }

    
    
    // get user by id 
    [HttpGet("{userId}")]
    public ActionResult<ApiResponseWrapper<UserResponseDTO>> GetUserById(int userId)
    {
        var user = _userService.GetUserById(userId); // throws NotFoundException if not found
        return Ok(ApiResponse.Ok(user, "User fetched successfully"));
    }

    
    
    // create a user
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

    
    
    // delete a user
    [HttpDelete("delete/{userId}")]
    public ActionResult<ApiResponseWrapper<object>> DeleteUser(int userId)
    {
        _userService.DeleteUser(userId); // throws exceptions if something goes wrong
        return Ok(ApiResponse.Ok<object>(null, "User deleted successfully"));
    }
}