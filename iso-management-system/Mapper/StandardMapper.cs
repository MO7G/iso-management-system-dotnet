using System;
using iso_management_system.Dto.Stander;
using iso_management_system.DTOs;
using iso_management_system.Models;

namespace iso_management_system.Mappers
{
    public static class StandardMapper
    {
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
    }
}