using Application.Shared;
using Shared;

namespace Application.Commands.BookAuthorcommands;

public class DeleteBookAuthorcommand : Command
{
    public int Id { get; set; }
    public override async Task<CommandExecutionResult> ExecuteAsync()
    {
        return await bookAuthorRepository.DeleteAsync(Id);
    }
}
