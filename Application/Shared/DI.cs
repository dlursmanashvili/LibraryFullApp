
using Domain.AuthorEntity.IRepository;
using Domain.BookAuthorEntity.IRepository;
using Domain.BookEntity.IBookRepository;
using Domain.Entities.FileEntity.IRepository;
using Domain.MailModel.IRepository;
using Domain.RoleEntity;
using Domain.RoleEntity.IRepository;
using Domain.UserEntity;
using Domain.UserEntity.IRepository;
using Infrastructure.DB;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Shared;

public static class DI
{
    public static void DependecyResolver(IServiceCollection services)
    {
        services.AddIdentity<User, ApplicationRole>()
              .AddEntityFrameworkStores<ApplicationDbContext>()
              .AddDefaultTokenProviders();
        services.AddScoped<IQueryExecutor, QueryExecutor>();
        services.AddScoped<ICommandExecutor, CommandExecutor>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ISendNotification, SendNotification>();
        services.AddScoped<IAuthorRepository, AuthorRepository>();
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IBookAuthorRepository, BookAuthorRepository>();
        services.AddScoped<IFileClassRepository, FileClassRepository>();
    }
}
