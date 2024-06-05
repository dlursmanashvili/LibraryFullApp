using FluentValidation.Attributes;
using FluentValidation.Results;
using Infrastructure.DB;
using Microsoft.Extensions.Configuration;
using Shared;
using System.Reflection;

namespace Application.Shared
{
    public class CommandExecutor : ICommandExecutor
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;

        public CommandExecutor(
            IServiceProvider serviceProvider,
            ApplicationDbContext applicationDbContext,
            IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _applicationDbContext = applicationDbContext;
            _configuration = configuration;
        }
        public async Task<CommandExecutionResult> Execute(Command command)
        {
            try
            {
                var validationResult = Validate(command);
                if (!validationResult.IsValid)
                {
                    return new CommandExecutionResult
                    {
                        Success = false,
                        Errors = validationResult.Errors.Select(error => new Error { Message = error.ErrorMessage, Code = 0 })
                    };
                }

                command.Resolve(_applicationDbContext, _serviceProvider, _configuration);

                var commandResult = await command.ExecuteAsync();

                return commandResult;

            }
            catch (Exception ex)
            {

                return new CommandExecutionResult
                {
                    Success = false,
                    Errors = new List<Error>
                    {
                        new Error
                        {
                            Code = 0,
                            Message = ex.ToString() // TEMP:
                        }
                    }
                };
            }
        }

        public static ValidationResult Validate(Command execution)
        {
            var validatorAttribute = execution.GetType().GetCustomAttribute<ValidatorAttribute>(true);
            if (validatorAttribute != null)
            {
                var instance = (dynamic)Activator.CreateInstance(validatorAttribute.ValidatorType);
                var modelState = instance.Validate((dynamic)execution);
                return modelState;
            }

            return new ValidationResult();
        }
    }
}
