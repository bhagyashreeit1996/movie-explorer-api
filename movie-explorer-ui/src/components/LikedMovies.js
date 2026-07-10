import React, { useEffect, useState } from "react";
import { getLikedMovies, unlikeMovie } from "../services/api";
import { toast } from "react-toastify";

function LikedMovies({
    refresh,
    onLikedMoviesLoaded,
    likedMovieIds,
    setLikedMovieIds
}) {
  const [movies, setMovies] = useState([]);
  const [pageNumber, setPageNumber] = useState(1);
  const [pageSize] = useState(2);
  const [totalCount, setTotalCount] = useState(0);

  const fetchLikes = async () => {
    try {
      const result = await getLikedMovies(pageNumber, pageSize);

      setMovies(result.data);
      setTotalCount(result.totalCount);
      if (onLikedMoviesLoaded) {
          onLikedMoviesLoaded(result.totalCount);
      }
    } catch (error) {
      console.error(error);
      toast.error("Unable to unlike movie.");
    }
  };

  useEffect(() => {
    fetchLikes();
  }, [refresh, pageNumber]);

  const handleUnlike = async (movieId) => {
    try {
      await unlikeMovie(movieId);

      setLikedMovieIds(prev =>
          prev.filter(id => id !== movieId)
      );

      toast.success("Movie removed from liked list.");

      fetchLikes();
    } catch (error) {
      console.error(error);
      toast.error("Unable to unlike movie.");
    }
  };

  return (
    <div className="container mt-4">
      <div className="card shadow-sm">
        <div className="card-body">

          <h3 className="card-title mb-3">
            ❤️ Liked Movies
          </h3>

          {movies.length === 0 ? (
            <p>No liked movies found.</p>
          ) : (
            <>
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
                      className="btn btn-danger btn-sm"
                      onClick={() => handleUnlike(movie.movieId)}
                    >
                      Unlike
                    </button>
                  </li>
                ))}

              </ul>

              <div className="d-flex justify-content-between mt-3">

                <button
                  className="btn btn-secondary"
                  disabled={pageNumber === 1}
                  onClick={() => setPageNumber(pageNumber - 1)}
                >
                  Previous
                </button>

                <span className="align-self-center">
                  Page {pageNumber}
                </span>

                <button
                  className="btn btn-secondary"
                  disabled={pageNumber * pageSize >= totalCount}
                  onClick={() => setPageNumber(pageNumber + 1)}
                >
                  Next
                </button>

              </div>
            </>
          )}

        </div>
      </div>
    </div>
  );
}

export default LikedMovies;