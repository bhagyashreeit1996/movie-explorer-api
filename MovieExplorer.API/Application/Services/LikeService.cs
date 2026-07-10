using MovieExplorer.API.Application.DTOs;
using MovieExplorer.API.Application.Services;
using MovieExplorer.API.Core.Exceptions;
using MovieExplorer.API.Core.Interfaces;
using MovieExplorer.API.Core.Models;
using MovieExplorer.API.DTOs;

public class LikeService : ILikeService
{
    private readonly IMovieRepository _movieRepository;
    private readonly IMovieLikeRepository _movieLikeRepository;
    private readonly IOmdbService _omdbService;

    public LikeService(
            IMovieRepository movieRepository,
            IMovieLikeRepository movieLikeRepository,
            IOmdbService omdbService)
            {
                _movieRepository = movieRepository;
                _movieLikeRepository = movieLikeRepository;
                _omdbService = omdbService;
            }

    public async Task LikeMovieAsync(int userId, string movieId)
    {
        var movie = await _movieRepository.GetByMovieIdAsync(movieId);

        if (movie == null)
        {
            var movieDetails = await _omdbService.GetMovieDetailsAsync(movieId);

            if (movieDetails == null)
                throw new MovieNotFoundException(movieId);

            int year = 0;
            int.TryParse(movieDetails.Year, out year);

            movie = new Movie(
                movieId,
                movieDetails.Title,
                year,
                movieDetails.Genre);

            await _movieRepository.AddAsync(movie);
        }

        var alreadyLiked =
            await _movieLikeRepository.IsLikedAsync(userId, movieId);

        if (alreadyLiked)
            throw new MovieAlreadyLikedException(movieId);

        var like = new MovieLike(userId, movieId);
        await _movieLikeRepository.AddAsync(like);
    }

    public async Task<List<MovieDto>> GetLikedMoviesAsync(int userId)
    {
        var likes = await _movieLikeRepository.GetUserLikesAsync(userId);

        if (!likes.Any())
            return new List<MovieDto>();

        var movieIds = likes.Select(l => l.MovieId).ToList();

        var movies = await _movieRepository
            .GetMoviesByIdsAsync(movieIds);

        return movies.Select(m => new MovieDto
        {
            MovieId = m.MovieId,
            Title = m.Title,
            Year = m.Year,
            Genre = m.Genre
        }).ToList();
    }

    public async Task UnlikeMovieAsync(
    int userId,
    string movieId)
    {
        await _movieLikeRepository
            .RemoveLikeAsync(
                userId,
                movieId);
    }

    public async Task<PagedResponse<MovieDto>>
    GetLikedMoviesAsync(int userId, int pageNumber, int pageSize)
    {
        var (movies, totalCount) =
            await _movieLikeRepository
            .GetLikedMoviesAsync(userId, pageNumber, pageSize);

        return new PagedResponse<MovieDto>
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount,
            Data = movies
        };
    }
}