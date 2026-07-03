import React, { useState } from "react";
import { searchMovies, likeMovie } from "../services/api";

function SearchMovies({ refreshLikes }) {
  const [query, setQuery] = useState("");
  const [movies, setMovies] = useState([]);
  const [loading, setLoading] = useState(false);

  const handleSearch = async () => {
    try {
      if (!query.trim()) {
        alert("Please enter movie name");
        return;
      }

      setLoading(true);

      const result = await searchMovies(query);

      // If API returns paged response
      if (result.data) {
        setMovies(result.data);
      } else {
        setMovies(result);
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

        alert("Unable to like movie.");
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

              <ul className="list-group">

                {movies.map((movie) => (
                  <li
                    key={movie.movieId}
                    className="list-group-item d-flex justify-content-between align-items-center"
                  >
                    <div>
                      <strong>{movie.title}</strong>
                      <br />

                      <small>
                        {movie.year} | {movie.genre}
                      </small>
                    </div>

                    <button
                      className="btn btn-success btn-sm"
                      onClick={() => handleLike(movie.movieId)}
                    >
                      Like
                    </button>

                  </li>
                ))}

              </ul>
            </div>
          )}

        </div>
      </div>

    </div>
  );
}

export default SearchMovies;