using Domain.AuthorEntity.IRepository;
using Domain.BookAuthorEntity.IRepository;
using Domain.BookEntity.IBookRepository;
using Domain.Entities.FileEntity.IRepository;
using Domain.RoleEntity.IRepository;
using Domain.UserEntity;
using Domain.UserEntity.IRepository;
using Infrastructure.DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared;

namespace Application.Shared;

public abstract class Command : ResponseHelper
{
    protected ApplicationDbContext applicationDbContext;
    protected IServiceProvider ServiceProvider;
    protected IConfiguration Configuration;
    protected IRoleRepository RoleRepository;
    protected IUserRepository userRepository;
    protected IAuthorRepository authorRepository;
    protected IBookRepository bookRepository;
    protected IBookAuthorRepository bookAuthorRepository;
    protected IFileClassRepository fileClassRepository;

    public abstract Task<CommandExecutionResult> ExecuteAsync();

    protected UserManager<User> _userManager;

    protected string? UserId;
    protected string? Username;
    public void Resolve(ApplicationDbContext applicationContext, IServiceProvider serviceProvider, IConfiguration configuration)
    {
        ServiceProvider = serviceProvider;
        Configuration = configuration;
        applicationDbContext = applicationContext;

        var user = ServiceProvider.GetService<IHttpContextAccessor>()?.HttpContext?.User;

        if (user.IsNotNull() && user.Claims.Any())
        {
            Username = user.Claims.First(i => i.Type == "UserName").Value;
            UserId = user.Claims.First(i => i.Type == "UserId").Value;
        }
        userRepository = serviceProvider.GetService<IUserRepository>();
        authorRepository = serviceProvider.GetService<IAuthorRepository>();
        bookRepository = serviceProvider.GetService<IBookRepository>();
        bookAuthorRepository = serviceProvider.GetService<IBookAuthorRepository>();
        fileClassRepository = serviceProvider.GetService<IFileClassRepository>();


    }
}
