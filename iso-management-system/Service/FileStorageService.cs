using System;
using System.IO;
using iso_management_system.Dto.FileStorage;
using iso_management_system.DTOs;
using iso_management_system.Exceptions;
using iso_management_system.Mappers;
using iso_management_system.Models;
using iso_management_system.Repositories.Interfaces;

namespace iso_management_system.Services
{
    public class FileStorageService
    {
        private readonly IFileStorageRepository _fileStorageRepository;

        public FileStorageService(IFileStorageRepository fileStorageRepository)
        {
            _fileStorageRepository = fileStorageRepository;

        }

        // Simulated upload for a user
        public FileStorage UploadUserFile(FileUploadRequestDTO dto)
        {
            if (dto.File == null)
                throw new BadRequestException("No file provided.");

            var fileEntity = new FileStorage
            {
                FileName = dto.File.FileName,
                FilePath = $"simulated/path/{Guid.NewGuid()}{Path.GetExtension(dto.File.FileName)}",
                FileSize = dto.File.Length,
                UploadedAt = DateTime.Now,
                UploadedByUserID = dto.UserID.Value,
                UploadedByCustomerID = null
            };

            _fileStorageRepository.Add(fileEntity);
            return fileEntity; // return entity instead of DTO
        }


        // Simulated upload for a customer
        public FileStorageResponseDTO UploadCustomerFile(FileUploadRequestDTO dto)
        {
            if (dto.File == null)
                throw new BadRequestException("No file provided.");

            var fileEntity = new FileStorage
            {
                FileName = dto.File.FileName,
                FilePath = $"simulated/path/{Guid.NewGuid()}{Path.GetExtension(dto.File.FileName)}",
                FileSize = dto.File.Length,
                UploadedAt = DateTime.Now,
                UploadedByUser = null,
                UploadedByCustomerID = dto.CustomerID
            };

            _fileStorageRepository.Add(fileEntity);
            _fileStorageRepository.SaveChanges();

            return FileStorageMapper.ToResponseDTO(fileEntity);
        }
    }
}
