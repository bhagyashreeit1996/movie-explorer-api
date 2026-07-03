import React, { useState } from "react";
import SearchMovies from "./components/SearchMovies";
import LikedMovies from "./components/LikedMovies";
import Login from "./pages/Login";
import Register from "./pages/Register";

function App() {
  const [refresh, setRefresh] = useState(false);

  const [isLoggedIn, setIsLoggedIn] = useState(
    localStorage.getItem("token") !== null
  );

  const [showRegister, setShowRegister] = useState(false);

  const refreshLikes = () => {
    setRefresh(!refresh);
  };

  const handleLogin = () => {
    setIsLoggedIn(true);
  };

  const handleLogout = () => {
    localStorage.removeItem("token");
    setIsLoggedIn(false);
  };

  if (!isLoggedIn) {
  if (showRegister) {
    return (
      <Register
        onBackToLogin={() => setShowRegister(false)}
      />
    );
  }

  return (
    <Login
      onLogin={handleLogin}
      onRegister={() => setShowRegister(true)}
    />
  );
  }

  return (
    <div className="container mt-4">

      <div className="d-flex justify-content-between align-items-center mb-4">
        <h1>Movie Explorer</h1>

        <button
          className="btn btn-danger"
          onClick={handleLogout}
        >
          Logout
        </button>
      </div>

      <SearchMovies refreshLikes={refreshLikes} />

      <hr />

      <LikedMovies refresh={refresh} />

    </div>
  );
}

export default App;