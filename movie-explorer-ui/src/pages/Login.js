import React, { useState } from "react";
import axios from "axios";
import { toast } from "react-toastify";

const API_BASE = process.env.REACT_APP_API_BASE;;

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

      toast.success("Login successful!");

      setTimeout(() => {
          onLogin();
      }, 1000);
    }
    catch (error) {
      console.log("FULL ERROR:", error);
      console.log("Status:", error.response?.status);
      console.log("Data:", error.response?.data);
      console.log("Message:", error.message);
      console.log("Request:", error.request);

      const message =
        error.response?.data?.message ||
        error.response?.data?.error ||
        error.message ||
        "Something went wrong.";

      toast.error(message);
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