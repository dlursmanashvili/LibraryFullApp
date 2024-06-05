using Domain.RoleEntity.IRepository;
using Domain.UserEntity;
using Domain.UserEntity.IRepository;
using Infrastructure.DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Shared;

namespace Application.Shared;

public abstract class Query<TQueryResult> where TQueryResult : class
{
    protected ApplicationDbContext _appContext;
    protected IServiceProvider? ServiceProvider;
    protected UserManager<User> _userManager;
    protected IRoleRepository RoleRepository;
    protected IUserRepository userRepository;

    protected string? UserId;
    protected string? Username;
    protected ApplicationDbContext ApplicationContext
    {
        get { return _appContext; }
    }
    public abstract Task<QueryExecutionResult<TQueryResult>> Execute();


    public void Resolve(ApplicationDbContext appContext, IServiceProvider serviceProvider)
    {
        var user = serviceProvider.GetService<IHttpContextAccessor>().HttpContext.User; // <-- Исправление здесь
        _appContext = appContext;
        ServiceProvider = serviceProvider;
        _userManager = ServiceProvider.GetService<UserManager<User>>();


        if (user.Claims.Any())
        {
            Username = user.Claims.FirstOrDefault(i => i.Type == "UserName").Value;
            UserId = user.Claims.FirstOrDefault(i => i.Type == "UserId").Value;
        }

        userRepository = serviceProvider.GetService<IUserRepository>();

    }

    protected Task<QueryExecutionResult<TQueryResult>> Ok(TQueryResult data)
    {
        var result = new QueryExecutionResult<TQueryResult>
        {
            Data = data,
            Success = true
        };

        return Task.FromResult(result);
    }
    protected Task<QueryExecutionResult<TQueryResult>> Fail(params string[] errorMessages)
    {
        var result = new QueryExecutionResult<TQueryResult>
        {
            Success = false
        };

        if (errorMessages != null)
        {
            result.Errors = errorMessages.Select(x => new Error
            {
                Code = 0,
                Message = x
            });
        }

        return Task.FromResult(result);
    }
}
