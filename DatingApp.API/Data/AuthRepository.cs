using System;
using System.Threading.Tasks;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;

        private struct SaltyHash
        {
            public byte[] salt, hash;

            public SaltyHash(byte[] x, byte[] y)
            {
                salt = x;
                hash = y;
            }
        }

        public AuthRepository(DataContext context) { _context = context; }

        // Start - IAuthRepository implementation
        public async Task<User> Login(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
            var saltyHash = new SaltyHash(user.PasswordSalt, user.PasswordHash);

            if (user == null) return null;

            if (!VerifyPasswordHash(password, saltyHash)) return null;

            return user;
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

        public async Task<bool> UserExists(string username)
        {
            return await _context.FindAsync<string>(username) != null;
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

        private bool VerifyPasswordHash(string password, SaltyHash saltyHash)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA1(saltyHash.salt))
            {
                var computedHash = System.Text.Encoding.UTF8.GetBytes(password);

                for(var x = 0; x++ < saltyHash.hash.Length;)
                {
                    if (computedHash[x] != saltyHash.hash[x]) return false;
                }
            }
            return true;
        }
    }
}
