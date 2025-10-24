using System;
using iso_management_system.Dto.Status;
using iso_management_system.DTOs;
using iso_management_system.Models;

namespace iso_management_system.Mappers;

public static class StatusMapper
{
    public static StatusResponseDTO ToResponseDTO(ProjectStatus status)
    {
        return new StatusResponseDTO
        {
            StatusId = status.StatusId,
            StatusName = status.StatusName,
            Description = status.Description,
            CreatedAt = status.CreatedAt,
            ModifiedAt = status.ModifiedAt
        };
    }

    public static StatusResponseDTO ToResponseDTO(DocumentStatus status)
    {
        return new StatusResponseDTO
        {
            StatusId = status.StatusID,
            StatusName = status.StatusName,
            Description = status.Description,
            CreatedAt = status.CreatedAt,
            ModifiedAt = status.ModifiedAt
        };
    }

    public static ProjectStatus ToProjectEntity(StatusRequestDTO dto)
    {
        return new ProjectStatus
        {
            StatusName = dto.StatusName,
            Description = dto.Description,
            CreatedAt = DateTime.Now,
            ModifiedAt = DateTime.Now
        };
    }

    public static DocumentStatus ToDocumentEntity(StatusRequestDTO dto)
    {
        return new DocumentStatus
        {
            StatusName = dto.StatusName,
            Description = dto.Description,
            CreatedAt = DateTime.Now,
            ModifiedAt = DateTime.Now
        };
    }
}