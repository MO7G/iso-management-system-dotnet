using System;
using System.Collections.Generic;
using System.Linq;
using iso_management_system.Models;
using iso_management_system.Repositories.Interfaces;
using iso_management_system.Configurations.Db; // Your AppDbContext

namespace iso_management_system.Repositories.Implementations
{
    public class StandardTemplateRepository : IStandardTemplateRepository
    {
        private readonly AppDbContext _context;

        public StandardTemplateRepository(AppDbContext context)
        {
            _context = context;
            Console.WriteLine($"UserRepository DbContext Hash: {_context.GetHashCode()}");

        }

        public void AddTemplate(StandardTemplate template)
        {
            _context.StandardTemplates.Add(template);
        }

        public StandardTemplate? GetTemplateById(int templateId)
        {
            return _context.StandardTemplates.FirstOrDefault(t => t.TemplateID == templateId);
        }

        public IEnumerable<StandardTemplate> GetTemplatesBySection(int sectionId)
        {
            return _context.StandardTemplates.Where(t => t.SectionID == sectionId).ToList();
        }
        
        public IEnumerable<StandardTemplate> GetTemplatesByStandard(int standardId)
        {
            return _context.StandardTemplates
                .Where(t => t.Section.StandardID == standardId) // join via navigation property
                .ToList();
        }
        

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}