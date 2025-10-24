using System.Collections.Generic;
using System.Linq;
using iso_management_system.Configurations.Db;
using iso_management_system.Models;
using iso_management_system.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace iso_management_system.Repositories.Implementations
{
    public class StandardRepository : IStandardRepository
    {
        private readonly AppDbContext _context;

        public StandardRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Standard> GetAllStandards()
        {
            return _context.Standards
                .Include(s => s.Sections)
                .AsNoTracking()
                .ToList();
        }

        public Standard? GetStandardById(int id)
        {
            return _context.Standards
                .Include(s => s.Sections)
                .FirstOrDefault(s => s.StandardID == id);
        }

        public bool StandardNameExists(string name)
        {
            return _context.Standards.Any(s => s.Name.ToLower() == name.ToLower());
        }

        public void AddStandard(Standard standard)
        {
            _context.Standards.Add(standard);
            _context.SaveChanges();
        }

        public void DeleteStandard(Standard standard)
        {
            _context.Standards.Remove(standard);
            _context.SaveChanges();
        }
        
        public bool StandardExists(int standardId)
        {
            return _context.Standards.Any(s => s.StandardID == standardId);
        }

    }
}