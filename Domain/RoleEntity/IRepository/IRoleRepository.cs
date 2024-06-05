using Shared;

namespace Domain.RoleEntity.IRepository;

public interface IRoleRepository
{
    Task<CommandExecutionResult> AddNewRole(ApplicationRole role);
    Task<CommandExecutionResult> EditRoleName(ApplicationRole role, string roleName);
    Task<CommandExecutionResult> DeleteRole(ApplicationRole role);
}
