import React, { useEffect, useState } from "react";
import { getMovieDetails } from "../services/api";

function MovieDetails({ movieId, onClose }) {

    const [movie, setMovie] = useState(null);

    useEffect(() => {
        loadMovie();
    }, [movieId]);

    const loadMovie = async () => {
        try {
            const result = await getMovieDetails(movieId);
            setMovie(result);
        }
        catch (error) {
            console.error(error);
        }
    };

    if (!movie) {
        return null;
    }

    return (

        <div
            className="modal fade show"
            style={{
                display: "block",
                backgroundColor: "rgba(0,0,0,0.7)"
            }}
        >

            <div className="modal-dialog modal-lg">

                <div className="modal-content">

                    <div className="modal-header">

                        <h5 className="modal-title">
                            {movie.title}
                        </h5>

                        <button
                            className="btn-close"
                            onClick={onClose}
                        />

                    </div>

                    <div className="modal-body">

                        <div className="row">

                            <div className="col-md-4">

                                <img
                                    src={movie.poster}
                                    alt={movie.title}
                                    className="img-fluid rounded"
                                />

                            </div>

                            <div className="col-md-8">

                                <h4>{movie.title}</h4>

                                <p>
                                    ⭐ <strong>{movie.imdbRating}</strong>
                                </p>

                                <p>
                                    <strong>Year:</strong> {movie.year}
                                </p>

                                <p>
                                    <strong>Genre:</strong> {movie.genre}
                                </p>

                                <p>
                                    <strong>Director:</strong> {movie.director}
                                </p>

                                <p>
                                    <strong>Actors:</strong> {movie.actors}
                                </p>

                                <p>
                                    <strong>Runtime:</strong> {movie.runtime}
                                </p>

                                <p>
                                    <strong>Language:</strong> {movie.language}
                                </p>

                            </div>

                        </div>

                        <hr />

                        <h5>Plot</h5>

                        <p>{movie.plot}</p>

                    </div>

                    <div className="modal-footer">

                        <button
                            className="btn btn-secondary"
                            onClick={onClose}
                        >
                            Close
                        </button>

                    </div>

                </div>

            </div>

        </div>

    );
}

export default MovieDetails;