using FluentAssertions;
using Moq;
using MovieExplorer.API.Core.Interfaces;
using MovieExplorer.API.Core.Models;
using MovieExplorer.API.Application.DTOs;
using Xunit;
using MovieExplorer.API.Core.Exceptions;
namespace MovieExplorer.Tests.Services
{
    public class LikeServiceTests
    {
        private readonly Mock<IMovieRepository> _movieRepositoryMock;
        private readonly Mock<IMovieLikeRepository> _movieLikeRepositoryMock;
        private readonly LikeService _likeService;

        public LikeServiceTests()
        {
            _movieRepositoryMock = new Mock<IMovieRepository>();

            _movieLikeRepositoryMock =
                new Mock<IMovieLikeRepository>();

            _likeService = new LikeService(
                _movieRepositoryMock.Object,
                _movieLikeRepositoryMock.Object);
        }

        [Fact]
        public async Task LikeMovieAsync_ShouldLikeMovieSuccessfully()
        {
            // Arrange

            int userId = 1;

            string movieId = "tt0848228";

            var movie = new Movie(
                movieId,
                "The Avengers",
                2012,
                "Action");

            _movieRepositoryMock
                .Setup(x => x.GetByMovieIdAsync(movieId))
                .ReturnsAsync(movie);

            _movieLikeRepositoryMock
                .Setup(x => x.IsLikedAsync(userId, movieId))
                .ReturnsAsync(false);

            // Act

            await _likeService.LikeMovieAsync(
                userId,
                movieId);

            // Assert

            _movieLikeRepositoryMock.Verify(
                x => x.AddAsync(It.IsAny<MovieLike>()),
                Times.Once);

            _movieLikeRepositoryMock.Verify(
                x => x.IsLikedAsync(userId, movieId),
                Times.Once);

            _movieRepositoryMock.Verify(
                x => x.GetByMovieIdAsync(movieId),
                Times.Once);
        }

        [Fact]
        public async Task LikeMovieAsync_ShouldThrowMovieAlreadyLikedException_WhenMovieAlreadyLiked()
        {
            // Arrange

            int userId = 1;
            string movieId = "tt0848228";

            var movie = new Movie(
                movieId,
                "The Avengers",
                2012,
                "Action");

            _movieRepositoryMock
                .Setup(x => x.GetByMovieIdAsync(movieId))
                .ReturnsAsync(movie);

            _movieLikeRepositoryMock
                .Setup(x => x.IsLikedAsync(userId, movieId))
                .ReturnsAsync(true);

            // Act

            Func<Task> act = async () =>
                await _likeService.LikeMovieAsync(userId, movieId);

            // Assert

            await act.Should()
                .ThrowAsync<MovieAlreadyLikedException>();

            _movieLikeRepositoryMock.Verify(
                x => x.AddAsync(It.IsAny<MovieLike>()),
                Times.Never);
        }

        [Fact]
        public async Task LikeMovieAsync_ShouldThrowMovieNotFoundException_WhenMovieDoesNotExist()
        {
            // Arrange

            int userId = 1;
            string movieId = "tt9999999";

            _movieRepositoryMock
                .Setup(x => x.GetByMovieIdAsync(movieId))
                .ReturnsAsync((Movie?)null);

            // Act

            Func<Task> act = async () =>
                await _likeService.LikeMovieAsync(userId, movieId);

            // Assert

            await act.Should()
                .ThrowAsync<MovieNotFoundException>();

            _movieRepositoryMock.Verify(
                x => x.GetByMovieIdAsync(movieId),
                Times.Once);

            _movieLikeRepositoryMock.Verify(
                x => x.IsLikedAsync(It.IsAny<int>(), It.IsAny<string>()),
                Times.Never);

            _movieLikeRepositoryMock.Verify(
                x => x.AddAsync(It.IsAny<MovieLike>()),
                Times.Never);
        }
    }
}