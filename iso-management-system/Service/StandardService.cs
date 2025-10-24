using System;
using System.Collections.Generic;
using System.Linq;
using iso_management_system.Dto.FileStorage;
using iso_management_system.Dto.Stander;
using iso_management_system.DTOs;
using iso_management_system.Exceptions;
using iso_management_system.Mappers;
using iso_management_system.Models;
using iso_management_system.Repositories.Interfaces;

namespace iso_management_system.Services
{
    public class StandardService
    {
        
        private readonly IStandardRepository _standardRepository;
        private readonly IStandardSectionRepository _standardSectionRepository;
        private readonly IStandardTemplateRepository _standardTemplateRepository;
        private readonly FileStorageService _fileStorageService;

        public StandardService(
            IStandardRepository standardRepository,
            IStandardSectionRepository standardSectionRepository,
            FileStorageService fileStorageService,
            IStandardTemplateRepository standardTemplateRepository)
        {
            _standardRepository = standardRepository;
            _standardSectionRepository = standardSectionRepository;
            _fileStorageService = fileStorageService;
            _standardTemplateRepository = standardTemplateRepository;
        }

        public IEnumerable<StandardResponseDTO> GetAllStandards()
        {
            var standards = _standardRepository.GetAllStandards();
            return standards.Select(StandardMapper.ToResponseDTO);
        }

        public StandardResponseDTO GetStandardById(int id)
        {
            var standard = _standardRepository.GetStandardById(id);
            if (standard == null)
                throw new NotFoundException($"Standard with ID {id} not found.");

            return StandardMapper.ToResponseDTO(standard);
        }

        public StandardResponseDTO CreateStandard(StandardRequestDTO dto)
        {
            if (_standardRepository.StandardNameExists(dto.Name))
                throw new BusinessRuleException("A standard with this name already exists.");

            var standard = StandardMapper.ToEntity(dto);
            _standardRepository.AddStandard(standard);
            return StandardMapper.ToResponseDTO(standard);
        }

        public void DeleteStandard(int id)
        {
            var standard = _standardRepository.GetStandardById(id);
            if (standard == null)
                throw new NotFoundException($"Standard with ID {id} not found.");

            _standardRepository.DeleteStandard(standard);
        }
        
        
        
        
        
        
        
        public IEnumerable<StandardSectionResponseDTO> GetSectionsByStandard(int standardId)
        {
            var sections = _standardSectionRepository.GetSectionsByStandard(standardId);
            return sections
                .Where(s => s.ParentSectionID == null)
                .Select(BuildSectionHierarchy)
                .ToList();
        }
        
        
        
        
        public StandardSectionResponseDTO CreateSection(StandardSectionRequestDTO dto)
        {
            var standard = _standardRepository.GetStandardById(dto.StandardID);
            if (standard == null)
                throw new NotFoundException($"Standard with ID {dto.StandardID} not found.");

            
            var section = StandardMapper.ToSectionEntity(dto);
            _standardSectionRepository.AddSection(section);
            _standardSectionRepository.SaveChanges();
            return StandardMapper.ToSectionResponseDTO(section);
            
        }
        
        private StandardSectionResponseDTO BuildSectionHierarchy(StandardSection section)
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
                Children = section.ChildSections?.Select(BuildSectionHierarchy).ToList() ?? new()
            };
        }
        
        
        
        
        
        
        // -----------------------------
// File uploads + store in StandardTemplate
// -----------------------------
        public FileStorageResponseDTO UploadFileForUser(int standardId, int sectionId, FileUploadRequestDTO dto)
        {
            var section = _standardSectionRepository.GetSectionById(sectionId);
            if (section == null || section.StandardID != standardId)
                throw new NotFoundException("Section not found or does not belong to the standard.");

            // Upload file
            var file = _fileStorageService.UploadUserFile(dto);

            // Create StandardTemplate linking this file to the section
            var template = new StandardTemplate
            {
                SectionID = sectionId,
                FileID = file.FileID,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            };

            _standardTemplateRepository.AddTemplate(template);
            _standardTemplateRepository.SaveChanges();

            return file;
        }

        public FileStorageResponseDTO UploadFileForCustomer(int standardId, int sectionId, FileUploadRequestDTO dto)
        {
            var section = _standardSectionRepository.GetSectionById(sectionId);
            if (section == null || section.StandardID != standardId)
                throw new NotFoundException("Section not found or does not belong to the standard.");

            // Upload file
            var file = _fileStorageService.UploadCustomerFile(dto);

            // Create StandardTemplate linking this file to the section
            var template = new StandardTemplate
            {
                SectionID = sectionId,
                FileID = file.FileID,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            };

            _standardTemplateRepository.AddTemplate(template);
            _standardTemplateRepository.SaveChanges();

            return file;
        }

        
        
        
    }
}