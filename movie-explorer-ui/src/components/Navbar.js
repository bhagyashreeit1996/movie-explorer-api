import React from "react";

function Navbar({
    user,
    onLogout,
    onHome,
    onProfile
}) {
  return (
    <nav className="navbar navbar-expand-lg navbar-dark bg-dark shadow">

      <div className="container">

        <span className="navbar-brand">
          🎬 Movie Explorer
        </span>

        <div className="ms-auto d-flex align-items-center">

          <button
              className="btn btn-link text-white me-3"
              onClick={onHome}
          >
              🏠 Home
          </button>

          <button
              className="btn btn-link text-white me-3"
              onClick={onProfile}
          >
              👤 {user?.name}
          </button>

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