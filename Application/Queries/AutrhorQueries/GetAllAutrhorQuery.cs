using Application.Shared;
using Shared;

namespace Application.Queries.AutrhorQueries;

public class GetAllAutrhorQuery : Query<List<GetAuthorQueryResultItem>?>
{
    public override async Task<QueryExecutionResult<List<GetAuthorQueryResultItem>?>> Execute()
    {
        var result = _appContext.Authors.Where(x => x.IsDeleted == false).AsQueryable();

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
