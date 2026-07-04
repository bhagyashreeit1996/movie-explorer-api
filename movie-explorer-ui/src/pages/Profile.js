import React from "react";

function Profile({ user }) {
    if (!user) {
        return <p>Loading...</p>;
    }

    return (
        <div className="container mt-4">

            <div className="card shadow">

                <div className="card-body">

                    <h2 className="mb-4">
                        👤 My Profile
                    </h2>

                    <p>
                        <strong>Name:</strong>
                        {" "}
                        {user.name}
                    </p>

                    <p>
                        <strong>Email:</strong>
                        {" "}
                        {user.email}
                    </p>

                </div>

            </div>

        </div>
    );
}

export default Profile;