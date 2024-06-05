using Shared;
using System.Web.Mvc;

namespace Domain.Entities.FileEntity.IRepository
{
    public interface IFileClassRepository
    {
        Task<(CommandExecutionResult result, string filePath)> SaveFileServer(string FilePathForDb, string hostingEnvironmentPath, string folderName, string base64String, string fileName, string ext);

        Task<(CommandExecutionResult result, string filePath)> SaveFileLocal(string FilePathForDb, string hostingEnvironmentPath, string folderName, string base64String, string fileName, string ext);     
    }
}
