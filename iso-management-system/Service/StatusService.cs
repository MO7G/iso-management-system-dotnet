using System.Collections.Generic;
using System.Linq;
using iso_management_system.Dto.Status;
using iso_management_system.DTOs;
using iso_management_system.Exceptions;
using iso_management_system.Mappers;
using iso_management_system.Models;
using iso_management_system.Repositories.Interfaces;

namespace iso_management_system.Services;

public class StatusService
{
    private readonly IStatusRepository _statusRepository;

    public StatusService(IStatusRepository statusRepository)
    {
        _statusRepository = statusRepository;
    }

  
    public IEnumerable<StatusResponseDTO> GetAllProjectStatuses()
    {
        var statuses = _statusRepository.GetAllProjectStatuses();
        return statuses.Select(StatusMapper.ToResponseDTO);
    }

    public StatusResponseDTO CreateProjectStatus(StatusRequestDTO dto)
    {
        if (_statusRepository.ProjectStatusNameExists(dto.StatusName))
            throw new BusinessRuleException("A project status with this name already exists.");

        var status = StatusMapper.ToProjectEntity(dto);
        _statusRepository.AddProjectStatus(status);
        return StatusMapper.ToResponseDTO(status);
    }

    public void DeleteProjectStatus(int id)
    {
        var status = _statusRepository.GetProjectStatusById(id);
        if (status == null)
            throw new NotFoundException($"Project status with ID {id} not found.");

        _statusRepository.DeleteProjectStatus(status);
    }

   
    
    public IEnumerable<StatusResponseDTO> GetAllDocumentStatuses()
    {
        var statuses = _statusRepository.GetAllDocumentStatuses();
        return statuses.Select(StatusMapper.ToResponseDTO);
    }

    public StatusResponseDTO CreateDocumentStatus(StatusRequestDTO dto)
    {
        if (_statusRepository.DocumentStatusNameExists(dto.StatusName))
            throw new BusinessRuleException("A document status with this name already exists.");

        var status = StatusMapper.ToDocumentEntity(dto);
        _statusRepository.AddDocumentStatus(status);
        return StatusMapper.ToResponseDTO(status);
    }
    
    public void DeleteDocumentStatus(int id)
    {
        var status = _statusRepository.GetDocumentStatusById(id);
        if (status == null)
            throw new NotFoundException($"Document status with ID {id} not found.");

        _statusRepository.DeleteDocumentStatus(status);
    }

}
