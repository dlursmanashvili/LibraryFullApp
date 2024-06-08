using Application.Shared;
using Shared;

namespace Application.Commands.BookAuthorcommands
{

    public class EditBookAuthorcommand : Command
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int AuthorId { get; set; }

        public override async Task<CommandExecutionResult> ExecuteAsync()
        {
            return await bookAuthorRepository.UpdateAsync(new Domain.BookAuthorEntity.BookAuthor()
            {
                Id = Id,
                AuthorId = AuthorId,
                BookId = BookId,
                UpdatedAt = DateTime.Now,

            });
        }
    }
}
