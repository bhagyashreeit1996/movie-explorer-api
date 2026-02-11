# 🚀 Project Features & Roadmap

This project is an evolving Movie Explorer. Below are the current features and what's coming next.

## 🎬 Current Features

### 1. Movie Search (`MoviesController`)
- **Endpoint:** `GET /api/movies/search?query={title}`
- **Description:** Allows users to search for movies by title. 
- **Internal Logic:** 
  - Checks if the movie exists in the local database.
  - If not, it saves the movie (mock data for now) to the database.
  - Returns a clean `MovieDto` to the caller.

## 📅 Roadmap (Coming Soon)
- [ ] **External API Integration:** Connect to OMDb or TMDB to fetch real movie data.
- [ ] **User Accounts:** Registration and Login.
- [ ] **Movie Likes:** Allow users to save their favorite movies.
- [ ] **Reviews:** Add comments and ratings to movies.

---
[⬅️ Back to main README](../README.md)
