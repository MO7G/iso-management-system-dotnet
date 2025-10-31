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
            bool IsFileUsedInAnyProject(int fileId , int templateId);

            
            void DeleteProjectDocumentsByProject(int projectId);

            ProjectDocuments? GetById(int projectDocumentId);

            // Save changes
            void SaveChanges();
        }
    }