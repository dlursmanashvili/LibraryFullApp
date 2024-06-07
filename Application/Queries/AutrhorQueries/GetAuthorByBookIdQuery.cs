using Application.Shared;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Application.Queries.AutrhorQueries;

public class GetAuthorByBookIdQuery : Query<List<GetAuthorQueryResultItem>?>
{
    public int BookId { get; set; }
    public override async Task<QueryExecutionResult<List<GetAuthorQueryResultItem>?>> Execute()
    {
        var result = await (from bookAuthor in _appContext.BookAuthors
                            join author in _appContext.Authors on bookAuthor.AuthorId equals author.Id
                            where bookAuthor.BookId == BookId && bookAuthor.IsDeleted == false && author.IsDeleted == false
                            select new GetAuthorQueryResultItem()
                            {
                                Id = author.Id,
                                BirthDate = author.BirthDate,
                                FirstName = author.FirstName,
                                LastName = author.LastName,

                            }).ToListAsync();

        return await Ok(result);
    }
}
