// src/Pages/AddTeacher.jsx
import React, { useState } from "react";
import axios from "axios";
import "./CSS/Register.css";

const AddTeacher = () => {
  const [formData, setFormData] = useState({
    id: "BA", // Tú lo ajustas a BA o BB manualmente
    nombreCompleto: "",
    correo: "",
    contrasena: "",
    curp: "",
  });

  const [loading, setLoading] = useState(false);
  const [message, setMessage] = useState("");

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setMessage("");

    try {
      const response = await axios.post("http://localhost:5000/api/usuario/registrarDocente", {
        ...formData,
        cuentaBloqueada: false,
        dadoDeBaja: false,
        rol: "Docente",
      });

      if (response.data.flag) {
        setMessage("✅ Docente registrado exitosamente.");
        setFormData({
          id: "BA",
          nombreCompleto: "",
          correo: "",
          contrasena: "",
          curp: "",
        });
      } else {
        setMessage("❌ " + (response.data.message || "Error al registrar docente."));
      }
    } catch (error) {
      console.error("❌ Error al registrar docente:", error);
      setMessage("❌ Error de conexión al servidor.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="register-container">
      <div className="register-content">
        <h2>Registro de Docente</h2>
        <form onSubmit={handleSubmit}>
          <label>ID (BA o BB)</label>
          <input name="id" type="text" value={formData.id} onChange={handleChange} required />

          <label>Nombre Completo</label>
          <input name="nombreCompleto" type="text" value={formData.nombreCompleto} onChange={handleChange} required />

          <label>Correo Electrónico</label>
          <input name="correo" type="email" value={formData.correo} onChange={handleChange} required />

          <label>Contraseña</label>
          <input name="contrasena" type="password" value={formData.contrasena} onChange={handleChange} required />

          <label>CURP</label>
          <input name="curp" type="text" value={formData.curp} onChange={handleChange} required />

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

export default AddTeacher;
