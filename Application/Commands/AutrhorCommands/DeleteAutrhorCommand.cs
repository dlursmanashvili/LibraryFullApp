using Application.Shared;
using Shared;

namespace Application.Commands.AutrhorCommands;

public class DeleteAutrhorCommand : Command
{
    public int Id { get;  set; }

    public override async Task<CommandExecutionResult> ExecuteAsync()
    {
        return await authorRepository.DeleteAsync(Id);
    }
}
