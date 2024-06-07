using Domain.BookEntity;
using Domain.BookEntity.IBookRepository;
using Infrastructure.DB;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Infrastructure.Repositories;

public class BookRepository : BaseRepository, IBookRepository
{
    public BookRepository(ApplicationDbContext applicationDbContext, IServiceProvider serviceProvider) : base(applicationDbContext, serviceProvider)
    {
    }

    public async Task<CommandExecutionResult> CreateAsync(Book model)
    {
        try
        {
            await Insert<Book, int>(model);
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
            var Book = await GetByIdAsync(id);
            if (Book.IsNull() || Book.IsDeleted == true) return new CommandExecutionResult { Success = false, ErrorMessage = "record not found" };

            if (Book != null)
            {
                Book.IsDeleted = true;
                await Update(Book);
                return new CommandExecutionResult { Success = true };
            }
            else
            {
                return new CommandExecutionResult { Success = false, ErrorMessage = "Book not found." };
            }
        }
        catch (Exception ex)
        {
            return new CommandExecutionResult { Success = false, ErrorMessage = "Error deleting Book." };
        }
    }

    public async Task<List<Book>?> GetAllAsync()
    {
        try
        {
            return await _ApplicationDbContext.Set<Book>().Where(x => x.IsDeleted != true).ToListAsync();
        }
        catch (Exception ex)
        {
            // Handle the exception or log it
            return new List<Book>();
        }
    }

    public async Task<Book?> GetByIdAsync(int id)
    {
        try
        {
            return await _ApplicationDbContext.Set<Book>().FindAsync(id);
        }
        catch (Exception ex)
        {
            // Handle the exception or log it
            return null;
        }
    }

    public async Task<CommandExecutionResult> UpdateAsync(Book model)
    {
        try
        {

            var result = await GetByIdAsync(model.Id);
            if (result.IsNull() || result.IsDeleted == true) return new CommandExecutionResult { Success = false, ErrorMessage = "record not found" };

            result.BookinLibrary = model.BookinLibrary;
            result.Title = model.Title;
            result.Description = model.Description;
            result.PublishDate = model.PublishDate;
            result.PathImg = model.PathImg;
            result.Rating = model.Rating;
            result.IsDeleted = model.IsDeleted;
            

            await Update(result);
            return new CommandExecutionResult { Success = true };
        }
        catch (Exception ex)
        {
            return new CommandExecutionResult { Success = false, ErrorMessage = "Error updating Book." };
        }
    }
}
