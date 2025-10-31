using System.Collections.Generic;
using System.Linq;
using iso_management_system.Models;
using iso_management_system.Repositories.Interfaces;
using iso_management_system.Configurations.Db;
using iso_management_system.Models.JoinEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace iso_management_system.Repositories.Implementations
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly AppDbContext _context;

        public ProjectRepository(AppDbContext context)
        {
            _context = context;
        }

        public Project AddProject(Project project)
        {
            _context.Projects.Add(project);
            return project;
        }

        public Project? GetProjectById(int projectId)
        {
            return _context.Projects.FirstOrDefault(p => p.ProjectId == projectId);
        }
        public void RemoveProjectRole(ProjectRoles role)
        {
            _context.ProjectRoles.Remove(role);
            _context.SaveChanges(); // optionally save immediately
        }

        public void AddProjectDocuments(IEnumerable<ProjectDocuments> documents)
        {
            _context.ProjectDocuments.AddRange(documents);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
        
        public void AddProjectAssignment(ProjectAssignments assignment)
        {
            _context.ProjectAssignments.Add(assignment);
        }

        public IEnumerable<ProjectRoles> GetProjectRolesByAssignment(int assignmentId)
        {
            return _context.ProjectRoles.Where(r => r.AssignmentId == assignmentId).ToList();
        }

        public void AddProjectRole(ProjectRoles role)
        {
            _context.ProjectRoles.Add(role);
        }

        
        
        // ✅ Fetch project assignments
        public IEnumerable<ProjectAssignments> GetAssignmentsByProject(int projectId)
        {
            var project = _context.Projects
                .Include(p => p.ProjectAssignments) // eager load
                .FirstOrDefault(p => p.ProjectId == projectId);

            if (project == null)
                return new List<ProjectAssignments>();

            return project.ProjectAssignments;
        }
        
        
        // In ProjectRepository
        public void DeleteProjectRolesByAssignments(IEnumerable<int> assignmentIds)
        {
            var roles = _context.ProjectRoles
                .Where(r => assignmentIds.Contains(r.AssignmentId))
                .ToList();

            _context.ProjectRoles.RemoveRange(roles);
            _context.SaveChanges();
        }

        
        public void DeleteAssignmentsByProject(int projectId)
        {
            // 1️⃣ Fetch assignments
            var assignments = _context.ProjectAssignments
                .Where(a => a.ProjectId == projectId)
                .ToList();

            if (assignments.Any())
            {
                // 2️⃣ Delete all roles linked to these assignments (just in case)
                var assignmentIds = assignments.Select(a => a.AssignmentId);
                var roles = _context.ProjectRoles
                    .Where(r => assignmentIds.Contains(r.AssignmentId))
                    .ToList();
                if (roles.Any())
                {
                    _context.ProjectRoles.RemoveRange(roles);
                }

                // 3️⃣ Delete assignments
                _context.ProjectAssignments.RemoveRange(assignments);

                // 4️⃣ Save changes
                _context.SaveChanges();
            }
        }

        public void DeleteProject(int projectId)
        {
            var project = _context.Projects.FirstOrDefault(p => p.ProjectId == projectId);
            if (project != null)
            {
                _context.Projects.Remove(project);
            }
        }
        
        public IDbContextTransaction BeginTransaction()
        {
            return _context.Database.BeginTransaction();
        }
        
    }
}