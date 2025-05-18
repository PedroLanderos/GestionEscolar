import React, { useState } from "react";
import axios from "axios";
import "./CSS/Register.css";

const AddAttendance = ({ onBack }) => {
  const [formData, setFormData] = useState({
    fecha: new Date().toISOString().split("T")[0],
    asistio: true,
    justificacion: "",
    idAlumno: "",
    idProfesor: ""
  });

  const [success, setSuccess] = useState("");
  const [error, setError] = useState("");

  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    const inputValue = type === "checkbox" ? checked : value;
    setFormData((prev) => ({ ...prev, [name]: inputValue }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setSuccess("");
    setError("");

    try {
      const payload = {
        id: 0,
        ...formData,
        idAlumno: formData.idAlumno,
        idProfesor: formData.idProfesor,
        fecha: new Date(formData.fecha).toISOString()
      };

      await axios.post("http://localhost:5004/api/asistencias", payload);
      setSuccess("✅ Asistencia registrada correctamente.");
      setFormData({
        fecha: new Date().toISOString().split("T")[0],
        asistio: true,
        justificacion: "",
        idAlumno: "",
        idProfesor: ""
      });
    } catch (err) {
      console.error("❌ Error al registrar la asistencia:", err);
      setError("❌ Ocurrió un error al guardar la asistencia.");
    }
  };

  return (
    <div className="register-container">
      <div className="register-content">
        <h2>Registrar Asistencia</h2>
        <form onSubmit={handleSubmit}>
          <label>Fecha</label>
          <input type="date" name="fecha" value={formData.fecha} onChange={handleChange} required />

          <label>¿Asistió?</label>
          <div className="checkbox-group">
            <input
              type="checkbox"
              name="asistio"
              checked={formData.asistio}
              onChange={handleChange}
            />
            <span>{formData.asistio ? "Sí" : "No"}</span>
          </div>

          <label>Justificación (si no asistió)</label>
          <input
            type="text"
            name="justificacion"
            value={formData.justificacion}
            onChange={handleChange}
            placeholder="Opcional"
          />

          <label>ID del Alumno</label>
          <input
            type="text"
            name="idAlumno"
            value={formData.idAlumno}
            onChange={handleChange}
            required
          />

          <label>ID del Profesor</label>
          <input
            type="text"
            name="idProfesor"
            value={formData.idProfesor}
            onChange={handleChange}
            required
          />

          <button type="submit">Guardar Asistencia</button>
          <button type="button" className="back-button" onClick={onBack}>
            Volver
          </button>

          {success && <p style={{ color: "green", marginTop: "1rem", textAlign: "center" }}>{success}</p>}
          {error && <p style={{ color: "red", marginTop: "1rem", textAlign: "center" }}>{error}</p>}
        </form>
      </div>
    </div>
  );
};

export default AddAttendance;
