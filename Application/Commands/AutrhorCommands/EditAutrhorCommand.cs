using Application.Shared;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.AutrhorCommands;

public class EditAutrhorCommand : Command
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public override async Task<CommandExecutionResult> ExecuteAsync()
    {
        return await authorRepository.UpdateAsync(new Domain.AuthorEntity.Author()
        {
            Id = Id,
            BirthDate = BirthDate,
            CreatedAt = DateTime.Now,
            FirstName = FirstName,
            LastName = LastName
        });
    }
}
