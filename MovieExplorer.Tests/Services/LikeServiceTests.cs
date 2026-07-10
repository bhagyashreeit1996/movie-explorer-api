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
        private readonly Mock<IOmdbService> _omdbServiceMock;

        public LikeServiceTests()
        {
            _movieRepositoryMock = new Mock<IMovieRepository>();
            _movieLikeRepositoryMock = new Mock<IMovieLikeRepository>();
            _omdbServiceMock = new Mock<IOmdbService>();

            _likeService = new LikeService(
                _movieRepositoryMock.Object,
                _movieLikeRepositoryMock.Object,
                _omdbServiceMock.Object);
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

        [Fact]
        public async Task UnlikeMovieAsync_ShouldRemoveMovieLike()
        {
            // Arrange

            int userId = 1;
            string movieId = "tt0848228";

            // Act

            await _likeService.UnlikeMovieAsync(userId, movieId);

            // Assert

            _movieLikeRepositoryMock.Verify(
                x => x.RemoveLikeAsync(userId, movieId),
                Times.Once);
        }

        [Fact]
        public async Task GetLikedMoviesAsync_ShouldReturnLikedMovies()
        {
            // Arrange

            int userId = 1;

            var likes = new List<MovieLike>
    {
        new MovieLike(userId, "tt0848228"),
        new MovieLike(userId, "tt4154796")
    };

            var movies = new List<Movie>
    {
        new Movie(
            "tt0848228",
            "The Avengers",
            2012,
            "Action"),

        new Movie(
            "tt4154796",
            "Avengers: Endgame",
            2019,
            "Action")
    };

            _movieLikeRepositoryMock
                .Setup(x => x.GetUserLikesAsync(userId))
                .ReturnsAsync(likes);

            _movieRepositoryMock
                .Setup(x => x.GetMoviesByIdsAsync(
                    It.IsAny<List<string>>()))
                .ReturnsAsync(movies);

            // Act

            var result = await _likeService.GetLikedMoviesAsync(userId);

            // Assert

            result.Should().NotBeNull();

            result.Should().HaveCount(2);

            result[0].Title.Should().Be("The Avengers");

            result[1].Title.Should().Be("Avengers: Endgame");

            _movieLikeRepositoryMock.Verify(
                x => x.GetUserLikesAsync(userId),
                Times.Once);

            _movieRepositoryMock.Verify(
                x => x.GetMoviesByIdsAsync(It.IsAny<List<string>>()),
                Times.Once);
        }

        [Fact]
        public async Task GetLikedMoviesAsync_ShouldReturnEmptyList_WhenNoMoviesLiked()
        {
            // Arrange

            int userId = 1;

            _movieLikeRepositoryMock
                .Setup(x => x.GetUserLikesAsync(userId))
                .ReturnsAsync(new List<MovieLike>());

            // Act

            var result = await _likeService.GetLikedMoviesAsync(userId);

            // Assert

            result.Should().NotBeNull();

            result.Should().BeEmpty();

            _movieLikeRepositoryMock.Verify(
                x => x.GetUserLikesAsync(userId),
                Times.Once);

            _movieRepositoryMock.Verify(
                x => x.GetMoviesByIdsAsync(It.IsAny<List<string>>()),
                Times.Never);
        }

        [Fact]
        public async Task GetLikedMoviesAsync_ShouldReturnPagedLikedMovies()
        {
            // Arrange

            int userId = 1;
            int pageNumber = 1;
            int pageSize = 5;

            var likedMovies = new List<MovieDto>
    {
        new MovieDto
        {
            MovieId = "tt0848228",
            Title = "The Avengers",
            Year = 2012,
            Genre = "Action"
        }
    };

            _movieLikeRepositoryMock
                .Setup(x => x.GetLikedMoviesAsync(
                    userId,
                    pageNumber,
                    pageSize))
                .ReturnsAsync((likedMovies, 1));

            // Act

            var result = await _likeService.GetLikedMoviesAsync(
                userId,
                pageNumber,
                pageSize);

            // Assert

            result.Should().NotBeNull();

            result.TotalCount.Should().Be(1);

            result.PageNumber.Should().Be(1);

            result.PageSize.Should().Be(5);

            result.Data.Should().HaveCount(1);

            result.Data.First().Title.Should().Be("The Avengers");

            _movieLikeRepositoryMock.Verify(
                x => x.GetLikedMoviesAsync(
                    userId,
                    pageNumber,
                    pageSize),
                Times.Once);
        }

    }
}