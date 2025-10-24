using System;
using iso_management_system.Models;
using iso_management_system.Models.JoinEntities;

namespace iso_management_system.Mappers;


public static class ProjectDocumentsMapper
{
    public static ProjectDocuments FromTemplate(int projectId, StandardTemplate template)
    {
        return new ProjectDocuments
        {
            ProjectId = projectId,
            TemplateID = template.TemplateID,
            FileID = template.FileID,
            StatusID = 1, // draft
            VersionNumber = 1, // initial version
            CreatedAt = DateTime.Now,
            ModifiedAt = DateTime.Now
        };
    }
}