﻿using Application.Shared;
using Shared;

namespace Application.Commands.AuthorCommands;

public class DeleteBookCommand : Command
{
    public int Id { get;  set; }

    public override async Task<CommandExecutionResult> ExecuteAsync()
    {
        return await bookRepository.DeleteAsync(Id);
    }
}
