using Domain.BookAuthorEntity;
using Domain.BookAuthorEntity.IRepository;
using Domain.BookEntity;
using Infrastructure.DB;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Infrastructure.Repositories;

public class BookAuthorRepository : BaseRepository, IBookAuthorRepository
{
    public BookAuthorRepository(ApplicationDbContext applicationDbContext, IServiceProvider serviceProvider) : base(applicationDbContext, serviceProvider)
    {
    }

    public async Task<CommandExecutionResult> CreateAsync(BookAuthor model)
    {
        try
        {
            if (!_ApplicationDbContext.Books.Any(x => x.IsDeleted == false && x.Id == model.BookId ))
            {
                return new CommandExecutionResult { Success = false, ErrorMessage = "ასეთი წიგნი არ არსებობს" };

            }
            if (!_ApplicationDbContext.Authors.Any(x => x.IsDeleted == false &&  x.Id == model.AuthorId))
            {
                return new CommandExecutionResult { Success = false, ErrorMessage = "ასეთი ავტორი არ არსებობს" };

            }
            if (_ApplicationDbContext.BookAuthors.Any(x=> x.IsDeleted == false && x.BookId == model.BookId && x.AuthorId == model.AuthorId))
            {
                return new CommandExecutionResult { Success = false, ErrorMessage = "ასეთი წიგნი ასეთი ავტორით უკვე არსებობს" };

            }
            await Insert<BookAuthor, int>(model);
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
            var BookAuthor = await _ApplicationDbContext.BookAuthors.FirstOrDefaultAsync(x => x.Id == id);
            if (BookAuthor.IsNull() || BookAuthor.IsDeleted == true) return new CommandExecutionResult { Success = false, ErrorMessage = "record not found" };

            if (BookAuthor != null)
            {
                BookAuthor.IsDeleted = true;
                await Update(BookAuthor);
                return new CommandExecutionResult { Success = true };
            }
            else
            {
                return new CommandExecutionResult { Success = false, ErrorMessage = "Book not found." };
            }
        }
        catch (Exception ex)
        {
            return new CommandExecutionResult { Success = false, ErrorMessage = "Error deleting BookAuthor." };
        }
    }

    public async Task<List<BookAuthor>?> GetAllAsync()
    {
        try
        {
            return await _ApplicationDbContext.Set<BookAuthor>().Where(x => x.IsDeleted != true).ToListAsync();
        }
        catch (Exception ex)
        {
            // Handle the exception or log it
            return new List<BookAuthor>();
        }
    }

    public async Task<BookAuthor?> GetByIdAsync(int id)
    {
        try
        {
            return await _ApplicationDbContext.Set<BookAuthor>().FindAsync(id);
        }
        catch (Exception ex)
        {
            // Handle the exception or log it
            return null;
        }
    }

    public async Task<CommandExecutionResult> UpdateAsync(BookAuthor model)
    {
        try
        {
            if (!_ApplicationDbContext.Books.Any(x => x.IsDeleted == false && x.Id == model.BookId))
            {
                return new CommandExecutionResult { Success = false, ErrorMessage = "ასეთი წიგნი არ არსებობს" };

            }
            if (!_ApplicationDbContext.Authors.Any(x => x.IsDeleted == false && x.Id == model.AuthorId))
            {
                return new CommandExecutionResult { Success = false, ErrorMessage = "ასეთი ავტორი არ არსებობს" };

            }
            if (_ApplicationDbContext.BookAuthors.Any(x => x.IsDeleted == false && x.BookId == model.BookId && x.AuthorId == model.AuthorId))
            {
                return new CommandExecutionResult { Success = false, ErrorMessage = "ასეთი წიგნი ასეთი ავტორით უკვე არსებობს" };

            }
            var result = await GetByIdAsync(model.Id);
            if (result.IsNull() || result.IsDeleted == true) return new CommandExecutionResult { Success = false, ErrorMessage = "record not found" };

            result.AuthorId = model.AuthorId;
            result.BookId = model.BookId;         


            await Update(result);
            return new CommandExecutionResult { Success = true };
        }
        catch (Exception ex)
        {
            return new CommandExecutionResult { Success = false, ErrorMessage = "Error updating BookAuthor." };
        }
    }
}
