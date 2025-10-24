using System.Collections.Generic;
using iso_management_system.Models.JoinEntities;

namespace iso_management_system.Repositories.Interfaces
{
    public interface IProjectDocumentRepository
    {
        // Add documents
        void AddProjectDocuments(IEnumerable<ProjectDocuments> documents);

        // Get documents by project
        IEnumerable<ProjectDocuments> GetByProject(int projectId);

        // Delete related document revisions
        void DeleteDocumentRevisions(int projectDocumentId);

        
        void DeleteProjectDocumentsByProject(int projectId);

        
        // Save changes
        void SaveChanges();
    }
}