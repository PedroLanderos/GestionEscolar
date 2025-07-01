import React, { useState } from "react";
import axios from "axios";
import "./CSS/Register.css";

const Register = ({ data }) => {
  const nombre = data?.nombreAlumno || "";
  const curp = data?.curpAlumno || "";
  const correo = data?.correoPadre || "";

  const [loading, setLoading] = useState(false);
  const [message, setMessage] = useState("");

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setMessage("");

    try {
      const response = await axios.post("http://localhost:5000/api/usuario/registrarAlumno", {
        id: "TEMP123",
        nombreCompleto: nombre,
        correo,
        contrasena: "temporal123!",
        curp,
        cuentaBloqueada: false,
        dadoDeBaja: false,
        ultimaSesion: new Date().toISOString(),
        rol: "Alumno",
      });

      if (response.data.success) {
        setMessage("Elemento registrado exitosamente."); // MSG3
      } else {
        setMessage("Los datos ingresados no son válidos"); // ERR1
      }
    } catch (err) {
      console.error("Error al registrar alumno:", err);
      setMessage("Error de conexión al servidor. Intenta nuevamente."); // ERR6
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="register-container">
      <div className="register-content">
        <h2>Registro de Alumno</h2>
        <form onSubmit={handleSubmit}>
          <label>Nombre del Alumno</label>
          <input type="text" value={nombre} disabled />

          <label>CURP</label>
          <input type="text" value={curp} disabled />

          <label>Correo del Padre/Tutor</label>
          <input type="email" value={correo} disabled />

          <button type="submit" disabled={loading}>
            {loading ? "Registrando..." : "Registrar"}
          </button>

          {message && (
            <p style={{ marginTop: "1rem", color: message === "Elemento registrado exitosamente." ? "green" : "red" }}>
              {message}
            </p>
          )}
        </form>
      </div>
    </div>
  );
};

export default Register;
