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
  const [showRecovery, setShowRecovery] = useState(false);
  const [recoveryId, setRecoveryId] = useState("");

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
        setErrorCount(0);
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
      if (error.response && error.response.status === 401) {
        const nuevosIntentos = errorCount + 1;
        setErrorCount(nuevosIntentos);
        const mensaje =
          (error.response.data && error.response.data.message) ||
          "Credenciales inválidas";
        alert("❌ " + mensaje);

        if (nuevosIntentos >= 3) {
          await bloquearUsuario();
        }
      } else {
        console.error("Error al iniciar sesión:", error);
        alert("Ocurrió un error al intentar iniciar sesión.");
      }
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

  const handleRecovery = async () => {
    if (!recoveryId.trim()) return alert("Ingresa un ID válido.");
    setLoading(true);

    try {
      const usuarioRes = await axios.get(`${AUTH_API}/usuario/obtenerUsuarioPorId/${recoveryId}`);
      const usuario = usuarioRes.data;

      if (!usuario || !usuario.rol) {
        alert("⚠️ Usuario no encontrado.");
        return;
      }

      const rol = usuario.rol.toLowerCase();

      if (rol === "alumno" || rol === "docente") {
        // Usar el nuevo flujo de recuperación
        const solicitudRes = await axios.post(`${AUTH_API}/SolicitudContrasena/CrearSolicitud/${recoveryId}`);
        alert(solicitudRes.data.message);
      } else if (rol === "padre" || rol === "administrador") {
        // Usar el flujo anterior
        const response = await axios.post(
          `${AUTH_API}/usuario/recuperarContrasena`,
          JSON.stringify(recoveryId),
          { headers: { "Content-Type": "application/json" } }
        );
        alert(response.data.message);
      } else {
        alert("⚠️ Este tipo de usuario no puede recuperar la contraseña automáticamente.");
      }

      setShowRecovery(false);
      setRecoveryId("");
    } catch (error) {
      console.error("❌ Error al recuperar contraseña:", error);
      alert("Error al intentar recuperar la contraseña.");
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
            <div className="password-recovery">
              <p>
                ¿Olvidaste tu contraseña?{" "}
                <span onClick={() => setShowRecovery(true)}>Recupérala aquí</span>
              </p>
            </div>
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

      {showRecovery && (
        <div className="modal-recovery">
          <div className="modal-content">
            <h3>Recuperar contraseña</h3>
            <input
              type="text"
              placeholder="ID del usuario"
              value={recoveryId}
              onChange={(e) => setRecoveryId(e.target.value)}
              required
            />
            <button onClick={handleRecovery} disabled={loading}>
              {loading ? "Enviando..." : "Enviar nueva contraseña"}
            </button>
            <button onClick={() => setShowRecovery(false)}>Cancelar</button>
          </div>
        </div>
      )}
    </div>
  );
};

export default LoginSignup;
