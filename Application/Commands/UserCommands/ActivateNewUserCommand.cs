using Application.Shared;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.UserCommands
{
    public class ActivateNewUserCommand : Command
    {
        public string ConfirmationGuiId { get; set; }
        public override async Task<CommandExecutionResult> ExecuteAsync()
        {
            return await userRepository.ActivateNewUser(ConfirmationGuiId);
        }
    }
}
