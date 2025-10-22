using System.Collections.Generic;
using iso_management_system.Models;

namespace iso_management_system.Repositories.Interfaces;

public interface IStatusRepository
{
    // Project statuses
    IEnumerable<ProjectStatus> GetAllProjectStatuses();
    ProjectStatus GetProjectStatusById(int id);
    bool ProjectStatusNameExists(string name);
    void AddProjectStatus(ProjectStatus status);
    void DeleteProjectStatus(ProjectStatus status);

    // Document statuses
    IEnumerable<DocumentStatus> GetAllDocumentStatuses();
    DocumentStatus GetDocumentStatusById(int id);
    bool DocumentStatusNameExists(string name);
    void AddDocumentStatus(DocumentStatus status);
    void DeleteDocumentStatus(DocumentStatus status);
}