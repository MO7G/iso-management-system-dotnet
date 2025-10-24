using System;
using System.Collections.Generic;
using System.Linq;
using iso_management_system.Exceptions;
using iso_management_system.Models;
using iso_management_system.Models.JoinEntities;
using iso_management_system.Dto.Project;
using iso_management_system.Mappers;
using iso_management_system.models;
using iso_management_system.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace iso_management_system.Services;

public class ProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly IStandardTemplateRepository _standardTemplateRepository;
    private readonly IProjectDocumentRepository _projectDocumentRepository;
    private readonly IStandardRepository _standardRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IDocumentRevisionRepository _documentRevisionRepository;
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    public ProjectService(
        IProjectRepository projectRepository,
        IStandardTemplateRepository standardTemplateRepository,
        IProjectDocumentRepository projectDocumentRepository,
        IStandardRepository standardRepository,
        ICustomerRepository customerRepository,
        IDocumentRevisionRepository documentRevisionRepository,
        IUserRepository userRepository,
        IRoleRepository roleRepository)
    {
        _projectRepository = projectRepository;
        _standardTemplateRepository = standardTemplateRepository;
        _projectDocumentRepository = projectDocumentRepository;
        _standardRepository = standardRepository;
        _customerRepository = customerRepository;
        _documentRevisionRepository = documentRevisionRepository;
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        
    }

    public int CreateProject(ProjectRequestDTO dto)
    {
        // 1️⃣ Validate
        if (!_customerRepository.CustomerExists(dto.CustomerID!.Value))
            throw new NotFoundException($"Customer with ID {dto.CustomerID} not found.");

        if (!_standardRepository.StandardExists(dto.StandardID!.Value))
            throw new NotFoundException($"Standard with ID {dto.StandardID} not found.");

        // 2️⃣ Begin transaction
        using var transaction = _projectRepository.BeginTransaction();
        try
        {
            // 3️⃣ Map DTO → Project entity
            var project = ProjectMapper.ToEntity(dto);

            _projectRepository.AddProject(project);
            _projectRepository.SaveChanges(); // to generate ProjectID

            // 4️⃣ Map standard templates → ProjectDocuments
            var templates = _standardTemplateRepository.GetTemplatesByStandard(dto.StandardID.Value);

            var projectDocuments = templates
                .Select(t => ProjectDocumentsMapper.FromTemplate(project.ProjectId, t))
                .ToList();

            _projectDocumentRepository.AddProjectDocuments(projectDocuments);
            _projectDocumentRepository.SaveChanges();

            // 5️⃣ Commit transaction
            transaction.Commit();

            return project.ProjectId;
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            throw new Exception($"Error creating project: {ex.Message}", ex);
        }
    }
    
    
    public void AssignRoleToProject(ProjectRoleAssignmentDTO dto)
    {
        // 1️⃣ Validate project exists
        var project = _projectRepository.GetProjectById(dto.ProjectId!.Value);
        if (project == null)
            throw new NotFoundException($"Project with ID {dto.ProjectId} not found.");

        // 2️⃣ Validate user exists
        var user = _userRepository.GetUserById(dto.UserId!.Value);
        if (user == null)
            throw new NotFoundException($"User with ID {dto.UserId} not found.");
        
        
        var role = _roleRepository.GetRoleById(dto.RoleId!.Value);
        if (role == null)
            throw new NotFoundException($"Role with ID {dto.RoleId} not found.");


        // 3️⃣ Try to find existing assignment for this user in this project
        var assignment = _projectRepository.GetAssignmentsByProject(dto.ProjectId!.Value)
            .FirstOrDefault(a => a.UserId == dto.UserId);

        // 👉 If not found, create a new assignment (first time this user joins the project)
        if (assignment == null)
        {
            assignment = new ProjectAssignments
            {
                ProjectId = dto.ProjectId!.Value,
                UserId = dto.UserId!.Value,
                AssignedAt = DateTime.Now
            };
            _projectRepository.AddProjectAssignment(assignment);
            _projectRepository.SaveChanges(); // generate AssignmentID
        }

        // 4️⃣ Check if this specific role is already assigned to this user in this project
        var existingRole = _projectRepository.GetProjectRolesByAssignment(assignment.AssignmentId)
            .FirstOrDefault(r => r.RoleId == dto.RoleId);

        if (existingRole != null)
        {
            // 🚫 Same user + same role already exists
            throw new BusinessRuleException(
                $"User {dto.UserId} already has role {dto.RoleId} in project {dto.ProjectId}."
            );
        }

        // 5️⃣ Otherwise, add the new role for this user's assignment
        _projectRepository.AddProjectRole(new ProjectRoles
        {
            AssignmentId = assignment.AssignmentId,
            RoleId = dto.RoleId!.Value,
            CreatedAt = DateTime.Now,
            ModifiedAt = DateTime.Now
        });

        _projectRepository.SaveChanges();
    }

    
    public void DeleteProject(int projectId)
    {
        // 1️⃣ Check if the project exists
        var project = _projectRepository.GetProjectById(projectId);
        if (project == null)
            throw new NotFoundException($"Project with ID {projectId} not found.");

        // 2️⃣ Begin transaction
        using var transaction = _projectRepository.BeginTransaction();
        try
        {
            // -----------------------
            // a) Delete ProjectRoles
            // -----------------------
            var assignments = _projectRepository.GetAssignmentsByProject(projectId);
            var assignmentIds = assignments.Select(a => a.AssignmentId);
            _projectRepository.DeleteProjectRolesByAssignments(assignmentIds);
            
            
            // -----------------------
            // d) Delete ProjectAssignments
            // -----------------------
            _projectRepository.DeleteAssignmentsByProject(projectId);
            

            // -----------------------
            // b) Delete DocumentRevisions
            // -----------------------
            IEnumerable<DocumentRevision> projectDocuments = _documentRevisionRepository.GetByProjectDocument(projectId);
            _documentRevisionRepository.DeleteRevisions(projectDocuments);

                
            // -----------------------
            // c) Delete ProjectDocuments
            // -----------------------
            _projectDocumentRepository.DeleteProjectDocumentsByProject(projectId);

            

            // -----------------------
            // e) Delete the Project itself
            // -----------------------
            _projectRepository.DeleteProject(projectId);

            // 3️⃣ Save changes and commit
            _projectDocumentRepository.SaveChanges();
            _projectRepository.SaveChanges();
            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            throw new Exception($"Error deleting project {projectId}: {ex.Message}", ex);
        }
    }

}
