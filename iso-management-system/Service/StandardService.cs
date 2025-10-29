using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iso_management_system.Dto.FileStorage;
using iso_management_system.Dto.Stander;
using iso_management_system.DTOs;
using iso_management_system.Exceptions;
using iso_management_system.Mappers;
using iso_management_system.models;
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
        private readonly IUserRepository _userRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public StandardService(IStandardRepository standardRepository, IStandardSectionRepository standardSectionRepository, IStandardTemplateRepository standardTemplateRepository, FileStorageService fileStorageService, IUserRepository userRepository, ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
        {
            _standardRepository = standardRepository;
            _standardSectionRepository = standardSectionRepository;
            _standardTemplateRepository = standardTemplateRepository;
            _fileStorageService = fileStorageService;
            _userRepository = userRepository;
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
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
      public async Task<FileStorageResponseDTO> UploadFileForUser(int standardId, int sectionId, FileUploadRequestDTO dto)
{
    Console.WriteLine($"Starting upload for user {dto.UserID}, section {sectionId}");
    
    var section = _standardSectionRepository.GetSectionById(sectionId);
    if (section == null || section.StandardID != standardId)
        throw new NotFoundException("Section not found or does not belong to the standard.");
    
    Console.WriteLine("Section validated");

    var user = _userRepository.GetUserByIdNotTracked(dto.UserID.Value);
    if (user == null)
        throw new NotFoundException("User not found");

    Console.WriteLine("User validated");

    await using var transaction = await _unitOfWork.BeginTransactionAsync();
    try
    {
        Console.WriteLine("Transaction started");
        
        // Upload file
        var file = _fileStorageService.UploadUserFile(dto);
        Console.WriteLine("1 - File uploaded to service");
        
        // Save file entity
        await _unitOfWork.SaveChangesAsync();
        Console.WriteLine("2 - File saved to database");
        
        // Create template
        var template = new StandardTemplate
        {
            SectionID = sectionId,
            FileID = file.FileID,
            CreatedAt = DateTime.Now,
            ModifiedAt = DateTime.Now
        };
        _standardTemplateRepository.AddTemplate(template);
        Console.WriteLine("3 - Template created");

        await _unitOfWork.SaveChangesAsync();
        Console.WriteLine("4 - Template saved");

        await _unitOfWork.CommitAsync();
        Console.WriteLine("5 - Transaction committed");
        
        // Return mapped DTO, not the entity
        var response = FileStorageMapper.ToResponseDTO(file);
        Console.WriteLine("6 - Response mapped to DTO");
        
        return response;
    }
    catch (Exception ex)
    {
        await _unitOfWork.RollbackAsync();
        Console.WriteLine($"ERROR in transaction: {ex.Message}");
        Console.WriteLine($"Stack trace: {ex.StackTrace}");
        throw;
    }
}

      

        
        
        
    }
}