

using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Users.Api.Models;
using Users.Api.Repositories;
using Users.Api.Services;

namespace Users.Api.Tests.Unit;

public class UserServiceTests
{
    private readonly UserService _sut;
    private readonly IUserRepository _userRepository=Substitute.For<IUserRepository>();
    private readonly ILogger<User> _logger=Substitute.For<ILogger<User>>();

    public UserServiceTests()
    {
        _sut = new(_userRepository, _logger);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoUsersExist()
    {
        // Arrenge
        _userRepository.GetAllASync().Returns(Enumerable.Empty<User>().ToList());

        // Act
        var result= await _sut.GetAllAsync();

        //Assert
        result.Should().BeEmpty();
    }

    [Fact]  
    public async Task GetAllAsync_ShouldReturnUserList_WhenSomeUsersExist()
    {
        //Arrange
        var testUser = new User { FullName = "Test User" ,Id=Guid.NewGuid()};

        var expectedUsers = new  List<User>()
        {
            testUser
        };
        _userRepository.GetAllASync().Returns(expectedUsers.ToList());

        // Act
        var result = await _sut.GetAllAsync();

        //assert
        result.Should().BeEquivalentTo(expectedUsers);
    }

    [Fact]
    public async Task GetAllAsync_ShouldLogMessages_WhenInvoked()
    {
        //Arrange
        _userRepository.GetAllASync().Returns(Enumerable.Empty<User>().ToList());

        //Act
        await _sut.GetAllAsync();

        //Assert
        _logger.Received(1).LogInformation(Arg.Is("Retrieving all users"));
        _logger.Received(1).LogInformation(Arg.Is("Retrieved all users in {ElapsedMilliseconds} ms"), Arg.Any<long>());
    }


}
