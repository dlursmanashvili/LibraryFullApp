using Application.Shared;
using Domain.BookEntity;
using Domain.Entities.FileEntity;
using Shared;
using System.ComponentModel.DataAnnotations;

namespace Application.Commands.AuthorCommands;

public class AddBookCommand : Command
{
    public string Title { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }
    public string? AuthorId { get; set; }
    public string PathImg { get; set; }
    public int Rating { get; set; }
    public DateTime PublishDate { get; set; }
    public int BookStatus { get; set; } = 1;
    public FileClass File { get; set; }

    public override async Task<CommandExecutionResult> ExecuteAsync()
    {
        if (!Enum.IsDefined(typeof(BookStatusEnum), BookStatus))
        {
            return new CommandExecutionResult
            {
                Success = false,
                ErrorMessage = "Invalid book status."
            };
        }

        var fileResult = await fileClassRepository.SaveFileLocal(
          "Files/",
          "\\Files\\",
          "BookFiles",
          File.base64String,
          "BookFiles",
          File.ext);

        if (fileResult.result.Success == false)
        {
            return fileResult.result;
        }
        return await bookRepository.CreateAsync(new Domain.BookEntity.Book()
        {
            Title = Title,
            AuthorId = AuthorId,
            Description = Description,
            BookStatus = BookStatus,
            PublishDate = PublishDate,
            PathImg = fileResult.filePath,
            Rating = Rating,
            CreatedAt = DateTime.Now,
        });
    }
}
