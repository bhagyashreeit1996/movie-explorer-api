import React, { useEffect, useState } from "react";
import { getRecommendations } from "../services/api";
import MovieCard from "./MovieCard";

function Recommendations() {

    const [movies, setMovies] = useState([]);

    useEffect(() => {
        loadRecommendations();
    }, []);

    const loadRecommendations = async () => {
        try {
            const result = await getRecommendations();
            setMovies(result);
        }
        catch (error) {
            console.error(error);
        }
    };

    return (
        <div className="mt-5">

            <h3 className="mb-3">
                ❤️ Recommended For You
            </h3>

            <div className="row">

                {movies.length === 0 ? (

                    <p>No recommendations available.</p>

                ) : (

                    movies.map(movie => (

                        <MovieCard
                            key={movie.movieId}
                            movie={movie}
                            onLike={() => { }}
                            onDetails={() => { }}
                        />

                    ))

                )}

            </div>

        </div>
    );
}

export default Recommendations;