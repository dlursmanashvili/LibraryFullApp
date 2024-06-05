using Application.Shared;
using FluentValidation;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.UserCommands
{
    public class ResetPasswordByAdmin : Command
    {
        public string UserId { get; set; }
        public string Password { get; set; }
        public string RepeatPassword { get; set; }
        public string ConfirmationId { get; set; }
        public override async Task<CommandExecutionResult> ExecuteAsync()
        {
            return await userRepository.ResetPassword(UserId, Password, ConfirmationId);
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
