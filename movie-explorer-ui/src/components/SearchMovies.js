import React, { useState } from "react";
import { searchMovies, likeMovie } from "../services/api";
import MovieDetails from "./MovieDetails";
import MovieCard from "./MovieCard";

function SearchMovies({ refreshLikes, onSearchCompleted }) {
  const [query, setQuery] = useState("");
  const [movies, setMovies] = useState([]);
  const [loading, setLoading] = useState(false);
  const [selectedMovieId, setSelectedMovieId] = useState(null);

  const handleSearch = async () => {
      try {
        if (!query.trim()) {
          alert("Please enter movie name");
          return;
        }

        setLoading(true);

        const result = await searchMovies(query);

        const movieList = result.data ? result.data : result;

        // Update the UI
        setMovies(movieList);

        // Update dashboard
        if (onSearchCompleted) {
          onSearchCompleted(movieList.length, 1);
        }

      } catch (error) {
        console.error(error);
        alert("Error searching movies");
      } finally {
        setLoading(false);
      }
    };

  const handleLike = async (movieId) => {
    try {
        await likeMovie(movieId);

        alert("Movie liked successfully.");

        refreshLikes();
    }
    catch (error) {
        console.error(error);

        alert("Movie already liked.");
    }
};

  return (
    <div className="container mt-3">

      <div className="card shadow-sm">
        <div className="card-body">

          <h3 className="card-title mb-3">
            Search Movies
          </h3>

          <div className="input-group mb-3">

            <input
              type="text"
              className="form-control"
              placeholder="Enter movie title..."
              value={query}
              onChange={(e) => setQuery(e.target.value)}
            />

            <button
              className="btn btn-primary"
              onClick={handleSearch}
            >
              Search
            </button>

          </div>

          {loading && (
            <div className="alert alert-info">
              Loading movies...
            </div>
            )}

            {movies.length > 0 && (
              <div>
                <h5>Search Results</h5>

                <div className="row">

                  {movies.map((movie) => (

                    <MovieCard
                        key={movie.movieId}
                        movie={movie}
                        onLike={handleLike}
                        onDetails={setSelectedMovieId}
                    />
                  ))}

                </div>

            </div>
          )}

        </div>
      </div>

      {selectedMovieId && (
        <MovieDetails
          movieId={selectedMovieId}
          onClose={() => setSelectedMovieId(null)}
        />
      )}

    </div>
  );
}

export default SearchMovies;