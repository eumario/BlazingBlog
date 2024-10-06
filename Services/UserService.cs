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
            if (hashed != HashPassword(model.Password, salt)) return null; // Login Failed
            return new LoggedInUser(dbUser.Id, $"{dbUser.FirstName} {dbUser.LastName}".Trim()); // Login Success
        }
    }
}
