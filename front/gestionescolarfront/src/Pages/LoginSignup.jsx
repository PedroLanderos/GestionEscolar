import React, { useState, useContext } from "react";
import "./CSS/LoginSignup.css";
import { useNavigate } from "react-router-dom";
import { AuthContext } from "../Context/AuthContext";
import axios from "axios";
import { AUTH_API } from "../Config/apiConfig";

const LoginSignup = () => {
  const [isRequesting, setIsRequesting] = useState(false);
  const [loading, setLoading] = useState(false);
  const [errorCount, setErrorCount] = useState(0);
  const navigate = useNavigate();
  const { login } = useContext(AuthContext);

  const [loginData, setLoginData] = useState({
    identificador: "",
    password: "",
  });

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

  const bloquearUsuario = async () => {
    try {
      const response = await axios.get(`${AUTH_API}/usuario/obtenerUsuarios`);
      const usuario = response.data.find(
        (u) =>
          u.id === loginData.identificador || u.correo === loginData.identificador
      );

      if (usuario) {
        const updateDto = {
          ...usuario,
          cuentaBloqueada: true,
        };

        await axios.put(`${AUTH_API}/usuario/editarUsuario`, updateDto);
        alert("🚫 Cuenta bloqueada por intentos fallidos.");
      }
    } catch (error) {
      console.error("Error al bloquear usuario:", error);
    }
  };

  const handleLoginSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);

    try {
      const response = await axios.post(`${AUTH_API}/usuario/login`, {
        identificador: loginData.identificador,
        password: loginData.password,
      });

      if (response.data.flag) {
        login(response.data.message);
        navigate("/MenuPrincipal");
      } else {
        const nuevosIntentos = errorCount + 1;
        setErrorCount(nuevosIntentos);
        alert("❌ " + response.data.message);

        if (nuevosIntentos >= 3) {
          await bloquearUsuario();
        }
      }
    } catch (error) {
      console.error("Error al iniciar sesión:", error);
      alert("Ocurrió un error al intentar iniciar sesión.");
    } finally {
      setLoading(false);
    }
  };

  const handleRequestSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);

    try {
      const response = await axios.post(`${AUTH_API}/solicitud/enviar`, {
        ...requestData,
        grado: parseInt(requestData.grado),
      });

      alert(response.data.message);
      setRequestData({
        nombreAlumno: "",
        curpAlumno: "",
        grado: "1",
        nombrePadre: "",
        telefono: "",
        correoPadre: "",
      });
    } catch (error) {
      console.error("❌ Error al enviar solicitud:", error);
      alert("Ocurrió un error al enviar la solicitud.");
    } finally {
      setLoading(false);
    }
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
              type="text"
              name="correoPadre"
              placeholder="Correo electrónico del padre"
              value={requestData.correoPadre}
              onChange={handleRequestChange}
              required
            />
            <button type="submit" disabled={loading}>
              {loading ? "Enviando..." : "Solicitar registro"}
            </button>
          </>
        ) : (
          <>
            <input
              type="text"
              name="identificador"
              placeholder="Correo o ID"
              value={loginData.identificador}
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
            <button type="submit" disabled={loading}>
              {loading ? "Iniciando..." : "Iniciar sesión"}
            </button>
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
