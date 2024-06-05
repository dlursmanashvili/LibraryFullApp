using Application.Shared;
using Domain.UserEntity;
using Shared;

namespace Application.Commands.UserCommands;

public class UserUpdateCommand : Command
{
    public string id { get; set; }
    //public string username { get; set; }
    //public string password { get; set; }
    //public string email { get; set; }
    public string PNumber { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Phone { get; set; }
    public bool IsOorganisation { get; set; }

    public bool isActive { get; set; }
    public override async Task<CommandExecutionResult> ExecuteAsync()
    {
        return await userRepository.UpdateAsyncUser(new User()
        {
            Id = id,            
            PNumber = PNumber,
            FirstName = FirstName,
            LastName = LastName,
            PhoneNumber = Phone,
            IsActive = isActive,
            IsOorganisation = IsOorganisation,
        });
    }
}
