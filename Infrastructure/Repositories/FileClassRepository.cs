using Domain.Entities.FileEntity.IRepository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Shared;

namespace Infrastructure.Repositories
{
    public class FileClassRepository : IFileClassRepository
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _config;

        public FileClassRepository(IHostingEnvironment hostingEnvironment, IConfiguration config)
        {
            _hostingEnvironment = hostingEnvironment;
            _config = config;
        }
        public async Task<(CommandExecutionResult result, string filePath)> SaveFileServer(string FilePathForDb, string hostingEnvironmentPath, string folderName, string base64String, string fileName, string ext)
        {
            //var directoryPath =; //"\\Files\\Employees"
            try
            {
                string FullFolderPath = Path.Combine(new DirectoryInfo(_hostingEnvironment.ContentRootPath + hostingEnvironmentPath)?.FullName ?? throw new Exception("_hostingEnvironment fullname is null "), folderName);

                if (ext != "png" && ext != "jpg" && ext != "docx" && ext != "pdf" && ext != "xlsx")
                {
                    return (new CommandExecutionResult { Success = false, ErrorMessage = "File format error" }, null);
                }

                if (!Directory.Exists(FullFolderPath))
                {
                    Directory.CreateDirectory(FullFolderPath);
                }

                string generatedName = Guid.NewGuid().ToString();
                string filePath = Path.Combine(FullFolderPath, $"{fileName}_{generatedName}.{ext}");
                string fileAddres = FilePathForDb + folderName + $"/{fileName}_{generatedName}.{ext}";

                byte[] fileBytes = Convert.FromBase64String(base64String);
                string ApiAddress = _config.GetSection("SendVerifyMailLinks:ApiAddress").Value?.ToString() ?? throw new Exception("ApiAddress  is null ");
                File.WriteAllBytes(filePath, Convert.FromBase64String(base64String));

                return (new CommandExecutionResult { Success = true }, ApiAddress + fileAddres);
            }
            catch (Exception ex)
            {
                return (new CommandExecutionResult { Success = false, ErrorMessage = $"File Write error \n{ex.Message}" }, null);
            }
        }
        public async Task<(CommandExecutionResult result, string filePath)> SaveFileLocal(string filePathForDb, string hostingEnvironmentPath, string folderName, string base64String, string fileName, string ext)
        {
            try
            {
                // Construct full folder path
                string fullFolderPath = Path.Combine(
                    new DirectoryInfo(Path.Combine(_hostingEnvironment.ContentRootPath, hostingEnvironmentPath))?.FullName
                    ?? throw new Exception("_hostingEnvironment full name is null"),
                    folderName);

                // Validate file extension
                if (ext != "png" && ext != "jpg")
                {
                    return (new CommandExecutionResult { Success = false, ErrorMessage = "File format error" }, null);
                }

                // Ensure the directory exists
                if (!Directory.Exists(fullFolderPath))
                {
                    Directory.CreateDirectory(fullFolderPath);
                }

                // Generate unique file name
                string generatedName = Guid.NewGuid().ToString();
                string filePath = Path.Combine(fullFolderPath, $"{fileName}_{generatedName}.{ext}");
                string fileAddress = $"{filePathForDb}/{folderName}/{fileName}_{generatedName}.{ext}";

                // Convert base64 string to byte array and write to file
                byte[] fileBytes = Convert.FromBase64String(base64String);
                await File.WriteAllBytesAsync(filePath, fileBytes);

                return (new CommandExecutionResult { Success = true }, filePath);
            }
            catch (Exception ex)
            {
                return (new CommandExecutionResult { Success = false, ErrorMessage = $"File write error: {ex.Message}" }, null);
            }
        }






    }
}
