using System.Diagnostics;
using UnderstandingDependencies.Api.Models;
using UnderstandingDependencies.Api.Repositories;

namespace UnderstandingDependencies.Api.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _logger=new Logger<UserService>(new LoggerFactory());
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            _logger.LogInformation("Fetching all users from the repository.");
            var stopWatch = Stopwatch.StartNew();
            try
            {
                var users = await _userRepository.GetAllAsync();
                return users;
            }

            finally
            {
                stopWatch.Stop();
                _logger.LogInformation($"Fetched {stopWatch.ElapsedMilliseconds} ms to retrieve users.");
            }

        }


    }
}
