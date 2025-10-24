using System.Collections.Generic;
using System.Linq;
using iso_management_system.Models.JoinEntities;
using iso_management_system.Configurations.Db;
using iso_management_system.Repositories.Interfaces;

namespace iso_management_system.Repositories.Implementations
{
    public class ProjectDocumentRepository : IProjectDocumentRepository
    {
        private readonly AppDbContext _context;

        public ProjectDocumentRepository(AppDbContext context)
        {
            _context = context;
        }

        public void AddProjectDocuments(IEnumerable<ProjectDocuments> documents)
        {
            _context.ProjectDocuments.AddRange(documents);
        }

        public IEnumerable<ProjectDocuments> GetByProject(int projectId)
        {
            return _context.ProjectDocuments
                .Where(pd => pd.ProjectId == projectId)
                .ToList();
        }

        public void DeleteDocumentRevisions(int projectDocumentId)
        {
            var revisions = _context.DocumentRevisions
                .Where(r => r.ProjectDocumentID == projectDocumentId)
                .ToList();

            if (revisions.Any())
            {
                _context.DocumentRevisions.RemoveRange(revisions);
            }
        }

        public void DeleteProjectDocumentsByProject(int projectId)
        {
            // 1️⃣ Fetch documents of the project
            var documents = _context.ProjectDocuments
                .Where(pd => pd.ProjectId == projectId)
                .ToList();

            // 2️⃣ Delete related revisions first
            foreach (var doc in documents)
            {
                DeleteDocumentRevisions(doc.ProjectDocumentID);
            }

            // 3️⃣ Delete project documents
            if (documents.Any())
            {
                _context.ProjectDocuments.RemoveRange(documents);
            }
        }
        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}