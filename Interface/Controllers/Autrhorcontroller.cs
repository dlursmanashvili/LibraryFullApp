using Application.Commands.AutrhorCommands;
using Application.Queries.AutrhorQueries;
using Application.Shared;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace Interface.Controllers;

public class Autrhorcontroller : ControllerBase
{
    private readonly ICommandExecutor _commandExecutor;
    private readonly IQueryExecutor _queryExecutor;

    public Autrhorcontroller(
        ICommandExecutor commandExecutor,
        IQueryExecutor queryExecutor)
    {
        _commandExecutor = commandExecutor;
        _queryExecutor = queryExecutor;
    }
    [AuthoriseHelper(UserGroups.Admin)]
    [Route("AddNewAuthor")]
    [HttpPost]
    public async Task<CommandExecutionResult> AddNewAuthor([FromBody] AddAutrhorCommand command) =>
       await _commandExecutor.Execute(command);
   
    [AuthoriseHelper(UserGroups.Admin)]
    [Route("EditAuthor")]
    [HttpPut]
    public async Task<CommandExecutionResult> EditAuthor([FromBody] EditAutrhorCommand command) =>
      await _commandExecutor.Execute(command);

    [AuthoriseHelper(UserGroups.Admin)]
    [Route("DeleteAuthor")]
    [HttpPut]
    public async Task<CommandExecutionResult> DeleteAuthor([FromBody] DeleteAutrhorCommand command) =>
    await _commandExecutor.Execute(command);

    #region Queries
    [AuthoriseHelper(UserGroups.All)]
    [Route("GetAllAuthor")]
    [HttpGet]
    public async Task<QueryExecutionResult<List<GetAuthorQueryResultItem>?>> GetAllAuthor([FromQuery] GetAllAutrhorQuery query) =>
         await _queryExecutor.Execute<GetAllAutrhorQuery, List<GetAuthorQueryResultItem>?>(query);


    [AuthoriseHelper(UserGroups.All)]
    [Route("GetAuthorByID")]
    [HttpGet]
    public async Task<QueryExecutionResult<GetAuthorQueryResultItem?>> GetAuthorByID([FromQuery] GetByIdAutrhorQuery query) =>
         await _queryExecutor.Execute<GetByIdAutrhorQuery, GetAuthorQueryResultItem?>(query);


    [AuthoriseHelper(UserGroups.All)]
    [Route("GetAuthorDetalisByAuthorName")]
    [HttpGet]
    public async Task<QueryExecutionResult<List<GetAuthorDetalisQueryResultItem>?>> GetAuthorDetalisByAuthorName([FromQuery] GetAuthorDetalisByNameQuery query) =>
        await _queryExecutor.Execute<GetAuthorDetalisByNameQuery, List<GetAuthorDetalisQueryResultItem>?>(query);


    [AuthoriseHelper(UserGroups.All)]
    [Route("GetAuthorByBookId")]
    [HttpGet]
    public async Task<QueryExecutionResult<List<GetAuthorQueryResultItem>?>> GetAuthorByBookId([FromQuery] GetAuthorByBookIdQuery query) =>
        await _queryExecutor.Execute<GetAuthorByBookIdQuery, List<GetAuthorQueryResultItem>?>(query);
    #endregion
}
