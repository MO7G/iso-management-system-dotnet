using System;

namespace iso_management_system.Mappers;


using iso_management_system.Dto.Project;
using iso_management_system.Models;
using iso_management_system.Models.JoinEntities;


public static class ProjectMapper
{
    public static Project ToEntity(ProjectRequestDTO dto)
    {
        return new Project
        {
            CustomerId = dto.CustomerID!.Value,
            StandardID = dto.StandardID!.Value,
            StatusID = 1,       // draft
            StartDate = DateTime.Now,
            CompletionDate = null,
            CreatedAt = DateTime.Now,
            ModifiedAt = DateTime.Now
        };
    }
}

