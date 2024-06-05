using Domain.UserEntity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Interface
{
    public class AuthoriseHelper : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly string[] _roles;

        public AuthoriseHelper(params string[] roles)
        {
            _roles = roles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var userManager = context.HttpContext.RequestServices.GetService<UserManager<User>>();
            var user = userManager.GetUserAsync(context.HttpContext.User).Result;

            if (user != null && _roles.Any(role => userManager.IsInRoleAsync(user, role).Result))
            {
                return;
            }

            context.Result = new ForbidResult();
        }
    }
}
