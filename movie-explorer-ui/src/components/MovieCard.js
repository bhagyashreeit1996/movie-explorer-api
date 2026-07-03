import React from "react";

function MovieCard({
  movie,
  onLike,
  onDetails
}) {
  return (
    <div className="col-md-4 mb-4">

      <div className="card h-100 shadow-sm border-0">

        <div className="card-body">

          <h5 className="card-title">
            {movie.title}
          </h5>

          <p className="text-muted mb-1">
            📅 {movie.year}
          </p>

          <p>
            {movie.genre}
          </p>

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