using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using MovieExplorer.API.Core.DTOs;
using MovieExplorer.API.Core.Interfaces;
using MovieExplorer.API.Core.Models;
using MovieExplorer.API.Core.Services;
using Xunit;

namespace MovieExplorer.Tests.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();

            _configurationMock = new Mock<IConfiguration>();

            _configurationMock.Setup(x => x["Jwt:Key"])
                .Returns("ThisIsAVeryLongSecretKeyForJwtAuthentication123456789");

            _configurationMock.Setup(x => x["Jwt:Issuer"])
                .Returns("MovieExplorer");

            _configurationMock.Setup(x => x["Jwt:Audience"])
                .Returns("MovieExplorerUsers");

            _authService = new AuthService(
                _userRepositoryMock.Object,
                _configurationMock.Object);
        }

        [Fact]
        public async Task RegisterAsync_ShouldRegisterUserSuccessfully()
        {
            // Arrange

            var request = new RegisterRequest
            {
                Name = "John Doe",
                Email = "john@test.com",
                Password = "Password123"
            };

            _userRepositoryMock
                .Setup(x => x.GetByEmailAsync(request.Email))
                .ReturnsAsync((User?)null);

            // Act

            await _authService.RegisterAsync(request);

            // Assert

            _userRepositoryMock.Verify(
                x => x.AddAsync(It.IsAny<User>()),
                Times.Once);

            _userRepositoryMock.Verify(
                x => x.GetByEmailAsync(request.Email),
                Times.Once);
        }

        [Fact]
        public async Task RegisterAsync_ShouldThrowException_WhenUserAlreadyExists()
        {
            // Arrange

            var request = new RegisterRequest
            {
                Name = "John",
                Email = "john@test.com",
                Password = "Password123"
            };

            var existingUser = new User
            {
                UserId = 1,
                Name = "John",
                Email = "john@test.com",
                PasswordHash = "Password123"
            };

            _userRepositoryMock
                .Setup(x => x.GetByEmailAsync(request.Email))
                .ReturnsAsync(existingUser);

            // Act

            Func<Task> act = async () =>
                await _authService.RegisterAsync(request);

            // Assert

            await act.Should()
                .ThrowAsync<Exception>()
                .WithMessage("User already exists.");

            _userRepositoryMock.Verify(
                x => x.AddAsync(It.IsAny<User>()),
                Times.Never);
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnToken_WhenCredentialsAreValid()
        {
            // Arrange

            var request = new LoginRequest
            {
                Email = "john@test.com",
                Password = "Password123"
            };

            var user = new User
            {
                UserId = 1,
                Name = "John",
                Email = "john@test.com",
                PasswordHash = "Password123"
            };

            _userRepositoryMock
                .Setup(x => x.GetByEmailAsync(request.Email))
                .ReturnsAsync(user);

            // Act

            var result = await _authService.LoginAsync(request);

            // Assert

            result.Should().NotBeNull();

            result.Email.Should().Be(request.Email);

            result.Token.Should().NotBeNullOrWhiteSpace();

            _userRepositoryMock.Verify(
                x => x.GetByEmailAsync(request.Email),
                Times.Once);
        }

        [Fact]
        public async Task LoginAsync_ShouldThrowException_WhenUserDoesNotExist()
        {
            // Arrange

            var request = new LoginRequest
            {
                Email = "unknown@test.com",
                Password = "Password123"
            };

            _userRepositoryMock
                .Setup(x => x.GetByEmailAsync(request.Email))
                .ReturnsAsync((User?)null);

            // Act

            Func<Task> act = async () =>
                await _authService.LoginAsync(request);

            // Assert

            await act.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Invalid email or password.");

            _userRepositoryMock.Verify(
                x => x.GetByEmailAsync(request.Email),
                Times.Once);
        }

        [Fact]
        public async Task LoginAsync_ShouldThrowException_WhenPasswordIsIncorrect()
        {
            // Arrange

            var request = new LoginRequest
            {
                Email = "john@test.com",
                Password = "WrongPassword"
            };

            var user = new User
            {
                UserId = 1,
                Name = "John",
                Email = "john@test.com",
                PasswordHash = "Password123"
            };

            _userRepositoryMock
                .Setup(x => x.GetByEmailAsync(request.Email))
                .ReturnsAsync(user);

            // Act

            Func<Task> act = async () =>
                await _authService.LoginAsync(request);

            // Assert

            await act.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Invalid email or password.");
        }

        [Fact]
        public async Task GetCurrentUserAsync_ShouldReturnUserProfile()
        {
            // Arrange

            int userId = 1;

            var user = new User
            {
                UserId = 1,
                Name = "John Doe",
                Email = "john@test.com",
                PasswordHash = "Password123"
            };

            _userRepositoryMock
                .Setup(x => x.GetByIdAsync(userId))
                .ReturnsAsync(user);

            // Act

            var result = await _authService.GetCurrentUserAsync(userId);

            // Assert

            result.Should().NotBeNull();

            result!.UserId.Should().Be(1);
            result.Name.Should().Be("John Doe");
            result.Email.Should().Be("john@test.com");

            _userRepositoryMock.Verify(
                x => x.GetByIdAsync(userId),
                Times.Once);
        }

        [Fact]
        public async Task GetCurrentUserAsync_ShouldReturnNull_WhenUserNotFound()
        {
            // Arrange

            int userId = 999;

            _userRepositoryMock
                .Setup(x => x.GetByIdAsync(userId))
                .ReturnsAsync((User?)null);

            // Act

            var result = await _authService.GetCurrentUserAsync(userId);

            // Assert

            result.Should().BeNull();

            _userRepositoryMock.Verify(
                x => x.GetByIdAsync(userId),
                Times.Once);
        }
    }

}