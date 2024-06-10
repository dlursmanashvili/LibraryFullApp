using Domain.AuthorEntity;
using Domain.AuthorEntity.IRepository;
using Infrastructure.DB;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Infrastructure.Repositories;

public class AuthorRepository : BaseRepository, IAuthorRepository
{
    public AuthorRepository(ApplicationDbContext applicationDbContext, IServiceProvider serviceProvider) : base(applicationDbContext, serviceProvider)
    {
    }

    public async Task<CommandExecutionResult> CreateAsync(Author model)
    {
        try
        {
            await Insert<Author, int>(model);
            return new CommandExecutionResult { Success = true };
        }
        catch (Exception ex)
        {
            return new CommandExecutionResult { Success = false, ErrorMessage = "Error creating model." };
        }
    }

    public async Task<CommandExecutionResult> DeleteAsync(int id)
    {
        try
        {
            var author = await GetByIdAsync(id);
            if (author.IsNull() || author.IsDeleted == true) return new CommandExecutionResult { Success = false, ErrorMessage = "record not found" };

            if (_ApplicationDbContext.BookAuthors.Any(x => x.Id == id && x.IsDeleted == false))
            {
                var bookId = (from ba in _ApplicationDbContext.BookAuthors
                              join book in _ApplicationDbContext.Books on ba.Id equals book.Id
                              where book.IsDeleted == false && ba.IsDeleted == false && ba.AuthorId == author.Id
                              select book.Id).ToList();

                return new CommandExecutionResult
                {
                    Success = false,
                    ErrorMessage = $"ავტორის წაშლა ამ ეტაპზე შეუზლებელია აღნიშნული ავტორი აირს  " +
                    $"წიგნების ავტორი წიგნების id-ებია :{string.Join(", ", bookId)}. გთხოვთ სუშალოთ აღნიშნულ წიგნებს ავტორები "
                };


            }
            if (author != null)
            {
                author.IsDeleted = true;
                await Update(author);
                return new CommandExecutionResult { Success = true };
            }
            else
            {
                return new CommandExecutionResult { Success = false, ErrorMessage = "author not found." };
            }
        }
        catch (Exception ex)
        {
            return new CommandExecutionResult { Success = false, ErrorMessage = "Error deleting author." };
        }
    }

    public async Task<List<Author>?> GetAllAsync()
    {
        try
        {
            return await _ApplicationDbContext.Set<Author>().Where(x => x.IsDeleted != true).ToListAsync();
        }
        catch (Exception ex)
        {
            // Handle the exception or log it
            return new List<Author>();
        }
    }

    public async Task<Author?> GetByIdAsync(int id)
    {
        try
        {
            return await _ApplicationDbContext.Set<Author>().FindAsync(id);
        }
        catch (Exception ex)
        {
            // Handle the exception or log it
            return null;
        }
    }

    public async Task<CommandExecutionResult> UpdateAsync(Author model)
    {
        try
        {

            var result = await GetByIdAsync(model.Id);
            if (result.IsNull() || result.IsDeleted == true) return new CommandExecutionResult { Success = false, ErrorMessage = "record not found" };

            result.FirstName = model.FirstName;
            result.LastName = model.LastName;
            result.BirthDate = model.BirthDate;

            await Update(result);
            return new CommandExecutionResult { Success = true };
        }
        catch (Exception ex)
        {
            return new CommandExecutionResult { Success = false, ErrorMessage = "Error updating Author." };
        }
    }
}
