using Application.Shared;
using Shared;

namespace Application.Commands.AuthorCommands;

public class EditBookCommand : Command
{
    public string Title { get; set; }
    public int Id { get; set; }
    public string? Description { get; set; }
    public string? AuthorId { get; set; }
    public string PathImg { get; set; }
    public int Rating { get; set; }
    public DateTime PublishDate { get; set; }
    public int BookStatus { get; set; }
    public override async Task<CommandExecutionResult> ExecuteAsync()
    {
        return await bookRepository.UpdateAsync(new Domain.BookEntity.Book()
        {
            Id = Id,
            Title = Title,
            AuthorId = AuthorId,
            Description = Description,
            BookStatus = BookStatus,
            PublishDate = PublishDate,
            PathImg = PathImg,
            Rating = Rating,
            UpdatedAt = DateTime.Now
        });
    }
}
