using System.Collections.Generic;
using System.Linq;
using iso_management_system.Models.JoinEntities;
using iso_management_system.Configurations.Db;
using iso_management_system.Models;
using iso_management_system.Repositories.Interfaces;

namespace iso_management_system.Repositories.Implementations
{
    public class DocumentRevisionRepository : IDocumentRevisionRepository
    {
        private readonly AppDbContext _context;

        public DocumentRevisionRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<DocumentRevision> GetByProjectDocument(int projectDocumentId)
        {
            return _context.DocumentRevisions
                .Where(r => r.ProjectDocumentID == projectDocumentId)
                .ToList();
        }

        public void DeleteRevisions(IEnumerable<DocumentRevision> revisions)
        {
            if (revisions.Any())
            {
                _context.DocumentRevisions.RemoveRange(revisions);
            }
        }
        
        public void Add(DocumentRevision revision)
        {
            _context.DocumentRevisions.Add(revision);
        }
        

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}