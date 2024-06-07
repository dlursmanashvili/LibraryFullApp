using Application.Shared;
using Domain.Entities.FileEntity;
using Shared;
using System.ComponentModel.DataAnnotations;

namespace Application.Commands.AuthorCommands;

public class AddBookCommand : Command
{
    public string Title { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }
    //public string PathImg { get; set; }
    public int Rating { get; set; }
    public DateTime PublishDate { get; set; }
    public bool BookinLibrary { get; set; } = true;
    public FileClass? File { get; set; }

    public override async Task<CommandExecutionResult> ExecuteAsync()
    {

        if (File.IsNotNull() && File.ext.IsNotNull() && File.base64String.IsNotNull())
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

            return await bookRepository.CreateAsync(new Domain.BookEntity.Book()
            {
                Title = Title,
                Description = Description,
                BookinLibrary = BookinLibrary,
                PublishDate = PublishDate,
                PathImg = fileResult.filePath,
                Rating = Rating,
                CreatedAt = DateTime.Now,
            });
        }

        return await bookRepository.CreateAsync(new Domain.BookEntity.Book()
        {
            Title = Title,
            Description = Description,
            BookinLibrary = BookinLibrary,
            PublishDate = PublishDate,
            PathImg = null,
            Rating = Rating,
            CreatedAt = DateTime.Now,
        });
    }
}
