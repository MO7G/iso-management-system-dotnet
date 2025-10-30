using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iso_management_system.Dto.FileStorage;
using iso_management_system.Dto.General;
using iso_management_system.Dto.Stander;
using iso_management_system.DTOs;
using iso_management_system.Exceptions;
using iso_management_system.Helpers;
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

        public StandardService(IStandardRepository standardRepository,
            IStandardSectionRepository standardSectionRepository,
            IStandardTemplateRepository standardTemplateRepository, FileStorageService fileStorageService,
            IUserRepository userRepository, ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
        {
            _standardRepository = standardRepository;
            _standardSectionRepository = standardSectionRepository;
            _standardTemplateRepository = standardTemplateRepository;
            _fileStorageService = fileStorageService;
            _userRepository = userRepository;
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
        }

        public PagedResponse<StandardResponseDTO> GetAllStandards(int pageNumber, int pageSize)
        {
            var standards = _standardRepository.GetAllStandards(pageNumber, pageSize, out int totalRecords);

            var dtoList = standards.Select(StandardMapper.ToResponseDTO);

            return new PagedResponse<StandardResponseDTO>(dtoList, totalRecords, pageNumber, pageSize);
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

        public PagedResponse<StandardResponseDTO> SearchStandards(
            string? query,
            int pageNumber,
            int pageSize,
            SortingParameters sorting)
        {
            var standards =
                _standardRepository.SearchStandards(query, pageNumber, pageSize, sorting, out int totalRecords);

            var dtoList = standards.Select(StandardMapper.ToResponseDTO).ToList();

            return new PagedResponse<StandardResponseDTO>(dtoList, totalRecords, pageNumber, pageSize);
        }


        public void DeleteStandard(int id)
        {
            var status = _standardRepository.GetStandardDeletionStatus(id);

            if (!status.Exists)
                throw new NotFoundException($"Standard with ID {id} not found.");

            if (status.HasSections)
                throw new BusinessRuleException("Cannot delete a Standard that has related Sections.");

            if (status.HasProjects)
                throw new BusinessRuleException("Cannot delete a Standard that has related Projects.");

            _standardRepository.DeleteStandardById(id);
        }


        public IEnumerable<StandardSectionResponseDTO> GetSectionsByStandard(int standardId)
        {
            var sections = _standardSectionRepository.GetSectionsByStandard(standardId);
            return sections
                .Where(s => s.ParentSectionID == null) // first we filter here to start with the root sections 
                .Select(BuildSectionHierarchy) // for each root section here we call on it the function that builds the hierarchy !!
                .ToList();


            /*
             *  var sections = _standardSectionRepository.GetSectionsByStandard(standardId);


                  var rootSections = sections
                      .Where(s => s.ParentSectionID == null)
                      .ToList();


                  var result = new List<StandardSectionResponseDTO>();


                  foreach (var root in rootSections)
                  {
                      var sectionDto = BuildSectionHierarchy(root);
                      result.Add(sectionDto);
                  }


                  return result;
             */
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
            // Here, I create an empty list that will hold the child sections of the current section.
            var children = new List<StandardSectionResponseDTO>();

// I check if the current section has any child sections.
            if (section.ChildSections != null && section.ChildSections.Any())
            {
                // If it does, I recursively build each child's hierarchy
                // and add the result to the children list.
                foreach (var child in section.ChildSections)
                {
                    children.Add(BuildSectionHierarchy(child));
                }
            }

// At this point, I construct the current section's DTO.
// If the section has no children, the 'children' list will just be empty.
// This way, the tree builds recursively from the top section down to all nested levels.
            return new StandardSectionResponseDTO
            {
                SectionID = section.SectionID,
                StandardID = section.StandardID,
                ParentSectionID = section.ParentSectionID,
                Number = section.Number,
                Title = section.Title,
                OrderIndex = section.OrderIndex,
                CreatedAt = section.CreatedAt,
                Children = children
            };


            // the same thing can be done like this 
            /*
         * private StandardSectionResponseDTO BuildSectionHierarchy(StandardSection section)
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
         */
        }


        
        public StandardSectionResponseDTO GetSectionById(int sectionId)
        {
            var section = _standardSectionRepository.GetSectionById(sectionId);
            if (section == null)
                throw new NotFoundException($"Section with ID {sectionId} not found.");

            // Map to DTO (you already have a mapper for this)
            var dto = StandardMapper.ToSectionResponseDTO(section);
            return dto;
        }

        public async Task DeleteSectionAsync(int sectionId)
        {
            // === 1️⃣ VALIDATION PHASE (outside the transaction) ===
            var section = _standardSectionRepository.GetSectionById(sectionId);
            if (section == null)
                throw new NotFoundException($"Section with ID {sectionId} not found.");

            if (_standardRepository.IsStandardUsedInAnyProject(section.StandardID))
                throw new BusinessRuleException("Cannot delete... used in a Project.");

            if (_standardSectionRepository.HasChildSections(section.SectionID))
                throw new BusinessRuleException("Cannot delete... has child sections.");

            await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                var templates = _standardTemplateRepository.GetTemplatesBySectionId(section.SectionID).ToList();
               
                foreach (var template in templates)
                {
                    // This deletes the physical file but does NOT SaveChanges
                    _fileStorageService.DeleteFile(template.FileID);

                    
                    // Mark the template for deletion (not yet saved)
                    _standardTemplateRepository.DeleteTemplate(template);
                    
                   
                }

                // Mark the section for deletion
                _standardSectionRepository.DeleteSection(section);
                
                
            });

        }



        public async Task<FileStorageResponseDTO> UploadFileForUser(int standardId, int sectionId,
            FileUploadRequestDTO dto)
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









