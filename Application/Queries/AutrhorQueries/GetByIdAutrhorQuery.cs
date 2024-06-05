using Application.Shared;
using Shared;

namespace Application.Queries.AutrhorQueries;

public class GetByIdAutrhorQuery : Query<GetAuthorQueryResultItem?>
{
    public int Id { get; set; }

    public override async Task<QueryExecutionResult<GetAuthorQueryResultItem?>> Execute()
    {
        var result = await authorRepository.GetByIdAsync(Id);

        if (result.IsNotNull())
        {
            return await Ok(new GetAuthorQueryResultItem()
            {
                LastName = result.LastName,
                Id = result.Id,
                BirthDate = result.BirthDate,
                FirstName = result.FirstName
            });
        }
        return await Ok(null);
    }
}
public class GetAuthorQueryResultItem
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
}