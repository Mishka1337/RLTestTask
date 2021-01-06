using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RLTestTask.Models;

namespace RLTestTask.Services
{
    public interface IRoleService
    {
        public Task<IEnumerable<RoleDto>> GetAllRoles();
    }

    public class RoleService : IRoleService
    {
        private ApplicationContext dbContext;

        public RoleService(ApplicationContext context)
        {
            dbContext = context;
        }
        public async Task<IEnumerable<RoleDto>> GetAllRoles()
        {
            return await dbContext.Roles.Select(r => new RoleDto(r)).ToArrayAsync();
        }
    }
}