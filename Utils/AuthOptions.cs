using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace RLTestTask.Utils
{
    public class AuthOptions 
    {
        public const string ISSUER = "RLUserServer";
        public const string AUDIENCE = "RLUserClient";
        private const string KEY = "qwertyqwertyqwertyqwertyqwertyqwerty";
        public const int LIFETIME = 5 * 60;
        public static SymmetricSecurityKey SymmetricSecurityKey => new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
    }
}