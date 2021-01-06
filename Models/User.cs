using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace RLTestTask.Models 
{
    public class User 
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public ICollection<Role> Roles { get; set; }
        public User() {}
        public User(UserDto userDto) 
        {
            this.Id = userDto.Id;
            this.Login = userDto.Login;
            this.Name = userDto.Name;
            this.Email = userDto.Email;
            this.Password = userDto.Password;
        }

        internal void FillDataFromDto(UserDto userDto)
        {
            this.Id = userDto.Id;
            this.Login = userDto.Login;
            this.Name = userDto.Name;
            this.Email = userDto.Email;
            this.Password = userDto.Password;
        }
    }

    public class UserDto
    {
        public int Id { get; set; }
        [Required]
        [StringLength(31)]
        public string Login { get; set; }
        [StringLength(127)]
        public string Name { get; set; }
        [EmailAddress]
        [StringLength(63)]
        public string Email { get; set; }
        [StringLength(63)]
        public string Password { get; set; }

        public IEnumerable<RoleDto> Roles { get; set; }
        public UserDto() { }
        public UserDto(User user) 
        {
            this.Id = user.Id;
            this.Login = user.Login;
            this.Name = user.Name;
            this.Email = user.Email;
            this.Password = "";
            this.Roles = user.Roles == null 
                ? Enumerable.Empty<RoleDto>()
                : user.Roles.Select(r => new RoleDto(r));
        }
    }
}