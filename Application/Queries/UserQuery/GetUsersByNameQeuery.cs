using Application.Queries.UserQuery;
using Application.Shared;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.UserQuery
{
    public class GetUsersByNameQeuery : Query<GetAllUserQueryResult>
    {
        public string Param { get; set; }
        public override async Task<QueryExecutionResult<GetAllUserQueryResult>> Execute()
        {

            var dataResponse = await _appContext.Users.Where(x => (x.FirstName + " " + x.LastName).Contains(Param)).Take(50).ToListAsync();

            var result = dataResponse.Select(x => new UserQueryResultItem()
            {
                Id= x.Id,
                Email = x.Email,
                IsActive = x.IsActive,
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
}


