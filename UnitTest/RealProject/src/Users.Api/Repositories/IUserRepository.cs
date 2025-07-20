using Users.Api.Dtos;
using Users.Api.Models;

namespace Users.Api.Repositories;

public interface IUserRepository
{
    Task<List<User>> GetAllASync(CancellationToken cancellationToken = default);
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> NameExist(string fullName, CancellationToken cancellationToken = default);
    Task<bool> CreateUserAsync(User user,CancellationToken cancellationToken=default);
    Task<bool>DeleteAsync(User user, CancellationToken cancellationToken = default);
}
