using Application.Shared;
using Shared;

namespace Application.Commands.BookAuthorcommands;

public class DeleteBookAuthorcommand : Command
{
    public int BookId { get; set; }
    public List<int> AuthorIds { get; set; }

    public override async Task<CommandExecutionResult> ExecuteAsync()
    {
        try
        {
            var bookAuthor = applicationDbContext.BookAuthors.Where(x => x.IsDeleted == false && x.BookId == BookId && AuthorIds.Contains(x.AuthorId)).ToList();
            if (bookAuthor.IsNotNull())
            {
                foreach (var item in bookAuthor)
                {

                    await bookAuthorRepository.DeleteAsync(item.Id);
                }
            }

            return new CommandExecutionResult() { Success = true };
        }
        catch (Exception ex)
        {
            return new CommandExecutionResult() { Success = false, ErrorMessage = ex.Message.ToString() };
        }

    }
}
