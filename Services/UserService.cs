using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace BlazingBlog.Services
{
    public class UserService
    {
        private readonly BlogContext _context;

        public UserService(BlogContext context)
        {
            _context = context;
        }

        public async Task<LoggedInUser?> LoginAsync(LoginModel model)
        {
            var dbUser = await _context.Users
                            .AsNoTracking()
                            .FirstOrDefaultAsync(u => u.Email == model.Username);
            if (dbUser is null) return null;
            var salt = dbUser.Salt;
            var hashed = dbUser.Hash;
            if (hashed != model.Password?.HashPassword(salt!)) return null; // Login Failed
            return new LoggedInUser(dbUser.Id, $"{dbUser.FirstName} {dbUser.LastName}".Trim()); // Login Success
        }

        public async Task<bool> CreateUser(RegisterUser model)
        {
            var salt = PasswordUtility.GenerateRandomSalt();
            var user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Salt = salt,
                Hash = model.Password?.HashPassword(salt)
            };
            var res = await _context.Users.AddAsync(user);
            var res2 = (res.State == EntityState.Added);
            var res3 = await _context.SaveChangesAsync();
            return res2 && res3 == 1;
        }
        
        public async Task<bool> IsBlogSetup() => await _context.Users.AnyAsync();
    }
}
