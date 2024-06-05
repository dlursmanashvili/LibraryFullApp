using Application.Shared;
using Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    public int BookStatus { get; set; }
    public override async Task<CommandExecutionResult> ExecuteAsync()
    {
        return await bookRepository.CreateAsync (new Domain.BookEntity.Book()
        {
            Title = Title,
            AuthorId = AuthorId,
            Description = Description,
            BookStatus = BookStatus,
            PublishDate = PublishDate,
            PathImg = PathImg,
            Rating = Rating,
            CreatedAt = DateTime.Now,
        });
    }
}
