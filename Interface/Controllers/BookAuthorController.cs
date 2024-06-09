using Application.Commands.AuthorCommands;
using Application.Commands.BookAuthorcommands;
using Application.Queries.AutrhorQueries;
using Application.Queries.BookAuthorQueries;
using Application.Shared;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace Interface.Controllers
{
    public class BookAuthorAuthorController : ControllerBase
    {
        private readonly ICommandExecutor _commandExecutor;
        private readonly IQueryExecutor _queryExecutor;

        public BookAuthorAuthorController(
            ICommandExecutor commandExecutor,
            IQueryExecutor queryExecutor)
        {
            _commandExecutor = commandExecutor;
            _queryExecutor = queryExecutor;
        }

        [AuthoriseHelper(UserGroups.Admin)]
        [Route("AddNewBookAuthor")]
        [HttpPost]
        public async Task<CommandExecutionResult> AddNewBookAuthor([FromBody] AddBookAuthorCommand command) =>
           await _commandExecutor.Execute(command);

        [AuthoriseHelper(UserGroups.Admin)]
        [Route("EditBookAuthor")]
        [HttpPut]
        public async Task<CommandExecutionResult> EditBookAuthor([FromBody] EditBookAuthorcommand command) =>
          await _commandExecutor.Execute(command);

        [AuthoriseHelper(UserGroups.Admin)]
        [Route("DeleteBookAuthor")]
        [HttpPut]
        public async Task<CommandExecutionResult> DeleteBookAuthor([FromBody] DeleteBookAuthorcommand command) =>
        await _commandExecutor.Execute(command);

        #region Queries
        [AuthoriseHelper(UserGroups.Admin)]
        [Route("GetAllBookAuthor")]
        [HttpGet]
        public async Task<QueryExecutionResult<List<GetBookauthorQueryResultItem>?>> GetAllBookAuthor([FromQuery] GetAllBookAuthorQuery query) =>
             await _queryExecutor.Execute<GetAllBookAuthorQuery, List<GetBookauthorQueryResultItem>?>(query);


        [AuthoriseHelper(UserGroups.Admin)]
        [Route("GetBookAuthorByID")]
        [HttpGet]
        public async Task<QueryExecutionResult<GetBookauthorQueryResultItem?>> GetBookAuthorByID([FromQuery] GetByIdBookAuthorQuery query) =>
             await _queryExecutor.Execute<GetByIdBookAuthorQuery, GetBookauthorQueryResultItem?>(query);
        #endregion
    }

}
