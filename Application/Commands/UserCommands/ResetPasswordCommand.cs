using Application.Shared;
using FluentValidation;
using FluentValidation.Attributes;
using Shared;

namespace Application.Commands.UserCommands
{
    [Validator(typeof(ResetPasswordCommandValidation))]
    public class ResetPasswordCommand : Command
    {
        public string userId { get; set; }
        public string Password { get; set; }
        public string RepeatPassword { get; set; }
        public string ConfirmationId { get; set; }

        public override async Task<CommandExecutionResult> ExecuteAsync()
        {
            if(Password != RepeatPassword)
            {
                return new CommandExecutionResult() { ErrorMessage = "პაროლები არ ემთხვევა ერთმანეთს", Success = false };
            }
            return await userRepository.ResetPassword(userId, Password, ConfirmationId);
        }
        public class ResetPasswordCommandValidation : AbstractValidator<ResetPasswordCommand>
        {
            public ResetPasswordCommandValidation()
            {
                RuleFor(x => x.Password).Equal(x => x.RepeatPassword);
                RuleFor(x => x.Password).NotEmpty().WithMessage("this value is required");//"პაროლის ველის შევსება აუცილებელია"
            }
        }
    }
}
