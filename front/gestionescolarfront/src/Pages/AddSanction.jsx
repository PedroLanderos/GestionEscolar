import React, { useState, useContext } from "react";
import axios from "axios";
import "./CSS/Register.css";
import { AuthContext } from "../Context/AuthContext";

const AddSanction = ({ onBack }) => {
  const { auth } = useContext(AuthContext);

  const [formData, setFormData] = useState({
    tipoSancion: "Riña",
    descripcion: "",
    fecha: new Date().toISOString().split("T")[0],
    idAlumno: ""
  });

  const [success, setSuccess] = useState("");
  const [error, setError] = useState("");

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setSuccess("");
    setError("");

    try {
      const payload = {
        id: 0,
        ...formData,
        idAlumno: parseInt(formData.idAlumno),
        idProfesor: parseInt(auth.user?.id), // ID del docente desde sesión
        fecha: new Date(formData.fecha).toISOString()
      };

      await axios.post("http://localhost:5004/api/sancion", payload);
      setSuccess("✅ La sanción fue registrada exitosamente.");
      setFormData({
        tipoSancion: "Riña",
        descripcion: "",
        fecha: new Date().toISOString().split("T")[0],
        idAlumno: ""
      });
    } catch (err) {
      console.error("❌ Error al crear la sanción:", err);
      setError("❌ Ocurrió un error al guardar la sanción.");
    }
  };

  return (
    <div className="register-container">
      <div className="register-content">
        <h2>Registrar Sanción</h2>
        <form onSubmit={handleSubmit}>
          <label>Tipo de Sanción</label>
          <select name="tipoSancion" value={formData.tipoSancion} onChange={handleChange} required>
            <option value="Riña">Riña</option>
            <option value="Asistencia">Asistencia</option>
            <option value="No trabajo">No trabajo</option>
            <option value="Vandalismo">Vandalismo</option>
            <option value="Exhibicionismo">Exhibicionismo</option>
            <option value="Acoso">Acoso</option>
          </select>

          <label>Descripción (opcional)</label>
          <input
            type="text"
            name="descripcion"
            value={formData.descripcion}
            onChange={handleChange}
            placeholder="Descripción breve"
          />

          <label>Fecha</label>
          <input type="date" name="fecha" value={formData.fecha} onChange={handleChange} required />

          <label>ID del Alumno</label>
          <input type="number" name="idAlumno" value={formData.idAlumno} onChange={handleChange} required />

          <button type="submit">Guardar Sanción</button>
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

export default AddSanction;
