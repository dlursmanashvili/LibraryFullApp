using Application.Shared;
using Shared;

namespace Application.Commands.BookAuthorcommands
{
    public class AddBookAuthorcommand : Command
    {
        public int BookId { get; set; }
        public int AuthorId { get; set; }

        public override async Task<CommandExecutionResult> ExecuteAsync()
        {
            return await bookAuthorRepository.CreateAsync(new Domain.BookAuthorEntity.BookAuthor()
            {
                AuthorId = AuthorId,
                BookId = BookId,
                CreatedAt = DateTime.Now,

            });
        }
    }
}
