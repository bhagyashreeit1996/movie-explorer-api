import React, { useState } from "react";
import SearchMovies from "./components/SearchMovies";
import LikedMovies from "./components/LikedMovies";
import Login from "./pages/Login";
import Register from "./pages/Register";
import Navbar from "./components/Navbar";
import Dashboard from "./components/Dashboard";
import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { useEffect } from "react";
import { getCurrentUser } from "./services/api";
import Profile from "./pages/Profile";

function App() {
  const [refresh, setRefresh] = useState(false);

  const [isLoggedIn, setIsLoggedIn] = useState(
    localStorage.getItem("token") !== null
  );

  const [showRegister, setShowRegister] = useState(false);
  const [searchCount, setSearchCount] = useState(0);
  const [currentPage, setCurrentPage] = useState(1);
  const [likedCount, setLikedCount] = useState(0);
  const [user, setUser] = useState(null);
  const [showProfile, setShowProfile] = useState(false);

  useEffect(() => {
  if (isLoggedIn) {
    loadCurrentUser();
  }
  }, [isLoggedIn]);

  const loadCurrentUser = async () => {
  try {
    const result = await getCurrentUser();
    setUser(result);
  } catch (error) {
    console.error(error);
  }
};

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

  const handleSearchCompleted = (
  count,
    page
  ) => {
    setSearchCount(count);
    setCurrentPage(page);
  };

  const handleLikedMoviesLoaded = (count) => {
    setLikedCount(count);
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
  <>

    <Navbar
        onLogout={handleLogout}
        user={user}
        onProfile={() => setShowProfile(true)}
    />

    <div className="container mt-4">

      <Dashboard
          likedCount={likedCount}
          searchCount={searchCount}
          pageNumber={currentPage}
      />

      {showProfile ? (
          <Profile user={user} />
      ) : (
          <>
              <SearchMovies
                  refreshLikes={refreshLikes}
                  onSearchCompleted={handleSearchCompleted}
              />

              <hr />

              <LikedMovies
                  refresh={refresh}
                  onLikedMoviesLoaded={handleLikedMoviesLoaded}
              />
          </>
      )}

    </div>

    <ToastContainer
    position="top-right"
    autoClose={3000}
    />

  </>
  );
}

export default App;