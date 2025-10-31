using System.Collections.Generic;
using iso_management_system.Models;
using iso_management_system.Models.JoinEntities;
using Microsoft.EntityFrameworkCore.Storage;

namespace iso_management_system.Repositories.Interfaces
{
    public interface IProjectRepository
    {
        Project AddProject(Project project);
        Project? GetProjectById(int projectId);
        void AddProjectDocuments(IEnumerable<ProjectDocuments> documents);
        void SaveChanges();
        
        IEnumerable<ProjectAssignments> GetAssignmentsByProject(int projectId);
        void RemoveProjectRole(ProjectRoles role);

        
        void AddProjectAssignment(ProjectAssignments assignment);
        IEnumerable<ProjectRoles> GetProjectRolesByAssignment(int assignmentId);
        void AddProjectRole(ProjectRoles role);

        
        void DeleteProjectRolesByAssignments(IEnumerable<int> assignmentIds);

        
        void DeleteAssignmentsByProject(int projectId);
        
        
        void DeleteProject(int projectId);

        // Add this for transactions
        IDbContextTransaction BeginTransaction();

    }
}