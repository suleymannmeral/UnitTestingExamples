using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Users.Api.Dtos;
using Users.Api.Logging;
using Users.Api.Models;
using Users.Api.Repositories;
using Users.Api.Validators;

namespace Users.Api.Services
{
    public sealed class UserService(IUserRepository userRepository, ILoggerAdapter<UserService> logger) : IUserService
    {
      
        public async Task<bool> CreateUserAsync(CreateUserDto request, CancellationToken cancellationToken = default)
        {
            CreateUserDtoValidator validator = new ();
            var result = validator.Validate(request);
            if(!result.IsValid)
            {
                throw new ValidationException(string.Join(", ", result.Errors.Select(s => s.ErrorMessage)));
            }

            User user =await userRepository.NameExist(request.FullName, cancellationToken)
                ? throw new ArgumentException("User with this name already exists")
                : new User
                {
                FullName = request.FullName
                };
          
            logger.LogInformation($"Creating user with full name:{request.FullName}");
            var stopWatch=Stopwatch.StartNew();
            try
            {
                return await userRepository.CreateUserAsync(user, cancellationToken);
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "An error occurred while creating user");
                throw;
            }
            finally
            {
                stopWatch.Stop();
                logger.LogInformation($"User created in {stopWatch.ElapsedMilliseconds} ms");
            }
           

        

        }

        public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
           User user = await userRepository.GetByIdAsync(id, cancellationToken);
            if (user is null)
            {
               throw new ArgumentException("User not found");
            }
            logger.LogInformation($"Deleting user with id: {id}");
            var stopWatch = Stopwatch.StartNew();
            try
            {
                return await userRepository.DeleteAsync(user, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while deleting user");
                throw;
            }
            finally
            {
                stopWatch.Stop();
                logger.LogInformation($"User deleted in {stopWatch.ElapsedMilliseconds} ms");
            }
        }

        public async Task<List<User>> GetAllAsync(CancellationToken cancellationToken = default)
        {
         
            logger.LogInformation("Retrieving all users");
            var stopWatch = Stopwatch.StartNew();
            try
            {
                return await userRepository.GetAllASync(cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving users");
                throw;
            }
            finally
            {
                stopWatch.Stop();
                logger.LogInformation("Users retrieved in {ElapsedMilliseconds} ms", stopWatch.ElapsedMilliseconds);

            }

        }

        public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            logger.LogInformation($"Retrieving user with id:{id}");
            var stopWatch = Stopwatch.StartNew();
            try
            {
                return await userRepository.GetByIdAsync(id,cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving user");
                throw;
            }
            finally
            {
                stopWatch.Stop();
                logger.LogInformation("User retrieved in {ElapsedMilliseconds} ms", stopWatch.ElapsedMilliseconds);
            }
        }
    }
}
