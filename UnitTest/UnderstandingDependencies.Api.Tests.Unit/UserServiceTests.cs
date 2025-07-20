using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnderstandingDependencies.Api.Context;
using UnderstandingDependencies.Api.Repositories;
using UnderstandingDependencies.Api.Services;

namespace UnderstandingDependencies.Api.Tests.Unit
{
    public class UserServiceTests
    {
     

        private readonly UserRepository _sut;
        private readonly ApplicationDbContext _context;


        public UserServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TestDb") 
                .Options;

            _context = new ApplicationDbContext(options);
            _sut = new UserRepository(_context);
        }

        [Fact]
        public async Task GetAllAsync_ShoulReturnEmptyList_WhenNoUsersExist()
        {
            // Arrange
            // Act
            var users = await _sut.GetAllAsync();
            // Assert
            users.Should().BeEmpty("because no users have been added to the in-memory database yet.");
        }
    }
}
