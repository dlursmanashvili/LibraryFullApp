using Application.Shared;
using Shared;

namespace Application.Queries.UserQuery;

public class GetOneUserByIDQuery : Query<GetOneUserByIDQueryResult>
{
    public string UserID { get; set; }
    public override async Task<QueryExecutionResult<GetOneUserByIDQueryResult>> Execute()
    {
        var result = await _userManager.FindByIdAsync(UserID);

        if (result.IsNull() || result.IsActive == false)
        {
            return await Fail("user not found");
        }
        else
        {
            return await Ok(new GetOneUserByIDQueryResult()
            {
                Id = result.Id,
                Email = result.Email,
                IsActive = result.IsActive,
                Username = result.UserName,
                PNumber = result.PNumber,
                FirstName = result.FirstName,
                LastName = result.LastName,
                Phone = result.PhoneNumber,
                IsOorganisation = result.IsOorganisation,
            });
        }
    }
}

public class GetOneUserByIDQueryResult : UserQueryResultItem
{
    
}