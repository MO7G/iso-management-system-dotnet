using System;
using System.IO;
using iso_management_system.Dto.FileStorage;
using iso_management_system.Dto.Project;
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
        public FileStorage UploadCustomerFile(FileUploadCustomerRequestDTO dto)
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
            //_fileStorageRepository.SaveChanges();
            return fileEntity;
            //return FileStorageMapper.ToResponseDTO(fileEntity);
        }

        public FileStorage getFileById(int fileId)
        {
            return  _fileStorageRepository.GetById(fileId);
        }
        
        public FileStorageResponseDTO DeleteFile(int fileId)
        {
            // 1️⃣ Retrieve the file from the database
            var file = _fileStorageRepository.GetById(fileId);
            if (file == null)
                throw new NotFoundException($"File with ID {fileId} not found.");

            // 2️⃣ (Optional) Delete the actual file from disk or storage service
            // If you’re storing files on disk or cloud, handle physical deletion here.
            // Example for local files:
            // if (File.Exists(file.FilePath))
            // {
            //     File.Delete(file.FilePath);
            // }

            // 3️⃣ Remove the file entity from the database
            _fileStorageRepository.Delete(file);

            // 4️⃣ Save changes to persist deletion
           // _fileStorageRepository.SaveChanges();

            // 5️⃣ Return DTO (optional, useful if you want to confirm deletion info)
            return FileStorageMapper.ToResponseDTO(file);
        }

    }
}
