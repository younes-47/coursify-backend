using AutoMapper;
using coursify_backend.DTO.INTERNAL;
using coursify_backend.DTO.POST;
using coursify_backend.Interfaces.IRepository;
using coursify_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace coursify_backend.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly CoursifyContext _context;
        private readonly IMapper _mapper;

        public UserRepository(CoursifyContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users
                .Where(u => u.Email == email)
                .FirstAsync();
        }   

        //public async Task<User?> GetByRefreshToken(string refreshToken)
        //{
        //    return await _context.Users
        //        .Where(u => u.RefreshToken == refreshToken)
        //        .FirstOrDefaultAsync();
        //}

        public async Task<bool> IsRegistered(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> IsEmailVerified(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email && u.EmailVerifiedAt != null);
        }

        public async Task<bool> Add(User user)
        {
            await _context.Users.AddAsync(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Update(User user)
        {
            _context.Users.Update(user);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
