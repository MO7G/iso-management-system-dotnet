using System.Collections.Generic;
using System.Linq;
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

        public StandardService(IStandardRepository standardRepository)
        {
            _standardRepository = standardRepository;
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
    }
}