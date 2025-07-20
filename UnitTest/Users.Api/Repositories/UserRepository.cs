using Microsoft.EntityFrameworkCore;
using Users.Api.Context;
using Users.Api.Models;

namespace Users.Api.Repositories;

public sealed class UserRepository(ApplicationDbContext context) : IUserRepository
{

    public async Task<bool> CreateUserAsync(User user, CancellationToken cancellationToken = default)
    {
       await context.Users.AddAsync(user, cancellationToken);
       var result= await context.SaveChangesAsync(cancellationToken);
        return result> 0;
    }

    public async Task<bool> DeleteAsync(User user, CancellationToken cancellationToken = default)
    {
       context.Users.Remove(user);
        var result = await context.SaveChangesAsync(cancellationToken);
        return result > 0;
    }

    public async Task<List<User>> GetAllASync(CancellationToken cancellationToken = default)
    {
       return await context.Users.ToListAsync(cancellationToken);
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
       return await context.Users
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<bool> NameExist(string fullName, CancellationToken cancellationToken = default)
    {
        return await context.Users.Where(u => u.FullName == fullName)
            .AnyAsync(cancellationToken);

    }
}