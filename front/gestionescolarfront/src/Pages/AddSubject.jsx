import React, { useState, useEffect } from "react";
import axios from "axios";
import "./CSS/Register.css";
import { SUBJ_API } from "../Config/apiConfig";

const AddSubject = ({ subject = null, onSuccess }) => {
  const [formData, setFormData] = useState({
    id: 0,
    nombre: "",
    codigo: "",
    tipo: "Materia",
    grado: 1,
    esObligatoria: true,
    estaActiva: true,
    fechaCreacion: new Date().toISOString(),
  });

  const [loading, setLoading] = useState(false);
  const [message, setMessage] = useState("");

  useEffect(() => {
    if (subject) {
      setFormData({
        id: subject.id,
        nombre: subject.nombre,
        codigo: subject.codigo,
        tipo: subject.tipo,
        grado: subject.grado,
        esObligatoria: subject.esObligatoria,
        estaActiva: subject.estaActiva,
        fechaCreacion: subject.fechaCreacion,
      });
    }
  }, [subject]);

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

    // Validar tipo antes de enviar
    if (formData.tipo !== "Materia" && formData.tipo !== "Taller") {
      setMessage("Los datos ingresados no son válidos"); // ERR1
      setLoading(false);
      return;
    }

    const payload = {
      ...formData,
      grado: parseInt(formData.grado),
    };

    const isEdit = payload.id && payload.id > 0;
    const endpoint = `${SUBJ_API}/Subject/${isEdit ? "editarMateria" : "crearMateria"}`;
    const method = isEdit ? axios.put : axios.post;

    try {
      const res = await method(endpoint, payload);

      if (res.data.flag || res.data.success) {
        setMessage(
          isEdit
            ? "Elemento actualizado exitosamente."  // MSG2
            : "Elemento registrado exitosamente."   // MSG3
        );
        if (!isEdit) {
          setFormData({
            id: 0,
            nombre: "",
            codigo: "",
            tipo: "Materia",
            grado: 1,
            esObligatoria: true,
            estaActiva: true,
            fechaCreacion: new Date().toISOString(),
          });
        }
        if (onSuccess) onSuccess();
      } else {
        setMessage("Los datos ingresados no son válidos"); // ERR1
      }
    } catch (err) {
      console.error("Error:", err);
      setMessage("Error de conexión al servidor. Intenta nuevamente."); // ERR6
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="register-container">
      <div className="register-content">
        <h2>{formData.id ? "Editar Materia" : "Registro de Materia"}</h2>
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
          <select name="tipo" value={formData.tipo} onChange={handleChange} required>
            <option value="Materia">Materia</option>
            <option value="Taller">Taller</option>
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
            {loading ? "Guardando..." : formData.id ? "Actualizar" : "Registrar"}
          </button>

          {message && (
            <p style={{ marginTop: "1rem", color: message === "Elemento registrado exitosamente." || message === "Elemento actualizado exitosamente." ? "green" : "red" }}>
              {message}
            </p>
          )}
        </form>
      </div>
    </div>
  );
};

export default AddSubject;
