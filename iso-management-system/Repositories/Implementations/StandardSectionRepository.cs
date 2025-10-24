// Repositories/Implementations/StandardSectionRepository.cs
using System.Collections.Generic;
using System.Linq;
using iso_management_system.Configurations.Db;
using iso_management_system.Models;
using iso_management_system.Repositories.Interfaces;

namespace iso_management_system.Repositories.Implementations
{
    public class StandardSectionRepository : IStandardSectionRepository
    {
        private readonly AppDbContext _context;

        public StandardSectionRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<StandardSection> GetSectionsByStandard(int standardId)
        {
            return _context.StandardSections
                .Where(s => s.StandardID == standardId)
                .ToList();
        }

        public StandardSection? GetSectionById(int id)
        {
            return _context.StandardSections.FirstOrDefault(s => s.SectionID == id);
        }

        public void AddSection(StandardSection section)
        {
            _context.StandardSections.Add(section);
        }

        public void DeleteSection(StandardSection section)
        {
            _context.StandardSections.Remove(section);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}