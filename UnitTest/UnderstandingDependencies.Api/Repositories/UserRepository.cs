using Microsoft.EntityFrameworkCore;
using UnderstandingDependencies.Api.Context;
using UnderstandingDependencies.Api.Models;

namespace UnderstandingDependencies.Api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }
    }
}
