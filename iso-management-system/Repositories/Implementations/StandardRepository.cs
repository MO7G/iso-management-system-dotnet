using System.Collections.Generic;
using System.Linq;
using iso_management_system.Configurations.Db;
using iso_management_system.Dto.General;
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

        public IEnumerable<Standard> GetAllStandards(int pageNumber, int pageSize, out int totalRecords)
        {
            var query = _context.Standards
                .Include(s => s.Sections)
                .AsNoTracking();

            totalRecords = query.Count();

            return query
                .OrderBy(s => s.StandardID)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }


        public Standard? GetStandardById(int id)
        {
            return _context.Standards
                .Include(s => s.Sections)
                .FirstOrDefault(s => s.StandardID == id);
        }


        public bool IsStandardUsedInAnyProject(int standardId)
        {
            
            bool exists = _context.Standards.Include(s=>s.Projects)
                .Any(s => s.StandardID == standardId && s.Projects.Any());
            
            return exists;
        }

        
        public (bool Exists, bool HasSections, bool HasProjects) GetStandardDeletionStatus(int id)
        {
            // Project the existence and relation flags in a single DB round-trip.
            var result = _context.Standards
                .AsNoTracking()
                .Where(s => s.StandardID == id)
                .Select(s => new
                {
                    HasSections = s.Sections.Any(),
                    HasProjects = s.Projects.Any()
                })
                .FirstOrDefault();

            if (result == null)
                return (Exists: false, HasSections: false, HasProjects: false);

            return (Exists: true, HasSections: result.HasSections, HasProjects: result.HasProjects);
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
        
        
        public IEnumerable<Standard> SearchStandards(
            string? query,
            int pageNumber,
            int pageSize,
            SortingParameters sorting,
            out int totalRecords)
        {
            var baseQuery = _context.Standards
                .Include(s => s.Sections)
                .AsQueryable();

            // Apply search by name here
            if (!string.IsNullOrWhiteSpace(query))
            {
                baseQuery = baseQuery.Where(s => s.Name.Contains(query));
            }

            // Apply dynamic sorting depends on the user now 
            switch (sorting.SortBy?.ToLower())
            {
                case "name":
                    baseQuery = sorting.SortDirection.Equals("desc", StringComparison.CurrentCultureIgnoreCase)
                        ? baseQuery.OrderByDescending(s => s.Name)
                        : baseQuery.OrderBy(s => s.Name);
                    break;

                case "version":
                    baseQuery = sorting.SortDirection.Equals("desc", StringComparison.CurrentCultureIgnoreCase)
                        ? baseQuery.OrderByDescending(s => s.Version)
                        : baseQuery.OrderBy(s => s.Version);
                    break;

                case "publisheddate":
                    baseQuery = sorting.SortDirection.Equals("desc", StringComparison.CurrentCultureIgnoreCase)
                        ? baseQuery.OrderByDescending(s => s.PublishedDate)
                        : baseQuery.OrderBy(s => s.PublishedDate);
                    break;

                default:
                    baseQuery = sorting.SortDirection.Equals("desc", StringComparison.CurrentCultureIgnoreCase)
                        ? baseQuery.OrderByDescending(s => s.StandardID)
                        : baseQuery.OrderBy(s => s.StandardID);
                    break;
            }

            totalRecords = baseQuery.Count();

            return baseQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToList();
        }


        public void DeleteStandardById(int id)
        {
            var standard = new Standard { StandardID = id };
            _context.Standards.Attach(standard);
            _context.Standards.Remove(standard);
            _context.SaveChanges();
        }

        
        public bool StandardExists(int standardId)
        {
            return _context.Standards.Any(s => s.StandardID == standardId);
        }

    }
}