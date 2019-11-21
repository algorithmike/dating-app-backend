using System;
using System.Threading.Tasks;
using DatingApp.API.Models;


namespace DatingApp.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        private struct SaltyHash
        {
            public byte[] salt;
            public byte[] hash;
        }

        public AuthRepository(DataContext context) { _context = context; }

        // Start - IAuthRepository implementation
        public Task<User> Login(string username, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<User> Register(User user, string password)
        {
            SaltyHash hash = CreatePasswordHash(password);
            user.PasswordHash = hash.hash;
            user.PasswordSalt = hash.salt;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public Task<bool> UserExists(string username)
        {
            throw new NotImplementedException();
        }
        // End - IAuthRepository implementation

        private SaltyHash CreatePasswordHash(string password)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA1())
            {
                SaltyHash retVal;
                retVal.salt = hmac.Key;
                retVal.hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return retVal;
            }
        }
    }
}
