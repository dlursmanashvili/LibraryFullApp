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
    //public string PathImg { get; set; }
    public int Rating { get; set; }
    public DateTime PublishDate { get; set; }
    public bool BookinLibrary { get; set; } = true;
    public FileClass? File { get; set; }

    public override async Task<CommandExecutionResult> ExecuteAsync()
    {
        if ( File.IsNotNull()  &&File.ext != null && File.base64String != null)
        {
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
                BookinLibrary = BookinLibrary,
                PublishDate = PublishDate,
                PathImg = fileResult.filePath,
                Rating = Rating,
                UpdatedAt = DateTime.Now
            });
        }

        return await bookRepository.UpdateAsync(new Domain.BookEntity.Book()
        {
            Id = Id,
            Title = Title,
            Description = Description,
            BookinLibrary = BookinLibrary,
            PublishDate = PublishDate,
            PathImg = null,
            Rating = Rating,
            UpdatedAt = DateTime.Now
        });

    }
}
