using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iso_management_system.Dto.FileStorage;
using iso_management_system.Exceptions;
using iso_management_system.Models;
using iso_management_system.Models.JoinEntities;
using iso_management_system.Dto.Project;
using iso_management_system.Mappers;
using iso_management_system.models;
using iso_management_system.Repositories.Implementations;
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
    private readonly IStandardSectionRepository _standardSectionRepository;
    private readonly FileStorageService _fileStorageService;
    private readonly IUnitOfWork _unitOfWork;

    public ProjectService(IProjectRepository projectRepository, IStandardTemplateRepository standardTemplateRepository, IProjectDocumentRepository projectDocumentRepository, IStandardRepository standardRepository, ICustomerRepository customerRepository, IDocumentRevisionRepository documentRevisionRepository, IUserRepository userRepository, IRoleRepository roleRepository, IStandardSectionRepository standardSectionRepository, FileStorageService fileStorageService, IUnitOfWork unitOfWork)
    {
        _projectRepository = projectRepository;
        _standardTemplateRepository = standardTemplateRepository;
        _projectDocumentRepository = projectDocumentRepository;
        _standardRepository = standardRepository;
        _customerRepository = customerRepository;
        _documentRevisionRepository = documentRevisionRepository;
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _standardSectionRepository = standardSectionRepository;
        _fileStorageService = fileStorageService;
        _unitOfWork = unitOfWork;
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
        var user = _userRepository.GetUserByIdNotTracked(dto.UserId!.Value);
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

    
    public void UnassignRoleFromProject(ProjectRoleAssignmentDTO dto)
{
    // 1️⃣ Validate project exists
    var project = _projectRepository.GetProjectById(dto.ProjectId!.Value);
    if (project == null)
        throw new NotFoundException($"Project with ID {dto.ProjectId} not found.");

    // 2️⃣ Validate user exists
    var user = _userRepository.GetUserByIdNotTracked(dto.UserId!.Value);
    if (user == null)
        throw new NotFoundException($"User with ID {dto.UserId} not found.");

    // 3️⃣ Validate role exists
    var role = _roleRepository.GetRoleById(dto.RoleId!.Value);
    if (role == null)
        throw new NotFoundException($"Role with ID {dto.RoleId} not found.");

    // 4️⃣ Find the assignment for this user in this project
    var assignment = _projectRepository.GetAssignmentsByProject(dto.ProjectId!.Value)
        .FirstOrDefault(a => a.UserId == dto.UserId);

    if (assignment == null)
        throw new BusinessRuleException($"User {dto.UserId} is not assigned to project {dto.ProjectId}.");

    // 5️⃣ Find the specific role to remove
    var projectRole = _projectRepository.GetProjectRolesByAssignment(assignment.AssignmentId)
        .FirstOrDefault(r => r.RoleId == dto.RoleId);

    if (projectRole == null)
        throw new BusinessRuleException($"User {dto.UserId} does not have role {dto.RoleId} in project {dto.ProjectId}.");

    // 6️⃣ Remove the role
    _projectRepository.RemoveProjectRole(projectRole);

    _projectRepository.SaveChanges();
}

    
    public async Task<FileStorageResponseDTO> UploadFileForCustomer(int standardId, int sectionId, FileUploadCustomerRequestDTO dto)
{
    Console.WriteLine("=== Starting UploadFileForCustomer ===");

    // Print DTO details
    Console.WriteLine("=== DTO ===");
    Console.WriteLine($"CustomerID: {dto.CustomerID}");
    Console.WriteLine($"ProjectDocumentID: {dto.ProjectDocumentID}");
    Console.WriteLine($"ChangeNote: {dto.ChangeNote ?? "(null)"}");
    if (dto.File != null)
    {
        Console.WriteLine($"File Name: {dto.File.FileName}");
        Console.WriteLine($"File Length: {dto.File.Length} bytes");
        Console.WriteLine($"File ContentType: {dto.File.ContentType}");
    }
    else
    {
        Console.WriteLine("File is null!");
    }

    // Check repositories are not null
    Console.WriteLine($"_fileStorageRepository is null? {_fileStorageService == null}");
    Console.WriteLine($"_projectDocumentRepository is null? {_projectDocumentRepository == null}");
    Console.WriteLine($"_documentRevisionRepository is null? {_documentRevisionRepository == null}");
    Console.WriteLine($"_unitOfWork is null? {_unitOfWork == null}");
    Console.WriteLine($"_standardSectionRepository is null? {_standardSectionRepository == null}");
    Console.WriteLine($"_customerRepository is null? {_customerRepository == null}");

    // Validate section
    var section = _standardSectionRepository.GetSectionById(sectionId);
    if (section == null)
    {
        Console.WriteLine("Section not found!");
        throw new NotFoundException("Section not found.");
    }
    if (section.StandardID != standardId)
    {
        Console.WriteLine("Section does not belong to standard!");
        throw new NotFoundException("Section does not belong to the standard.");
    }
    Console.WriteLine("Section validated");

    // Validate customer
    var customer = _customerRepository.GetCustomerById(dto.CustomerID);
    if (customer == null)
    {
        Console.WriteLine("Customer not found!");
        throw new NotFoundException("Customer not found.");
    }
    Console.WriteLine("Customer validated");

    // Validate project document
    var projectDocument = _projectDocumentRepository.GetById(dto.ProjectDocumentID);
    if (projectDocument == null)
    {
        Console.WriteLine("Project document not found!");
        throw new NotFoundException("Project document not found.");
    }
    Console.WriteLine("Project document validated");

    // Begin transaction
    await using var transaction = await _unitOfWork.BeginTransactionAsync();
    Console.WriteLine("Transaction started");

    // Upload file
    if (dto.File == null)
    {
        Console.WriteLine("No file provided!");
        throw new BadRequestException("No file provided.");
    }

    var file = _fileStorageService.UploadCustomerFile(dto); // UploadedByCustomerID set here
    if (file == null)
    {
        Console.WriteLine("File entity returned null from service!");
        throw new Exception("Failed to upload file.");
    }

    await _unitOfWork.SaveChangesAsync();
    Console.WriteLine("1 - File uploaded and saved");

    // Update ProjectDocuments
    var currentVersion = projectDocument.VersionNumber;
    projectDocument.FileID = file.FileID;
    projectDocument.VersionNumber = currentVersion + 1;
    projectDocument.LastModifiedBy = null; // customer upload
    projectDocument.ModifiedAt = DateTime.Now;

    await _unitOfWork.SaveChangesAsync();
    Console.WriteLine("2 - ProjectDocuments updated with new file and version");

    // Insert DocumentRevision
    var revision = new DocumentRevision
    {
        ProjectDocumentID = projectDocument.ProjectDocumentID,
        FileID = file.FileID,
        VersionNumber = projectDocument.VersionNumber,
        ModifiedByUserID = null, // customer upload
        ChangeNote = dto.ChangeNote,
        ModifiedAt = DateTime.Now,
        CreatedAt = DateTime.Now
    };

    _documentRevisionRepository.Add(revision);
    await _unitOfWork.SaveChangesAsync();
    Console.WriteLine("3 - DocumentRevision inserted");

    // Commit transaction
    await _unitOfWork.CommitAsync();
    Console.WriteLine("4 - Transaction committed");

    // Map to DTO
    var response = FileStorageMapper.ToResponseDTO(file);
    Console.WriteLine("5 - Response mapped to DTO");

    Console.WriteLine("=== UploadFileForCustomer Completed ===");
    return response;
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
