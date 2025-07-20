using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Users.Api.Dtos;
using Users.Api.Models;
using Users.Api.Repositories;
using Users.Api.Validators;

namespace Users.Api.Services
{
    public sealed class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<User> _logger;

        public UserService(IUserRepository userRepository, ILogger<User> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<bool> CreateUserAsync(CreateUserDto request, CancellationToken cancellationToken = default)
        {
            CreateUserDtoValidator validator = new ();
            var result = validator.Validate(request);
            if(!result.IsValid)
            {
                throw new ValidationException(string.Join(", ", result.Errors.Select(s => s.ErrorMessage)));
            }

            User user =await _userRepository.NameExist(request.FullName, cancellationToken)
                ? throw new ArgumentException("User with this name already exists")
                : new User
                {
                FullName = request.FullName
                };
          
            _logger.LogInformation($"Creating user with full name:{request.FullName}");
            var stopWatch=Stopwatch.StartNew();
            try
            {
                return await _userRepository.CreateUserAsync(user, cancellationToken);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating user");
                throw;
            }
            finally
            {
                stopWatch.Stop();
                _logger.LogInformation($"User created in {stopWatch.ElapsedMilliseconds} ms");
            }
           

        

        }

        public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
           User user = await _userRepository.GetByIdAsync(id, cancellationToken);
            if (user is null)
            {
               throw new ArgumentException("User not found");
            }
            _logger.LogInformation($"Deleting user with id: {id}");
            var stopWatch = Stopwatch.StartNew();
            try
            {
                return await _userRepository.DeleteAsync(user, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting user");
                throw;
            }
            finally
            {
                stopWatch.Stop();
                _logger.LogInformation($"User deleted in {stopWatch.ElapsedMilliseconds} ms");
            }
        }

        public async Task<List<User>> GetAllAsync(CancellationToken cancellationToken = default)
        {
         
            _logger.LogInformation("Retrieving all users");
            var stopWatch = Stopwatch.StartNew();
            try
            {
                return await _userRepository.GetAllASync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving users");
                throw;
            }
            finally
            {
                stopWatch.Stop();
                _logger.LogInformation($"Users retrieved in {stopWatch.ElapsedMilliseconds} ms");
            }

        }

        public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"Retrieving user with id:{id} ");
            var stopWatch = Stopwatch.StartNew();
            try
            {
                return await _userRepository.GetByIdAsync(id,cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving user");
                throw;
            }
            finally
            {
                stopWatch.Stop();
                _logger.LogInformation($"User retrieved in {stopWatch.ElapsedMilliseconds} ms");
            }
        }
    }
}
