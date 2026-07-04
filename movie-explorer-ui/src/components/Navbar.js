import React from "react";

function Navbar({ onLogout, user }) {
  return (
    <nav className="navbar navbar-expand-lg navbar-dark bg-dark shadow">

      <div className="container">

        <span className="navbar-brand">
          🎬 Movie Explorer
        </span>

        <div className="ms-auto d-flex align-items-center">

          <span className="text-white me-3">
              👋 Welcome, {user?.name}
          </span>

          <button
            className="btn btn-outline-light"
            onClick={onLogout}
          >
            Logout
          </button>

        </div>

      </div>

    </nav>
  );
}

export default Navbar;