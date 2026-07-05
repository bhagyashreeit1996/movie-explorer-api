import React from "react";

function MovieCard({
    movie,
    onLike,
    onDetails,
    isLiked
}) {
  return (
    <div className="col-md-4 mb-4">
      <div className="card h-100 shadow border-0">

        <img
          src={movie.poster}
          className="card-img-top rounded-top"
          alt={movie.title}
          style={{
            height: "420px",
            objectFit: "cover"
          }}
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
              className={`btn btn-sm me-2 px-3 ${
                  isLiked ? "btn-secondary" : "btn-success"
              }`}
              disabled={isLiked}
              onClick={() => onLike(movie.movieId)}
          >
              {isLiked ? "❤️ Liked" : "❤️ Like"}
          </button>

          <button
              className="btn btn-outline-primary btn-sm px-3"
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