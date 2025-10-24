using System.Collections.Generic;
using iso_management_system.Models;
using iso_management_system.Models.JoinEntities;

namespace iso_management_system.Repositories.Interfaces
{
    public interface IDocumentRevisionRepository
    {
        IEnumerable<DocumentRevision> GetByProjectDocument(int projectDocumentId);
        void DeleteRevisions(IEnumerable<DocumentRevision> revisions);
        void SaveChanges();
    }
}