using FluentAssertions;
using NSubstitute;
using UnderstandingDependencies.Api.Models;
using UnderstandingDependencies.Api.Repositories;
using UnderstandingDependencies.Api.Services;

namespace UnderstandingDependencies.Api.Tests.Unit;

public class UserServiceTests
{
    private readonly UserService _sut;
    private readonly IUserRepository _userRepository=Substitute.For<IUserRepository>();


    public UserServiceTests()
    {
        _sut = new(_userRepository);
    }

    [Fact]
    public async Task GetAllAsync_ShoulReturnEmptyList_WhenNoUsersExist()
    {
        _userRepository.GetAllAsync().Returns(Array.Empty<User>());
        // Arrange
        // Act
        var users = await _sut.GetAllUsersAsync();
        // Assert
        users.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAListOfUsrs_WhenUsersExist()
    {
        //Arrange
        var exceptedUsers = new[]
        {
            new User()
            {
                Id=25,
                FullName="Süleyman Meral",
                Age=23,
                DateOfBirthDate= new DateOnly(2002, 06, 13)

            }

        };
        _userRepository.GetAllAsync().Returns(exceptedUsers);
        //Act
        var users=await _sut.GetAllUsersAsync();
        //Assert
        users.Should().ContainSingle(x => x.FullName == "Süleyman Meral");

    }
}
