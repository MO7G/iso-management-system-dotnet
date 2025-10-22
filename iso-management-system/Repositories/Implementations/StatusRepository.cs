using System.Collections.Generic;
using System.Linq;
using iso_management_system.Configurations.Db;
using iso_management_system.Models;
using iso_management_system.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace iso_management_system.Repositories.Implementations;

public class StatusRepository : IStatusRepository
{
    private readonly AppDbContext _context;

    public StatusRepository(AppDbContext context)
    {
        _context = context;
    }

    // ========================
    // 🔹 PROJECT STATUS
    // ========================

    public IEnumerable<ProjectStatus> GetAllProjectStatuses()
    {
        return _context.ProjectStatuses.AsNoTracking().ToList();
    }

    public ProjectStatus GetProjectStatusById(int id)
    {
        return _context.ProjectStatuses.FirstOrDefault(s => s.StatusId == id);
    }

    public bool ProjectStatusNameExists(string name)
    {
        return _context.ProjectStatuses.Any(s => s.StatusName.ToLower() == name.ToLower());
    }

    public void AddProjectStatus(ProjectStatus status)
    {
        _context.ProjectStatuses.Add(status);
        _context.SaveChanges();
    }

    public void DeleteProjectStatus(ProjectStatus status)
    {
        _context.ProjectStatuses.Remove(status);
        _context.SaveChanges();
    }

    // ========================
    // 🔹 DOCUMENT STATUS
    // ========================

    public IEnumerable<DocumentStatus> GetAllDocumentStatuses()
    {
        return _context.DocumentStatuses.AsNoTracking().ToList();
    }

    public DocumentStatus GetDocumentStatusById(int id)
    {
        return _context.DocumentStatuses.FirstOrDefault(s => s.StatusID == id);
    }

    public bool DocumentStatusNameExists(string name)
    {
        return _context.DocumentStatuses.Any(s => s.StatusName.ToLower() == name.ToLower());
    }

    public void AddDocumentStatus(DocumentStatus status)
    {
        _context.DocumentStatuses.Add(status);
        _context.SaveChanges();
    }

    public void DeleteDocumentStatus(DocumentStatus status)
    {
        _context.DocumentStatuses.Remove(status);
        _context.SaveChanges();
    }
}
