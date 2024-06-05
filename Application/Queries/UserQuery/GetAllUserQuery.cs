using Application.Shared;
using Shared;

namespace Application.Queries.UserQuery
{
    public class GetAllUserQuery : Query<GetAllUserQueryResult>
    {
        public override async Task<QueryExecutionResult<GetAllUserQueryResult>> Execute()
        {

            var result = _appContext.Users?.Select(x => new UserQueryResultItem()
            {
                Id = x.Id,
                Email = x.Email,
                //IsActive = x.IsActive,
                FirstName = x.FirstName, 
                LastName = x.LastName,
                PNumber = x.PNumber,
                Phone = x.PhoneNumber,
                Username = x.UserName,
                IsOorganisation = x.IsOorganisation,
                
            })?.ToList();


            var response = new GetAllUserQueryResult();
            response.Result = result;
            return await Ok(response);
        }
    }

    public class UserQueryResultItem
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public string PNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public bool IsOorganisation { get; set; }
        public string? RoleName{ get; set; }


    }

    public class GetAllUserQueryResult
    {
        public List<UserQueryResultItem>? Result { get; set; }
        public int? listCount { get; set; }
    }
}
