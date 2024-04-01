using coursify_backend.DTO.GET;
using coursify_backend.Interfaces.IRepository;
using Microsoft.EntityFrameworkCore;

namespace coursify_backend.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly CoursifyContext _context;
        public UserRepository(CoursifyContext context)
        {
            _context = context;
        }

        public async Task<UserCredential> GetCredentialByEmailAsync(string email)
        {
            return await _context.Users
                .Where(u => u.Email == email)
                .Select(u => new UserCredential
                {
                    Email = u.Email,
                    Password = u.Password,
                    Role = u.Role
                })
                .FirstAsync();
        }

        public async Task<bool> IsRegistered(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }
    }
}
