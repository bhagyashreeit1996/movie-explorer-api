import React, { useEffect, useState } from "react";
import { getMovieDetails } from "../services/api";

function MovieDetails({ movieId, onClose }) {
  const [movie, setMovie] = useState(null);

  useEffect(() => {
    fetchMovie();
  }, []);

  const fetchMovie = async () => {
    const result = await getMovieDetails(movieId);
    setMovie(result);
  };

  if (!movie)
    return <p>Loading...</p>;

  return (
    <div className="card mt-4">

      <div className="card-body">

        <button
          className="btn btn-secondary float-end"
          onClick={onClose}
        >
          Close
        </button>

        <h2>{movie.title}</h2>

        <img
          src={movie.poster}
          alt={movie.title}
          width="250"
        />

        <p><b>Year:</b> {movie.year}</p>

        <p><b>Genre:</b> {movie.genre}</p>

        <p><b>Director:</b> {movie.director}</p>

        <p><b>Actors:</b> {movie.actors}</p>

        <p><b>IMDb Rating:</b> ⭐ {movie.imdbRating}</p>

        <p><b>Runtime:</b> {movie.runtime}</p>

        <p><b>Language:</b> {movie.language}</p>

        <p><b>Plot:</b></p>

        <p>{movie.plot}</p>

      </div>
    </div>
  );
}

export default MovieDetails;