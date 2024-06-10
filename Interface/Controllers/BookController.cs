using Application.Commands.AuthorCommands;
using Application.Queries.AutrhorQueries;
using Application.Queries.BookQueries;
using Application.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace Interface.Controllers;

public class BookController : ControllerBase
{
    private readonly ICommandExecutor _commandExecutor;
    private readonly IQueryExecutor _queryExecutor;

    public BookController(
        ICommandExecutor commandExecutor,
        IQueryExecutor queryExecutor)
    {
        _commandExecutor = commandExecutor;
        _queryExecutor = queryExecutor;
    }

    [AuthoriseHelper(UserGroups.Admin)]
    [Route("AddNewBook")]
    [HttpPost]
    public async Task<CommandExecutionResult> AddNewBook([FromBody] AddBookCommand command) =>
       await _commandExecutor.Execute(command);

    [AuthoriseHelper(UserGroups.Admin)]
    [Route("EditBook")]
    [HttpPut]
    public async Task<CommandExecutionResult> EditBook([FromBody] EditBookCommand command) =>
      await _commandExecutor.Execute(command);

    [AuthoriseHelper(UserGroups.Admin)]
    [Route("DeleteBook")]
    [HttpPut]
    public async Task<CommandExecutionResult> DeleteBook([FromBody] DeleteBookCommand command) =>
    await _commandExecutor.Execute(command);

    #region Queries
    [Authorize]
    [Route("GetAllBook")]
    [HttpGet]
    public async Task<QueryExecutionResult<List<GetBookQueryResultItem>?>> GetAllBook([FromQuery] GetAllBookQuery query) =>
         await _queryExecutor.Execute<GetAllBookQuery, List<GetBookQueryResultItem>?>(query);


    [Authorize]
    [Route("GetBookByID")]
    [HttpGet]
    public async Task<QueryExecutionResult<GetBookQueryResultItem?>> GetBookByID([FromQuery] GetByIdBookQuery query) =>
         await _queryExecutor.Execute<GetByIdBookQuery, GetBookQueryResultItem?>(query);


    [Authorize]
    [Route("GetbookDetalisbyName")]
    [HttpGet]
    public async Task<QueryExecutionResult<List<GetbookDetalisQueryResultItem>?>> GetbookDetalisbyName([FromQuery] GetbookDetalisbyNameQuery query) =>
       await _queryExecutor.Execute<GetbookDetalisbyNameQuery, List<GetbookDetalisQueryResultItem>?>(query);


    [Authorize]
    [Route("GetOneAuthorBooksByAuthorID")]
    [HttpGet]
    public async Task<QueryExecutionResult<GetAuthorDetalisQueryResultItem?>> GetOneAuthorBooksByAuthorID([FromQuery] GetOneAuthorBooksByAuthorIDQuery query) =>
       await _queryExecutor.Execute<GetOneAuthorBooksByAuthorIDQuery, GetAuthorDetalisQueryResultItem?>(query);
    #endregion
}
