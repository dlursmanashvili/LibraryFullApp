using Application.Commands.UserCommands;
using Application.Queries.UserQuery;
using Application.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared;
using static Application.Queries.UserQuery.LoginQuery;

namespace Interface.Controllers;

//[Authorize]
[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ICommandExecutor _commandExecutor;
    private readonly IQueryExecutor _queryExecutor;

    public UserController(
        ICommandExecutor commandExecutor,
        IQueryExecutor queryExecutor)
    {
        _commandExecutor = commandExecutor;
        _queryExecutor = queryExecutor;
    }

    #region Queries
   
    [Authorize(Roles = UserGroups.Admin)]
    [Route("GetUsersByName")]
    [HttpGet]
    public async Task<QueryExecutionResult<GetAllUserQueryResult>> GetUsersByName([FromQuery] GetUsersByNameQeuery query) =>
         await _queryExecutor.Execute<GetUsersByNameQeuery, GetAllUserQueryResult>(query);

   

    [Authorize]
    [Route("GetOneUserByID")]
    [HttpGet]
    public async Task<QueryExecutionResult<GetOneUserByIDQueryResult>> GetOneUserByID([FromQuery] GetOneUserByIDQuery query) =>
     await _queryExecutor.Execute<GetOneUserByIDQuery, GetOneUserByIDQueryResult>(query);


    [Route("Login")]
    [HttpGet]
    public async Task<QueryExecutionResult<LoginQueryResult>> Login([FromQuery] LoginQuery query) =>
     await _queryExecutor.Execute<LoginQuery, LoginQueryResult>(query);


    #endregion

    #region Commands
    [Route("Registration")]
    [HttpPost]
    public async Task<CommandExecutionResult> Registration([FromBody] RegistrationCommand command) =>
         await _commandExecutor.Execute(command);


    [Route("ForgetPassword")]
    [HttpPost]
    public async Task<CommandExecutionResult> ResetPassword([FromBody] ForgetPasswordCommand command) =>
       await _commandExecutor.Execute(command);

    [Route("ResetPassword")]
    [HttpPost]
    public async Task<CommandExecutionResult> ResetPassword([FromBody] ResetPasswordCommand command) =>
            await _commandExecutor.Execute(command);

    [Route("activatenewuser")]
    [HttpPost]
    public async Task<CommandExecutionResult> activatenewuser([FromBody] ActivateNewUserCommand command) =>
            await _commandExecutor.Execute(command);

    [Route("ResetPasswordByAdmin")]
    [HttpPost]
    [AuthoriseHelper("RootAdmin")]
    public async Task<CommandExecutionResult> ResetPasswordByAdmin([FromBody] ResetPasswordByAdmin command) =>
            await _commandExecutor.Execute(command);

    [Route("ForgetPasswordByAdmin")]
    [HttpPost]
    [AuthoriseHelper("RootAdmin")]
    public async Task<CommandExecutionResult> ForgetPasswordByAdmin([FromBody] ForgetPasswordByAdminCommand command) =>
       await _commandExecutor.Execute(command);

    [Route("RegistrationByAdmin")]
    [HttpPost]
    [AuthoriseHelper("RootAdmin")]
    public async Task<CommandExecutionResult> RegistrationByAdmin([FromBody] RegistrationByAdminCommand command) =>
     await _commandExecutor.Execute(command);

    [Authorize]
    [Route("DeleteUser")]
    [HttpDelete]
    public async Task<CommandExecutionResult> DeleteUser([FromBody] UserDeleteCommand command) =>
          await _commandExecutor.Execute(command);

    [Authorize]
    [Route("UpdateUser")]
    [HttpPost]
    public async Task<CommandExecutionResult> UpdateUser([FromBody] UserUpdateCommand command) =>
            await _commandExecutor.Execute(command);
    #endregion
}
