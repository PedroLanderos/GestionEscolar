import React, { useState } from "react";
import axios from "axios";
import "./CSS/Register.css"; 
import { SUBJ_API } from "../Config/apiConfig";

const AddSubject = () => {
  const [formData, setFormData] = useState({
    nombre: "",
    codigo: "",
    tipo: "Teórica",
    grado: 1,
    esObligatoria: true,
    estaActiva: true,
  });

  const [loading, setLoading] = useState(false);
  const [message, setMessage] = useState("");

  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: type === "checkbox" ? checked : value,
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setMessage("");

    const payload = {
      ...formData,
      grado: parseInt(formData.grado),
      fechaCreacion: new Date().toISOString(),
    };

    try {
      const res = await axios.post(`${SUBJ_API}/Subject/crearMateria`, payload);

      if (res.data.success) {
        setMessage("✅ Materia registrada exitosamente.");
        setFormData({
          nombre: "",
          codigo: "",
          tipo: "Teórica",
          grado: 1,
          esObligatoria: true,
          estaActiva: true,
        });
      } else {
        setMessage(`❌ ${res.data.message || "Error al registrar la materia."}`);
      }
    } catch (err) {
      console.error("❌ Error al registrar materia:", err);
      setMessage("❌ Error al registrar materia. Revisa el servidor.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="register-container">
      <div className="register-content">
        <h2>Registro de Materia</h2>
        <form onSubmit={handleSubmit}>
          <label>Nombre</label>
          <input
            type="text"
            name="nombre"
            value={formData.nombre}
            onChange={handleChange}
            required
          />

          <label>Código</label>
          <input
            type="text"
            name="codigo"
            value={formData.codigo}
            onChange={handleChange}
            required
          />

          <label>Tipo</label>
          <select name="tipo" value={formData.tipo} onChange={handleChange}>
            <option value="Teórica">Teórica</option>
            <option value="Práctica">Práctica</option>
            <option value="Mixta">Mixta</option>
          </select>

          <label>Grado</label>
          <input
            type="number"
            name="grado"
            min="1"
            max="6"
            value={formData.grado}
            onChange={handleChange}
            required
          />

          <div className="checkbox-group">
            <input
              type="checkbox"
              name="esObligatoria"
              checked={formData.esObligatoria}
              onChange={handleChange}
            />
            <label htmlFor="esObligatoria">¿Es obligatoria?</label>
          </div>

          <div className="checkbox-group">
            <input
              type="checkbox"
              name="estaActiva"
              checked={formData.estaActiva}
              onChange={handleChange}
            />
            <label htmlFor="estaActiva">¿Está activa?</label>
          </div>

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

export default AddSubject;
