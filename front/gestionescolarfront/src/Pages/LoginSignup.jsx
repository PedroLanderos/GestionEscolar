import React, { useState, useContext } from "react";
import "./CSS/LoginSignup.css";
import { useNavigate } from "react-router-dom";
import { AuthContext } from "../Context/AuthContext";

const LoginSignup = () => {
  const [isRequesting, setIsRequesting] = useState(false);
  const navigate = useNavigate();
  const { login } = useContext(AuthContext);

  // Estado del formulario de login
  const [loginData, setLoginData] = useState({
    email: "",
    password: "",
  });

  // Estado del formulario de solicitud
  const [requestData, setRequestData] = useState({
    nombreAlumno: "",
    curpAlumno: "",
    grado: "1",
    nombrePadre: "",
    telefono: "",
    correoPadre: "",
  });

  const handleLoginChange = (e) => {
    const { name, value } = e.target;
    setLoginData((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const handleRequestChange = (e) => {
    const { name, value } = e.target;
    setRequestData((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const handleLoginSubmit = (e) => {
    e.preventDefault();
    console.log("🔐 Login:", loginData);
    // Aquí deberías llamar a tu endpoint de login con axios
    // login(token) si todo sale bien
    alert("Login simulado.");
  };

  const handleRequestSubmit = (e) => {
    e.preventDefault();
    console.log("📨 Solicitud enviada:", requestData);
    alert("Formulario capturado. (Aquí se conectará al backend)");
  };

  return (
    <div className="login-container">
      <form onSubmit={isRequesting ? handleRequestSubmit : handleLoginSubmit}>
        <h2>{isRequesting ? "Solicitud de registro de alumno" : "Iniciar sesión"}</h2>

        {isRequesting ? (
          <>
            <input
              type="text"
              name="nombreAlumno"
              placeholder="Nombre completo del alumno"
              value={requestData.nombreAlumno}
              onChange={handleRequestChange}
              required
            />
            <input
              type="text"
              name="curpAlumno"
              placeholder="CURP del alumno"
              value={requestData.curpAlumno}
              onChange={handleRequestChange}
              maxLength={18}
              required
            />
            <select
              name="grado"
              value={requestData.grado}
              onChange={handleRequestChange}
              required
            >
              <option value="1">1° Grado</option>
              <option value="2">2° Grado</option>
              <option value="3">3° Grado</option>
            </select>
            <input
              type="text"
              name="nombrePadre"
              placeholder="Nombre del padre o tutor"
              value={requestData.nombrePadre}
              onChange={handleRequestChange}
              required
            />
            <input
              type="tel"
              name="telefono"
              placeholder="Teléfono de contacto"
              value={requestData.telefono}
              onChange={handleRequestChange}
              pattern="[0-9]{10}"
              required
            />
            <input
              type="email"
              name="correoPadre"
              placeholder="Correo electrónico del padre"
              value={requestData.correoPadre}
              onChange={handleRequestChange}
              required
            />
            <button type="submit">Solicitar registro</button>
          </>
        ) : (
          <>
            <input
              type="email"
              name="email"
              placeholder="Correo electrónico"
              value={loginData.email}
              onChange={handleLoginChange}
              required
            />
            <input
              type="password"
              name="password"
              placeholder="Contraseña"
              value={loginData.password}
              onChange={handleLoginChange}
              required
            />
            <button type="submit">Iniciar sesión</button>
          </>
        )}

        <div className="login-toggle">
          {isRequesting ? (
            <p>
              ¿Ya tienes cuenta?{" "}
              <span onClick={() => setIsRequesting(false)}>Inicia sesión</span>
            </p>
          ) : (
            <p>
              ¿Eres padre/tutor y aún no tienes cuenta?{" "}
              <span onClick={() => setIsRequesting(true)}>
                Solicita registro de alumno
              </span>
            </p>
          )}
        </div>
      </form>
    </div>
  );
};

export default LoginSignup;
