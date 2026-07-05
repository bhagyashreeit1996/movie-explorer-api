import React, { useState, useEffect } from "react";
import { searchMovies, likeMovie } from "../services/api";
import MovieDetails from "./MovieDetails";
import MovieCard from "./MovieCard";
import { toast } from "react-toastify";
import { getMovieSuggestions } from "../services/api";

function SearchMovies({ refreshLikes, onSearchCompleted }) {
  const [query, setQuery] = useState("");
  const [debouncedQuery, setDebouncedQuery] = useState("");
  const [suggestions, setSuggestions] = useState([]);
  const [movies, setMovies] = useState([]);
  const [likedMovies, setLikedMovies] = useState([]);
  const [loading, setLoading] = useState(false);
  const [selectedMovieId, setSelectedMovieId] = useState(null);
  
  useEffect(() => {

      const timer = setTimeout(() => {

          setDebouncedQuery(query);

      }, 300);

      return () => clearTimeout(timer);

  }, [query]);

  useEffect(() => {

        if (debouncedQuery.trim().length < 2) {
            setSuggestions([]);
            return;
        }

        const fetchSuggestions = async () => {

            try {

                const result =
                    await getMovieSuggestions(debouncedQuery);

                setSuggestions(result);

            }
            catch (error) {
                console.error(error);
            }

        };

        fetchSuggestions();

    }, [debouncedQuery]);

  const handleSearch = async () => {
      try {
        if (!query.trim()) {
          toast.warning("Please enter a movie name.");
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
        toast.error("Error searching movies.");
      } finally {
        setLoading(false);
      }
    };

  const loadSuggestions = (value) => {
      setQuery(value);
  };

  const handleLike = async (movieId) => {
    try {
        await likeMovie(movieId);

        setLikedMovies(prev => [...prev, movieId]);

        toast.success("Movie liked successfully!");

        refreshLikes();
    }
    catch (error) {
        console.error(error);

        toast.error("Movie already liked.");
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
              onChange={(e) => loadSuggestions(e.target.value)}
            />

            <button
              className="btn btn-primary"
              onClick={handleSearch}
            >
              Search
            </button>

          </div>

          {suggestions.length > 0 && (

              <ul className="list-group mb-3">

                  {suggestions.map((item, index) => (

                      <li
                          key={index}
                          className="list-group-item list-group-item-action"
                          style={{ cursor: "pointer" }}
                          onClick={() => {
                              setQuery(item);
                              setSuggestions([]);
                          }}
                      >
                          🔍 {item}
                      </li>

                  ))}

              </ul>

          )}

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
                        isLiked={likedMovies.includes(movie.movieId)}
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