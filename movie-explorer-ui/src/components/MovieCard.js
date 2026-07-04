import React from "react";

function MovieCard({ movie, onLike, onDetails }) {
  return (
    <div className="col-md-4 mb-4">
      <div className="card h-100 shadow border-0">

        <img
          src={movie.poster}
          className="card-img-top"
          alt={movie.title}
          style={{ height: "420px", objectFit: "cover" }}
        />

        <div className="card-body">

          <h5 className="card-title">
            {movie.title}
          </h5>

          <p className="text-muted">
            📅 {movie.year}
          </p>

          <span className="badge bg-warning text-dark mb-2">
            ⭐ IMDb {movie.imdbRating}
          </span>

          <div className="mb-3">
            {movie.genre.split(",").map((genre) => (
              <span
                key={genre}
                className="badge bg-primary me-1"
              >
                {genre.trim()}
              </span>
            ))}
          </div>

        </div>

        <div className="card-footer bg-white">

          <button
            className="btn btn-success btn-sm me-2"
            onClick={() => onLike(movie.movieId)}
          >
            ❤️ Like
          </button>

          <button
            className="btn btn-primary btn-sm"
            onClick={() => onDetails(movie.movieId)}
          >
            📖 Details
          </button>

        </div>

      </div>
    </div>
  );
}

export default MovieCard;