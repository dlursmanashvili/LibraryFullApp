using Application.Shared;
using Shared;

namespace Application.Queries.BookAuthorQueries;

public class GetAllBookAuthorQuery : Query<List<GetBookauthorQueryResultItem>?>
{
    public override async Task<QueryExecutionResult<List<GetBookauthorQueryResultItem>?>> Execute()
    {
        var result = _appContext.BookAuthors.Where(x => x.IsDeleted == false).AsQueryable();

        if (result.IsNotNull())
        {
            return await Ok(result.Select(x => new GetBookauthorQueryResultItem
            {
                Id = x.Id,
                AuthorId = x.AuthorId,
                BookId = x.BookId,
            }).ToList());
        }
        return await Ok(null);
    }
}

public class GetBookauthorQueryResultItem
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public int AuthorId { get; set; }
}