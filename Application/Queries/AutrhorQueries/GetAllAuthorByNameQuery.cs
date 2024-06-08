using Application.Shared;
using Microsoft.IdentityModel.Tokens;
using Shared;

namespace Application.Queries.AutrhorQueries;

public class GetAllAuthorByNameQuery : Query<List<GetAuthorQueryResultItem>?>
{
    public string? Name { get; set; }
    public override async Task<QueryExecutionResult<List<GetAuthorQueryResultItem>?>> Execute()
    {
        var result = Name.IsNullOrEmpty() ? _appContext.Authors.Where(x => x.IsDeleted == false).AsQueryable() : _appContext.Authors.Where(x => x.IsDeleted == false && x.FirstName.Contains(Name)).AsQueryable();
      
        if (result.IsNotNull())
        {
            return await Ok(result.Select(x => new GetAuthorQueryResultItem
            {
                Id = x.Id,
                LastName = x.LastName,
                BirthDate = x.BirthDate,
                FirstName = x.FirstName
            }).ToList());
        }
        return await Ok(null);
    }
}
