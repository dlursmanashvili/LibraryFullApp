using Application.Shared;
using Domain.RoleEntity;
using FluentValidation;
using FluentValidation.Attributes;
using Shared;

namespace Application.Commands.RoleCommands
{
    [Validator(typeof(AddNewRoleCommandValidation))]
    public class AddNewRoleCommand : Command
    {
        public string RoleName { get; set; }
        public override async Task<CommandExecutionResult> ExecuteAsync()
        {
            if (RoleHelper.CheckRoleName(RoleName)) return await Fail($"გთხოვთ დაარედაკტიროთ როლის სახელი როლის სახელი უნდა შეიცავდეს მხოლოდ ციფრებს ან ლათინურ სიმბოლოებს");

            return await RoleRepository.AddNewRole(new ApplicationRole() { Name = RoleName, NormalizedName = RoleName.ToUpper() });
        }
    }
    public class AddNewRoleCommandValidation : AbstractValidator<AddNewRoleCommand>
    {
        public AddNewRoleCommandValidation()
        {
            RuleFor(x => x.RoleName).NotEmpty();
        }
    }
}
