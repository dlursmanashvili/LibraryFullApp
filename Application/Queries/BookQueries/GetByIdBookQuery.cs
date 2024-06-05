using Application.Shared;
using Shared;

namespace Application.Queries.BookQueries;

public class GetByIdBookQuery : Query<GetBookQueryResultItem?>
{
    public int Id { get; set; }

    public override async Task<QueryExecutionResult<GetBookQueryResultItem?>> Execute()
    {
        var result = await bookRepository.GetByIdAsync(Id);

        if (result.IsNotNull())
        {
            return await Ok(new GetBookQueryResultItem()
            {
                Id = result.Id,
                AuthorId = result.AuthorId,
                BookStatus = result.BookStatus,
                Description = result.Description,
                PathImg = result.PathImg,
                PublishDate = result.PublishDate,
                Rating = result.Rating,
                Title = result.Title


            });
        }
        return await Ok(null);
    }
}
