using Application.Shared;
using Shared;

namespace Application.Commands.BookAuthorcommands
{
    public class AddBookAuthorCommand : Command
    {
        public int BookId { get; set; }
        //public int AuthorId { get; set; }
        public List<int> AuthorIds { get; set; }

        public override async Task<CommandExecutionResult> ExecuteAsync()
        {
            foreach (var authorId in AuthorIds)
            {
             var result =   await bookAuthorRepository.CreateAsync(new Domain.BookAuthorEntity.BookAuthor()
                {
                    AuthorId = authorId,
                    BookId = BookId,
                    CreatedAt = DateTime.Now,

                });
                if (result.Success == false)
                {
                    return result;
                }
            }

            return new CommandExecutionResult() { Success = true };
        }
    }
}
