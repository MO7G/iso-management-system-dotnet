using System;
using iso_management_system.Dto.Stander;
using iso_management_system.DTOs;
using iso_management_system.Models;

namespace iso_management_system.Mappers
{
    public static class StandardMapper
    {
        // -------------------------------
        // Standard mappings
        // -------------------------------
        public static StandardResponseDTO ToResponseDTO(Standard standard)
        {
            return new StandardResponseDTO
            {
                StandardID = standard.StandardID,
                Name = standard.Name,
                Version = standard.Version,
                PublishedDate = standard.PublishedDate,
                CreatedAt = standard.CreatedAt,
                ModifiedAt = standard.ModifiedAt
            };
        }

        public static Standard ToEntity(StandardRequestDTO dto)
        {
            return new Standard
            {
                Name = dto.Name,
                Version = dto.Version,
                PublishedDate = dto.PublishedDate,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            };
        }

        // -------------------------------
        // StandardSection mappings
        // -------------------------------
        public static StandardSectionResponseDTO ToSectionResponseDTO(StandardSection section)
        {
            return new StandardSectionResponseDTO
            {
                SectionID = section.SectionID,
                StandardID = section.StandardID,
                ParentSectionID = section.ParentSectionID,
                Number = section.Number,
                Title = section.Title,
                OrderIndex = section.OrderIndex,
                CreatedAt = section.CreatedAt,
            };
        }

        public static StandardSection ToSectionEntity(StandardSectionRequestDTO dto)
        {
            return new StandardSection
            {
                StandardID = dto.StandardID,
                ParentSectionID = dto.ParentSectionID,
                Number = dto.Number,
                Title = dto.Title,
                OrderIndex = dto.OrderIndex,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            };
        }
    }
}
