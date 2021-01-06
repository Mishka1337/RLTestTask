using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RLTestTask.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<User> Users { get; set; }
        public Role() {}
        public Role (RoleDto roleDto) 
        {
            this.Id = roleDto.Id;
            this.Name = roleDto.Name;
        }
    }

    public class RoleDto
    {
        public int Id { get; set; }
        [Required]
        [StringLength(31)]
        public string Name { get; set; }
        public RoleDto() { }
        public RoleDto(Role role)
        {
            this.Id = role.Id;
            this.Name = role.Name;
        }
    }
}