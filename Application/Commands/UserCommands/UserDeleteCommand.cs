using Application.Shared;
using Shared;

namespace Application.Commands.UserCommands;

public class UserDeleteCommand : Command
{
    public string userId { get; set; }
    public override async Task<CommandExecutionResult> ExecuteAsync()
    {
        return await userRepository.DeleteAsync(userId);
    }
}
