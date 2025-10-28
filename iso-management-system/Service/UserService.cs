using System;
using System.Collections.Generic;
using System.Linq;
using iso_management_system.Dto.General;
using iso_management_system.Dto.User;
using iso_management_system.DTOs;
using iso_management_system.Exceptions;
using iso_management_system.Helpers;
using iso_management_system.Mappers;
using iso_management_system.models;
using iso_management_system.Models;
using iso_management_system.Repositories.Interfaces;

namespace iso_management_system.Services;

public class UserService
{
    private readonly IUserRepository _userRepository;

    
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    
    
    public PagedResponse<UserResponseDTO> GetAllUsers(int pageNumber, int pageSize)
    {
        var users = _userRepository.GetAllUsers(pageNumber, pageSize, out int totalRecords);

        var dtoList = users.Select(UserMapper.ToResponseDTO);

        return new PagedResponse<UserResponseDTO>(dtoList, totalRecords, pageNumber, pageSize);
    }

    public PagedResponse<UserResponseDTO> SearchUsers(
        string? query,
        int pageNumber,
        int pageSize,
        SortingParameters sorting) // add sorting
    {
        Console.WriteLine($"PageNumber: {pageNumber}");
        Console.WriteLine($"PageSize: {pageSize}");
        Console.WriteLine($"SortBy: {sorting.SortBy}");
        Console.WriteLine($"SortDirection: {sorting.SortDirection}");

        // Fetch users with sorting
        var users = _userRepository.SearchUsers(query, pageNumber, pageSize, sorting, out int totalRecords);
        var dtoList = users.Select(UserMapper.ToResponseDTO).ToList();

        return new PagedResponse<UserResponseDTO>(dtoList, totalRecords, pageNumber, pageSize);
    }

    
    public UserResponseDTO GetUserById(int userId)
    {
        var user = _userRepository.GetUserById(userId);

        if (user == null)
            throw new NotFoundException($"User with ID {userId} not found.");

        return UserMapper.ToResponseDTO(user);
    }

    
    
    public UserResponseDTO CreateUser(UserRequestDTO userRequest)
    {
        // Example business rule: prevent duplicate emails
        bool emailExists = _userRepository.EmailExists(userRequest.Email);
        if (emailExists)
        {
            throw new BusinessRuleException("A user with this email already exists.");
        }

        var user = UserMapper.ToEntity(userRequest);  // DTO → Entity
        _userRepository.AddUser(user);                // Save to DB
        return UserMapper.ToResponseDTO(user);        // Entity → DTO
    }

    
    public UserResponseDTO UpdateUser(int userId, UserUpdateDTO dto)
    {
        var user = _userRepository.GetUserById(userId);
        if (user == null)
            throw new NotFoundException($"User with ID {userId} not found.");

        // Apply changes only if explicitly sent
        if (dto.FirstNameHasValue) user.FirstName = dto.FirstName;
        if (dto.LastNameHasValue) user.LastName = dto.LastName;
        if (dto.EmailHasValue) user.Email = dto.Email;
        if (dto.IsActiveHasValue && dto.IsActive.HasValue) user.IsActive = dto.IsActive.Value;

        user.ModifiedAt = DateTime.Now;

        _userRepository.UpdateUser(user);

        return UserMapper.ToResponseDTO(user);
    }
    
    
    public void DeleteUser(int userId)
    {
        var user = _userRepository.GetUserWithRoles(userId);
        if (user == null)
            throw new NotFoundException($"User with ID {userId} not found.");

        // Business rule: cannot delete admin or active users (example)
        if (user.Roles.Any())
        {
            throw new BusinessRuleException("Cannot delete user assigned to roles.");
        }

        if (user.ProjectAssignments.Any())
        {
            throw new BusinessRuleException("Cann't a user assigned to active project");
        }
        

        _userRepository.DeleteUser(user);
    }
    
    
    
    
    
    
}
 