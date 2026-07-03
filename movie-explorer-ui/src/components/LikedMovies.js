import React, { useEffect, useState } from "react";
import { getLikedMovies, unlikeMovie } from "../services/api";

function LikedMovies({ refresh }) {
  const [movies, setMovies] = useState([]);
  const [pageNumber, setPageNumber] = useState(1);
  const [pageSize] = useState(2);
  const [totalCount, setTotalCount] = useState(0);

  const fetchLikes = async () => {
  try {
    const result = await getLikedMovies(
      pageNumber,
      pageSize
    );

    console.log("Likes API Response:", result);

    setMovies(result.data || []);
    setTotalCount(result.totalCount || 0);
  }
  catch (error) {
    console.error("Likes API Error:", error);
  }
};

const handleUnlike = async (movieId) => {
  try {
    await unlikeMovie(movieId);

    fetchLikes();
  }
  catch (error) {
    console.error(error);
  }
};

  useEffect(() => {
    fetchLikes();
  }, [refresh, pageNumber]);

  return (
    <div className="container mt-3">
      <div className="card shadow-sm">
      <div className="card-body">
      <h2>Liked Movies</h2>

      <ul>
        {movies.map((movie) => (
          <li key={movie.movieId}>
            {movie.title} ({movie.year})

            <button
              onClick={() =>
                handleUnlike(movie.movieId)
              }
            >
              Unlike
            </button>
          </li>
        ))}
      </ul>

      <div>
        <button
          disabled={pageNumber === 1}
          onClick={() => setPageNumber(pageNumber - 1)}
        >
          Previous
        </button>

        <span style={{ margin: "0 10px" }}>
          Page {pageNumber}
        </span>

        <button
          disabled={pageNumber * pageSize >= totalCount}
          onClick={() => setPageNumber(pageNumber + 1)}
        >
          Next
        </button>
      </div>
      </div>
      </div>
    </div>
  );
}

export default LikedMovies;