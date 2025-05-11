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
        id: "TEMP123", // Se sobrescribirá en backend
        nombreCompleto: nombre,
        correo,
        contrasena: "temporal123!", // Se sobrescribirá en backend
        curp,
        cuentaBloqueada: false,
        dadoDeBaja: false,
        ultimaSesion: new Date().toISOString(),
        rol: "Alumno", // Se sobrescribirá en backend
      });

      if (response.data.success) {
        setMessage("✅ Alumno registrado exitosamente.");
      } else {
        setMessage(`❌ ${response.data.message || "Error al registrar alumno."}`);
      }
    } catch (err) {
      console.error("❌ Error al registrar alumno:", err);
      setMessage("❌ Error al registrar alumno. Revisa el servidor.");
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
            <p style={{ marginTop: "1rem", color: message.startsWith("✅") ? "green" : "red" }}>
              {message}
            </p>
          )}
        </form>
      </div>
    </div>
  );
};

export default Register;
