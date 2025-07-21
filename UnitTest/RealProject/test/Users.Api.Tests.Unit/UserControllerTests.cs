using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Users.Api.Controllers;
using Users.Api.Dtos;
using Users.Api.Models;
using Users.Api.Services;

namespace Users.Api.Tests.Unit;

public class UserControllerTests
{
    private readonly UsersController _sut;
    private readonly IUserService _userService = Substitute.For<IUserService>();

    public UserControllerTests()
    {
        _sut = new(_userService);
    }

    [Fact]

    public async Task GetAll_ShouldReturnUsers()
    {
        //Arrange
        _userService.GetAllAsync(Arg.Any<CancellationToken>())
            .Returns(new List<User> { new() { FullName = "Test User", Id = Guid.NewGuid() } });

        // Act

        var result= (OkObjectResult)await _sut.GetAll(CancellationToken.None);

        //Assert

        result.StatusCode.Should().Be(200);



    }

    [Fact]
    public async Task GetById_ShouldReturnUser()
    {
        //Arrange
        var userId = Guid.NewGuid();
        var user = new User { FullName = "Test User", Id = userId };
        _userService.GetByIdAsync(userId, Arg.Any<CancellationToken>()).Returns(user);
        // Act
        var result = (OkObjectResult)await _sut.GetById(userId, CancellationToken.None);
        //Assert
        result.StatusCode.Should().Be(200);
        result.Value.Should().BeEquivalentTo(user);
    }

    [Fact]
    public async Task Create_ShouldReturnCreatedUser()
    {
        //Arrange
        var createUserDto = new CreateUserDto("Test user");
        _userService.CreateUserAsync(createUserDto, Arg.Any<CancellationToken>()).Returns(true);
        
        // Act
        var result = (OkObjectResult)await _sut.Create(createUserDto, CancellationToken.None);
        
        //Assert
        result.StatusCode.Should().Be(200);
        result.Value.Should().BeEquivalentTo(new { Result = true });
    }

    [Fact]
    public async Task DeleteById_ShouldReturnTrue()
    {
        //Arrange
        var userId = Guid.NewGuid();
        _userService.DeleteByIdAsync(userId, Arg.Any<CancellationToken>()).Returns(true);
        
        // Act
        var result = (OkObjectResult)await _sut.DeleteById(userId, CancellationToken.None);
        
        //Assert
        result.StatusCode.Should().Be(200);
        result.Value.Should().BeEquivalentTo(new { Result = true });
    }
}
