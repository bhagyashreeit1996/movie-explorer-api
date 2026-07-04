import React, { useState } from "react";
import axios from "axios";
import { toast } from "react-toastify";

const API_BASE = "https://localhost:7176/api";

function Register({ onBackToLogin }) {
  const [name, setName] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  const handleRegister = async () => {
    try {
      await axios.post(`${API_BASE}/auth/register`, {
        name,
        email,
        password
      });

      toast.success("Registration successful!");

      onBackToLogin();
    } catch (error) {
      toast.error("Registration failed.");
      console.error(error);
    }
  };

  return (
    <div className="container mt-5">

      <h2>Register</h2>

      <div className="mb-3">
        <input
          className="form-control"
          placeholder="Name"
          value={name}
          onChange={(e) => setName(e.target.value)}
        />
      </div>

      <div className="mb-3">
        <input
          className="form-control"
          placeholder="Email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
        />
      </div>

      <div className="mb-3">
        <input
          type="password"
          className="form-control"
          placeholder="Password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
        />
      </div>

      <button
        className="btn btn-success"
        onClick={handleRegister}
      >
        Register
      </button>

      <button
        className="btn btn-secondary ms-2"
        onClick={onBackToLogin}
      >
        Back to Login
      </button>

    </div>
  );
}

export default Register;