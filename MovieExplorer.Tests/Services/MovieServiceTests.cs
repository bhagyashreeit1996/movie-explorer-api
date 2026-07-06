using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using MovieExplorer.API.Application.DTOs;
using MovieExplorer.API.Application.Services;
using MovieExplorer.API.Core.Interfaces;
using MovieExplorer.API.Core.Models;
using MovieExplorer.API.DTOs;
using Xunit;

namespace MovieExplorer.Tests.Services
{
    public class MovieServiceTests
    {
        private readonly Mock<IMovieRepository> _movieRepositoryMock;
        private readonly MovieService _movieService;

        public MovieServiceTests()
        {
            _movieRepositoryMock = new Mock<IMovieRepository>();

            _movieService = new MovieService(_movieRepositoryMock.Object);
        }

        [Fact]
        public async Task SearchMoviesAsync_ShouldReturnPagedMovies()
        {
            // Arrange

            var request = new SearchMoviesRequest
            {
                Query = "Avengers",
                PageNumber = 1,
                PageSize = 5,
                SortBy = "Title",
                IsDescending = false
            };

            var movies = new List<Movie>
            {
                new Movie(
                    "tt0848228",
                    "The Avengers",
                    2012,
                    "Action")
            };

            _movieRepositoryMock
                .Setup(x => x.SearchAsync(
                    request.Query,
                    request.PageNumber,
                    request.PageSize,
                    request.SortBy,
                    request.IsDescending))
                .ReturnsAsync((movies, 1));

            //Act

            var result = await _movieService.SearchMoviesAsync(request);

            // Assert

            result.Should().NotBeNull();

            result.TotalCount.Should().Be(1);

            result.PageNumber.Should().Be(1);

            result.PageSize.Should().Be(5);

            result.Data.Should().HaveCount(1);

            var movie = result.Data.First();

            movie.Title.Should().Be("The Avengers");
            movie.MovieId.Should().Be("tt0848228");
            movie.Year.Should().Be(2012);
            movie.Genre.Should().Be("Action");

            _movieRepositoryMock.Verify(
                x => x.SearchAsync(
                    request.Query,
                    request.PageNumber,
                    request.PageSize,
                    request.SortBy,
                    request.IsDescending),
                Times.Once);

        }

        [Fact]
        public async Task SearchMoviesAsync_ShouldThrowArgumentException_WhenQueryIsEmpty()
        {
            // Arrange
            var request = new SearchMoviesRequest
            {
                Query = "",
                PageNumber = 1,
                PageSize = 5,
                SortBy = "Title",
                IsDescending = false
            };

            // Act
            Func<Task> act = async () =>
                await _movieService.SearchMoviesAsync(request);

            // Assert
            await act.Should()
                .ThrowAsync<ArgumentException>()
                .WithMessage("Search query is required.");

            _movieRepositoryMock.Verify(
                x => x.SearchAsync(
                    It.IsAny<string>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>()),
                Times.Never);
        }

        [Fact]
        public async Task GetRecommendedMoviesAsync_ShouldReturnRecommendations()
        {
            // Arrange

            int userId = 1;

            var recommendedMovies = new List<Movie>
    {
        new Movie(
            "tt4154796",
            "Avengers: Endgame",
            2019,
            "Action"),

        new Movie(
            "tt4154756",
            "Avengers: Infinity War",
            2018,
            "Action")
    };

            _movieRepositoryMock
                .Setup(x => x.GetRecommendedMoviesAsync(userId))
                .ReturnsAsync(recommendedMovies);

            // Act

            var result = await _movieService.GetRecommendedMoviesAsync(userId);

            // Assert

            result.Should().NotBeNull();

            result.Should().HaveCount(2);

            result[0].MovieId.Should().Be("tt4154796");
            result[0].Title.Should().Be("Avengers: Endgame");
            result[0].Year.Should().Be(2019);
            result[0].Genre.Should().Be("Action");

            result[1].MovieId.Should().Be("tt4154756");
            result[1].Title.Should().Be("Avengers: Infinity War");

            _movieRepositoryMock.Verify(
                x => x.GetRecommendedMoviesAsync(userId),
                Times.Once);
        }

        [Fact]
        public async Task GetMovieSuggestionsAsync_ShouldReturnSuggestions()
        {
            // Arrange

            var query = "Aven";

            var suggestions = new List<string>
    {
        "Avengers",
        "Avengers: Endgame",
        "Avengers: Infinity War"
    };

            _movieRepositoryMock
                .Setup(x => x.GetMovieSuggestionsAsync(query))
                .ReturnsAsync(suggestions);

            // Act

            var result = await _movieService.GetMovieSuggestionsAsync(query);

            // Assert

            result.Should().NotBeNull();

            result.Should().HaveCount(3);

            result.Should().Contain("Avengers");

            result.Should().Contain("Avengers: Endgame");

            result.Should().Contain("Avengers: Infinity War");

            _movieRepositoryMock.Verify(
                x => x.GetMovieSuggestionsAsync(query),
                Times.Once);
        }
    }
}
