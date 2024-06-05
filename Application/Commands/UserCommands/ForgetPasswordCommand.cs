using Application.Shared;
using Domain.UserEntity;
using Shared;

namespace Application.Commands.UserCommands
{
    public class ForgetPasswordCommand : Command
    {
        public string Email { get; set; }

        public override async Task<CommandExecutionResult> ExecuteAsync()
        {

            var user = applicationDbContext.Set<User>().FirstOrDefault(x => x.Email == Email);

            if (user.IsNull())
            {
                return await Fail("user noy found");//მომხმარებელი ამ მეილით არ მოიძებნა
            }
            else
            {
                return await userRepository.ForgetPassword(user);
            }
        }
    }
}
