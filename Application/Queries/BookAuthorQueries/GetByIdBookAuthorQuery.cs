using Application.Shared;
using Shared;

namespace Application.Queries.BookAuthorQueries;

public class GetByIdBookAuthorQuery : Query<GetBookauthorQueryResultItem?>
{
    public int Id { get; set; }

    public override async Task<QueryExecutionResult<GetBookauthorQueryResultItem?>> Execute()
    {
        var result = await bookAuthorRepository.GetByIdAsync(Id);

        if (result.IsNotNull())
        {
            return await Ok(new GetBookauthorQueryResultItem()
            {
                Id = result.Id,
                BookId = result.BookId,
                AuthorId = result.AuthorId,
            });
        }
        return await Ok(null);
    }
}