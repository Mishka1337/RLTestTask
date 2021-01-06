using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RLTestTask.Models;
using RLTestTask.Utils;

namespace RLTestTask.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> Authenticate(string login, string password);
    }

    public class AuthService : IAuthService
    {
        private IPasswordHasher<User> passwordHasher;
        private ApplicationContext dbContext;

        public AuthService(IPasswordHasher<User> passwordHasher, ApplicationContext context)
        {
            this.passwordHasher = passwordHasher;
            this.dbContext = context;
        }
        public async Task<AuthResponse> Authenticate(string login, string password)
        {
            ClaimsIdentity identity = await GetIdentity(login, password);
            if (identity == null)
            {
                return null;
            }
            DateTime now = DateTime.Now;
            JwtSecurityToken jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(AuthOptions.SymmetricSecurityKey, SecurityAlgorithms.HmacSha256
            ));
            string encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return new AuthResponse(encodedJwt,
                                    identity.Name,
                                    identity
                                        .Claims
                                        .Where(c => c.Type == ClaimTypes.Role)
                                        .Select(c => c.Value));
        }
        private async Task<ClaimsIdentity> GetIdentity(string login, string password)
        {
            User user = await dbContext.Users.FirstOrDefaultAsync(u => u.Login == login);
            if (user == null 
                || passwordHasher.VerifyHashedPassword(user,user.Password,password) == PasswordVerificationResult.Failed)
            {
                return null;
            }
            await dbContext.Entry(user).Collection(u => u.Roles).LoadAsync();
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, user.Login));
            foreach(Role role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,"Token");
            return claimsIdentity;
        }
    }
    
    public class AuthResponse
    {
        public string access_token { get; set; }
        public string login { get; set; }
        public IEnumerable<string> roles {get; set;}
        public AuthResponse() { }

        public AuthResponse(string token, string login, IEnumerable<string> roles)
        {
            this.access_token = token;
            this.login = login;
            this.roles = roles;
        }
    }
}
