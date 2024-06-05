﻿using Application.Shared;
using Domain.UserEntity;
using Shared;

namespace Application.Commands.UserCommands
{
    public class RegistrationByAdminCommand : Command
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string RoleName { get; set; }
        public bool IsOorganisation { get; set; }
        public override async Task<CommandExecutionResult> ExecuteAsync()
        {
            if (!PasswordHelper.IsValidEmail(Email))
            {
                return await Fail("Invalid email address");
            }

            return await userRepository.Registration(new User()
            {
                UserName = Username,
                PasswordHash = Password,
                Email = Email,
                PNumber = PNumber,
                FirstName = FirstName,
                LastName = LastName,
                PhoneNumber = Phone,
                IsOorganisation = IsOorganisation,
            }, RoleName,false);
        }

    }
}
