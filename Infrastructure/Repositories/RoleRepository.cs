using Domain.RoleEntity;
using Domain.RoleEntity.IRepository;
using Infrastructure.DB;
using Microsoft.AspNetCore.Identity;
using Shared;

namespace Infrastructure.Repositories
{
    public class RoleRepository : BaseRepository, IRoleRepository
    {
        public RoleManager<ApplicationRole> _roleManager { get; set; }
        public RoleRepository(ApplicationDbContext applicationDbContext,
            IServiceProvider serviceProvider,
            RoleManager<ApplicationRole> roleManager) : base(applicationDbContext, serviceProvider)
        {
            _roleManager = roleManager;
        }

        public async Task<CommandExecutionResult> AddNewRole(ApplicationRole role)
        {
            try
            {
                if (_ApplicationDbContext.Roles.Any(x => x.Name == role.Name && !x.IsDeleted))
                {
                    return new CommandExecutionResult() { Success = false, ErrorMessage = "როლის_დასახელება_უკვე_გამოყენებულია " };//"როლის დასახელება უკვე გამოყენებულია"
                }

                if (_ApplicationDbContext.Roles.Any(x => x.Name == role.Name && x.IsDeleted))
                {
                    var oldrole = _ApplicationDbContext.Roles.FirstOrDefault(x => x.Name == role.Name && x.IsDeleted);
                    await _roleManager.DeleteAsync(oldrole);
                }

                await _roleManager.CreateAsync(role);
                await _roleManager.UpdateAsync(role);




                return new CommandExecutionResult() { Success = true };
            }
            catch (Exception ex)
            {

                return new CommandExecutionResult()
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task<CommandExecutionResult> EditRoleName(ApplicationRole role, string roleName)
        {
            try
            {
                var oldRecord = new ApplicationRole()
                {
                    Id = role.Id,
                    Name = role.Name,
                    NormalizedName = role.NormalizedName,
                    UpdatedAt = role.UpdatedAt,
                    ConcurrencyStamp = role.ConcurrencyStamp,
                    CreatedAt = role.CreatedAt,
                    DeletedAt = role.DeletedAt,
                    IsDeleted = role.IsDeleted,
                };


                role.Name = roleName;
                role.UpdatedAt = DateTime.Now;
                await _roleManager.UpdateAsync(role);




                return new CommandExecutionResult() { Success = true };
            }
            catch (Exception ex)
            {
                return new CommandExecutionResult()
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }
        public async Task<CommandExecutionResult> DeleteRole(ApplicationRole role)
        {
            try
            {
                role.IsDeleted = true;
                role.DeletedAt = DateTime.Now;

                await _roleManager.UpdateAsync(role);


                return new CommandExecutionResult() { Success = true };
            }
            catch (Exception ex)
            {
                return new CommandExecutionResult()
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }

    
    }
}
