using Users.Api.Dtos;
using Users.Api.Models;

namespace Users.Api.Services
{
    public interface IUserService
    {
        Task<List<User>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<bool> CreateUserAsync(CreateUserDto createUserDto, CancellationToken cancellationToken = default);
        Task<bool> DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
