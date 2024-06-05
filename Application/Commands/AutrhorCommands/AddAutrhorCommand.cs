using Application.Shared;
using Shared;

namespace Application.Commands.AutrhorCommands;

public class AddAutrhorCommand : Command
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public override async Task<CommandExecutionResult> ExecuteAsync()
    {
        return await authorRepository.CreateAsync(new Domain.AuthorEntity.Author()
        {
            BirthDate = BirthDate,
            CreatedAt = DateTime.Now,
            FirstName = FirstName,
            LastName = LastName
        });
    }
}
