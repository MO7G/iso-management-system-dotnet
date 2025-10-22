using System;
using System.Collections.Generic;
using System.Linq;
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

    
    
    public IEnumerable<UserResponseDTO> GetAllUsers()
    {
        Console.WriteLine("Get all users");

        IEnumerable<User> users = _userRepository.GetAllUsers();
        
        // Use mapper to convert entities to DTOs
        var userDTOs = users.Select(UserMapper.ToResponseDTO);

        return userDTOs;
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
 