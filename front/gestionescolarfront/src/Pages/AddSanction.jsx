import React, { useState, useContext, useEffect } from "react";
import axios from "axios";
import "./CSS/Register.css";
import { AuthContext } from "../Context/AuthContext";

const AddSanction = ({ onBack, initialData }) => {
  const { auth } = useContext(AuthContext);

  const [formData, setFormData] = useState({
    tipoSancion: "Riña",
    descripcion: "",
    fecha: new Date().toISOString().split("T")[0],
    idAlumno: "",
  });
  const [success, setSuccess] = useState("");
  const [error, setError] = useState("");

  useEffect(() => {
    if (initialData) {
      setFormData({
        tipoSancion: initialData.tipoSancion,
        descripcion: initialData.descripcion,
        fecha: new Date(initialData.fecha).toISOString().split("T")[0],
        idAlumno: initialData.idAlumno,
      });
    }
  }, [initialData]);

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
        id: initialData ? initialData.id : 0,
        ...formData,
        idAlumno: formData.idAlumno,
        idProfesor: auth.user?.id,
        fecha: new Date(formData.fecha).toISOString().split("T")[0] + "T08:00:00",
      };

      if (initialData) {
        await axios.put("http://localhost:5004/api/sancion", payload);
        setSuccess("Elemento actualizado exitosamente."); // MSG2
      } else {
        await axios.post("http://localhost:5004/api/sancion", payload);
        setSuccess("Elemento registrado exitosamente."); // MSG3
      }

      setFormData({
        tipoSancion: "Riña",
        descripcion: "",
        fecha: new Date().toISOString().split("T")[0],
        idAlumno: "",
      });
    } catch (err) {
      console.error("Error al guardar la sanción:", err);
      setError("Los datos ingresados no son válidos"); // ERR1
    }
  };

  return (
    <div className="register-container">
      <div className="register-content">
        <h2>{initialData ? "Editar Sanción" : "Registrar Sanción"}</h2>
        <form onSubmit={handleSubmit}>
          <label>Tipo de Sanción</label>
          <select
            name="tipoSancion"
            value={formData.tipoSancion}
            onChange={handleChange}
            required
          >
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
          <input
            type="date"
            name="fecha"
            value={formData.fecha}
            onChange={handleChange}
            required
          />

          <label>ID del Alumno</label>
          <input
            type="text"
            name="idAlumno"
            value={formData.idAlumno}
            onChange={handleChange}
            required
          />

          <button type="submit">{initialData ? "Actualizar" : "Guardar"} Sanción</button>
          <button type="button" className="back-button" onClick={onBack}>
            Volver
          </button>

          {success && (
            <p style={{ color: "green", marginTop: "1rem", textAlign: "center" }}>
              {success}
            </p>
          )}
          {error && (
            <p style={{ color: "red", marginTop: "1rem", textAlign: "center" }}>
              {error}
            </p>
          )}
        </form>
      </div>
    </div>
  );
};

export default AddSanction;
