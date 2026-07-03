import React, { useState } from "react";
import axios from "axios";

const API_BASE = "https://localhost:7176/api";

function Login({ onLogin, onRegister })  {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  const handleLogin = async () => {
    try {
      const response = await axios.post(
        `${API_BASE}/auth/login`,
        {
          email,
          password
        }
      );

      const token = response.data.token;

      localStorage.setItem("token", token);

      alert("Login Successful");

      onLogin();
    }
    catch (error) {
      alert("Invalid Email or Password");
      console.error(error);
    }
  };

  return (
    <div className="container mt-5">

      <h2>Login</h2>

      <div className="mb-3">

        <input
          className="form-control"
          placeholder="Email"
          value={email}
          onChange={(e) =>
            setEmail(e.target.value)}
        />

      </div>

      <div className="mb-3">

        <input
          type="password"
          className="form-control"
          placeholder="Password"
          value={password}
          onChange={(e) =>
            setPassword(e.target.value)}
        />

      </div>

      <button
        className="btn btn-primary"
        onClick={handleLogin}
      >
        Login
      </button>

      <div className="mt-3">
        <span>Don't have an account? </span>

        <button
          className="btn btn-link p-0"
          onClick={onRegister}
        >
          Register
        </button>
      </div>

    </div>

    
  );

}

export default Login;