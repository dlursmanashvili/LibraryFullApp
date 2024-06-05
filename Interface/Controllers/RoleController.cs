using Application.Commands.RoleCommands;
using Application.Queries.RoleQueries;
using Application.Shared;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace Interface.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly ICommandExecutor _commandExecutor;
        private readonly IQueryExecutor _queryExecutor;

        public RoleController(
            ICommandExecutor commandExecutor,
            IQueryExecutor queryExecutor)
        {
            _commandExecutor = commandExecutor;
            _queryExecutor = queryExecutor;
        }

        [AuthoriseHelper(UserGroups.Admin)]
        [Route("AddNewRole")]
        [HttpPost]
        public async Task<CommandExecutionResult> AddNewRole([FromBody] AddNewRoleCommand command) =>
         await _commandExecutor.Execute(command);

        [AuthoriseHelper(UserGroups.Admin)]
        [Route("EditRoleName")]
        [HttpPut]
        public async Task<CommandExecutionResult> EditRoleName([FromBody] EditRoleNameCommand command) =>
          await _commandExecutor.Execute(command);

        [AuthoriseHelper(UserGroups.Admin)]
        [Route("DeleteRole")]
        [HttpDelete]
        public async Task<CommandExecutionResult> DeleteRole([FromBody] DeleteRoleCommand command) =>
         await _commandExecutor.Execute(command);

        [AuthoriseHelper(UserGroups.Admin)]
        [Route("GetRoles")]
        [HttpGet]
        public async Task<QueryExecutionResult<GetRolesQueryResult>> GetRoles([FromQuery] GetRolesQuery query) =>
             await _queryExecutor.Execute<GetRolesQuery, GetRolesQueryResult>(query);
        
        [AuthoriseHelper(UserGroups.Admin)]
        [Route("GetRoleById")]
        [HttpPost]
        public async Task<QueryExecutionResult<GetRoleByIdQueryResult>> GetRoleById([FromBody] GetRoleByIdQuery query) =>
            await _queryExecutor.Execute<GetRoleByIdQuery, GetRoleByIdQueryResult>(query);


        [AuthoriseHelper(UserGroups.Admin)]
        [Route("GetAllRoles")]
        [HttpGet]
        public async Task<QueryExecutionResult<List<RoleItemResponseItem>?>> GetAllRoles([FromQuery] GetAllRoles query) =>
            await _queryExecutor.Execute<GetAllRoles, List<RoleItemResponseItem>?>(query);
    }
}
