using Application.Shared;
using Domain.BookEntity;
using Domain.Entities.FileEntity;
using Shared;

namespace Application.Commands.AuthorCommands;

public class EditBookCommand : Command
{
    public string Title { get; set; }
    public int Id { get; set; }
    public string? Description { get; set; }
    public string PathImg { get; set; }
    public int Rating { get; set; }
    public DateTime PublishDate { get; set; }
    public int BookStatus { get; set; } = 1;
    public FileClass File { get; set; }

    public override async Task<CommandExecutionResult> ExecuteAsync()
    {
        if (!Enum.IsDefined(typeof(BookStatusEnum), BookStatus))
            return await Fail("Invalid book status.");
        

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
        return await bookRepository.UpdateAsync(new Domain.BookEntity.Book()
        {
            Id = Id,
            Title = Title,
            Description = Description,
            BookStatus = BookStatus,
            PublishDate = PublishDate,
            PathImg = fileResult.filePath,
            Rating = Rating,
            UpdatedAt = DateTime.Now
        });
    }
}
