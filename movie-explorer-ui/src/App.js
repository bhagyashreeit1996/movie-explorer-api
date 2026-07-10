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
import Recommendations from "./components/Recommendations";
import { getLikedMovies } from "./services/api";

function App() {
  const [refresh, setRefresh] = useState(false);

  const [isLoggedIn, setIsLoggedIn] = useState(
    localStorage.getItem("token") !== null
  );

  const [showRegister, setShowRegister] = useState(false);
  const [searchCount, setSearchCount] = useState(0);
  const [currentPage, setCurrentPage] = useState(1);
  const [likedCount, setLikedCount] = useState(0);
  const [likedMovieIds, setLikedMovieIds] = useState([]);
  const [user, setUser] = useState(null);
  const [activePage, setActivePage] = useState("home");

  useEffect(() => {
      if (isLoggedIn) {
          loadCurrentUser();
          loadLikedMovieIds();
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
  const loadLikedMovieIds = async () => {
    try {
        const result = await getLikedMovies(1, 1000);

        const ids = result.data.map(movie => movie.movieId);

        setLikedMovieIds(ids);
    }
    catch (error) {
        console.error(error);
    }
    };

  const refreshLikes = () => {
      setRefresh(prev => !prev);
      loadLikedMovieIds();
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
        <>
          <Register
            onBackToLogin={() => setShowRegister(false)}
          />
          <ToastContainer position="top-right" autoClose={3000} />
        </>
      );
    }

    return (
      <>
        <Login
          onLogin={handleLogin}
          onRegister={() => setShowRegister(true)}
        />
        <ToastContainer position="top-right" autoClose={3000} />
      </>
    );
  }

  return (
    <>

    <Navbar
        user={user}
        onLogout={handleLogout}
        onHome={() => setActivePage("home")}
        onProfile={() => setActivePage("profile")}
    />

    <div className="container mt-4">

      {activePage === "home" ? (
          <>
              <Dashboard
                  likedCount={likedCount}
                  searchCount={searchCount}
                  pageNumber={currentPage}
              />

              <SearchMovies
                  refreshLikes={refreshLikes}
                  onSearchCompleted={handleSearchCompleted}
                  likedMovieIds={likedMovieIds}
                  setLikedMovieIds={setLikedMovieIds}
              />

              <hr />

              <LikedMovies
                  refresh={refresh}
                  onLikedMoviesLoaded={handleLikedMoviesLoaded}
                  likedMovieIds={likedMovieIds}
                  setLikedMovieIds={setLikedMovieIds}
              />

              <Recommendations />
          </>
      ) : (
          <Profile user={user} />
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