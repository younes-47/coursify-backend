﻿
using coursify_backend.Models;

namespace coursify_backend.Interfaces.IRepository
{
    public interface IUserRepository
    {
        Task<User> GetByEmailAsync(string email);
        Task<int> GetIdByEmail(string email);
        Task<User?> GetByRefreshToken(string refreshToken);
        Task<bool> IsRegistered(string email);

        Task<bool> IsEmailVerified(string email);
        Task<bool> Add(User user);
        Task<bool> Update(User user);

        Task<int> GetTotalUsers();
        Task<int> GetTotalAdmins();
    }
}
