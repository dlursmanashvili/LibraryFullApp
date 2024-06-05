using Shared;

namespace Application.Shared;

public class ResponseHelper
{
    protected Task<CommandExecutionResult> Fail(string? errorMessage)
    {
        var result = new CommandExecutionResult
        {
            Success = false
        };

        result.Errors = new List<Error>
        {
            new Error
            {
                Message = errorMessage ?? string.Empty
            }
        };

        return Task.FromResult(result);
    }
    protected Task<CommandExecutionResult> Ok(string resultId, long? listCount = null)
    {
        var result = new CommandExecutionResult
        {
            ResultId = resultId,
            Success = true,
            ListCount = listCount,
            Code = 200
        };

        return Task.FromResult(result);
    }

   
}
