using Shared;
using System.Threading.Tasks;

namespace Application.Shared
{
    public interface ICommandExecutor
    {
        Task<CommandExecutionResult> Execute(Command command);
    }
}
