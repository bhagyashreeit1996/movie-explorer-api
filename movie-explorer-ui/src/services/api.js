import axios from "axios";

const API_BASE = "https://localhost:7176/api";

const api = axios.create({
  baseURL: API_BASE
});

api.interceptors.request.use((config) => {
  const token = localStorage.getItem("token");

  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }

  return config;
});

export const searchMovies = async (query) => {
  const response = await api.get(`/movies/search?query=${query}`);
  return response.data;
};

export const likeMovie = async (movieId) => {
  await api.post(`/movies/${movieId}/like`);
};

export const unlikeMovie = async (movieId) => {
  await api.delete(`/movies/${movieId}/like`);
};

export const getLikedMovies = async (pageNumber, pageSize) => {
  const response = await api.get(
    `/users/likes?pageNumber=${pageNumber}&pageSize=${pageSize}`
  );

  return response.data;
};

export const getMovieDetails = async (movieId) => {
  const response = await api.get(`/movies/${movieId}/details`);
  return response.data;
};

export const getCurrentUser = async () => {
  const response = await api.get("/auth/me");
  return response.data;
};


