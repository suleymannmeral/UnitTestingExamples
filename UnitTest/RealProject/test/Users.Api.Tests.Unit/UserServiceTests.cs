using FluentAssertions;
using FluentValidation;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;
using Users.Api.Dtos;
using Users.Api.Logging;
using Users.Api.Models;
using Users.Api.Repositories;
using Users.Api.Services;

namespace Users.Api.Tests.Unit;

public class UserServiceTests
{
    private readonly UserService _sut;
    private readonly IUserRepository _userRepository=Substitute.For<IUserRepository>();
    private readonly ILoggerAdapter<UserService> _logger = Substitute.For<ILoggerAdapter<UserService>>();

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
        _logger.Received(1).LogInformation("Users retrieved in {ElapsedMilliseconds} ms", Arg.Any<long>());

    }

    [Fact]
    public async Task GetAllAsync_ShouldLogMessagesAndException_WhenExceptionIsThrown()
    {
        //Arrange
        var exception = new ArgumentException("An error occurred while retrieving users");
        _userRepository.GetAllASync().Throws(exception);

        //Act
         var requestAction= async ()=> await _sut.GetAllAsync();

        //Assert
        await requestAction.Should()
              .ThrowAsync<ArgumentException>();
        _logger.Received(1).LogError(exception, "An error occurred while retrieving users");

    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_whenNoUserExist()
    {
        // Arrange
        _userRepository.GetByIdAsync(Arg.Any<Guid>()).ReturnsNull();

        // Act
        var result = await _sut.GetByIdAsync(Guid.NewGuid());

        //Assert
        result.Should().BeNull();

    }

    [Fact]
    public async Task GetByIdAsync_ShouldUser_WhenSomeUserExist()
    {
       var existingUser=new User
       {
              Id = Guid.NewGuid(),
              FullName = "Test User"
         };
    
          // Arrange
          _userRepository.GetByIdAsync(existingUser.Id).Returns(existingUser);
    
          // Act
          var result = await _sut.GetByIdAsync(existingUser.Id);
    
          //Assert
          result.Should().BeEquivalentTo(existingUser);

    }

    [Fact]
    public async Task GetByIdAsync_ShouldLogMessages_WhenInvoked()
    {
        //Arrange
        var userId= Guid.NewGuid();
        _userRepository.GetByIdAsync(userId).ReturnsNull();

        //Act
        await _sut.GetByIdAsync(userId);

        //Assert
        _logger.Received(1).LogInformation($"Retrieving user with id:{userId}");
        _logger.Received(1).LogInformation("User retrieved in {ElapsedMilliseconds} ms", Arg.Any<long>());

    }

    [Fact]
    public async Task GetByIdAsync_ShouldLogMessagesAndException_WhenExceptionIsThrown()
    {
        //Arrange
        var userId = Guid.NewGuid();
        var exception = new ArgumentException("Something went wrong ");
        _userRepository.GetByIdAsync(userId).Throws(exception);

        //Act
        var requestAction = async () => await _sut.GetByIdAsync(userId);

        //Assert
        await requestAction.Should()
              .ThrowAsync<ArgumentException>();
        _logger.Received(1).LogError(exception, "An error occurred while retrieving user");

    }

    [Fact]
    public async Task CreateUserAsync_ShouldThrownValidationException_WhenUserFullNameIsNotBeAtLeast3CharatersLong()
    {
        // Arrange
        var invalidUser = new CreateUserDto("a");

        // Act
        var requestAction = async () => await _sut.CreateUserAsync(invalidUser);

        // Assert
        var ex = await Assert.ThrowsAsync<ValidationException>(requestAction);
        ex.Errors.Should().Contain(e => e.ErrorMessage == "Full name must be at least 3 characters long.");

    }

    [Fact]
    public async Task CreateUserAsync_ShouldThrowValidationException_WhenFullNameIsEmpty()
    {
        // Arrange
        var invalidUser = new CreateUserDto(" ");

        // Act
        var requestAction = async () => await _sut.CreateUserAsync(invalidUser);

        // Assert
        var ex = await Assert.ThrowsAsync<ValidationException>(requestAction);
        ex.Errors.Should().Contain(e => e.ErrorMessage == "Full name is required.");
    }

    [Fact]
    public Task CreateUserAsync_ShouldThrownAnError_WhenNameExist()
    {
        // Arrange
        var existingUser = new CreateUserDto("Existing User");
        _userRepository.NameExist(existingUser.FullName).Returns(true);
        // Act
        var requestAction = async () => await _sut.CreateUserAsync(existingUser);
        // Assert
        return requestAction.Should().ThrowAsync<ArgumentException>()
            .WithMessage("User with this name already exists");
    }

    [Fact]
    public void CreateUserAsync_ShouldCreateUserDtoUserObject()
    {
        // Arrange
        var request= new CreateUserDto("New User");

        //Act
        var user=_sut.CreateUserDtoToUserObject(request);

        //Assert
        user.FullName.Should().Be(request.FullName);

    }

    [Fact]
    public async Task CreateAsync_ShouldCreateuser_WhenDetailsAreValidAndUnique()
    {
        //Assert
        var testUser = new CreateUserDto("Test User");
        _userRepository.NameExist(testUser.FullName).Returns(false);
        _userRepository.CreateUserAsync(Arg.Any<User>()).Returns(true);

        //Act

        var result = await _sut.CreateUserAsync(testUser);

        //Assert
        result.Should().BeTrue();

    }

    [Fact]
    public async Task DeleteByIdAsync_ShouldThrowArgumentException_WhenUserNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _userRepository.GetByIdAsync(userId).ReturnsNull();

        // Act
        var requestAction = async () => await _sut.DeleteByIdAsync(userId);

        // Assert
        await requestAction.Should().ThrowAsync<ArgumentException>()
            .WithMessage("User not found");
    }

    [Fact]
    public async Task DeleteByIdAsync_ShouldDeleteUser_WhenUserExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var existingUser = new User { Id = userId, FullName = "Test User" };
        _userRepository.GetByIdAsync(userId).Returns(existingUser);
        _userRepository.DeleteAsync(existingUser).Returns(true);

        // Act
        var result = await _sut.DeleteByIdAsync(userId);

        // Assert
        result.Should().BeTrue();
    }


}
