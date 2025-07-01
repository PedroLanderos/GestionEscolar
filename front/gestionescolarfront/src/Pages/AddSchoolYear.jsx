import React, { useState, useEffect } from "react";
import axios from "axios";
import "./CSS/Register.css";
import { CICLO_API } from "../Config/apiConfig";

const AddSchoolYear = ({ initialData = null, onSuccess, onBack }) => {
  const [formData, setFormData] = useState({
    id: "",
    fechaRegistroCalificaciones: new Date().toISOString().slice(0, 10),
    fechaInicio: new Date().toISOString().slice(0, 10),
    fechaFin: new Date().toISOString().slice(0, 10),
    esActual: true,
  });

  const [loading, setLoading] = useState(false);
  const [message, setMessage] = useState("");

  // Cargar datos iniciales para editar
  useEffect(() => {
    if (initialData) {
      setFormData({
        id: initialData.id || "",
        fechaRegistroCalificaciones: initialData.fechaRegistroCalificaciones?.slice(0, 10) || new Date().toISOString().slice(0, 10),
        fechaInicio: initialData.fechaInicio?.slice(0, 10) || new Date().toISOString().slice(0, 10),
        fechaFin: initialData.fechaFin?.slice(0, 10) || new Date().toISOString().slice(0, 10),
        esActual: true,
      });
    }
  }, [initialData]);

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

    try {
      // Si estamos editando un ciclo activo existente, primero desactivamos el que esté activo
      const resActual = await axios.get(`${CICLO_API}/CicloEscolar/actual`);
      if (resActual.status === 200 && resActual.data && resActual.data.id !== formData.id) {
        // Desactivar el ciclo actual diferente al que editamos
        await axios.put(`${CICLO_API}/CicloEscolar/${resActual.data.id}`, {
          ...resActual.data,
          esActual: false,
        });
      }

      const cicloPayload = {
        id: formData.id,
        fechaRegistroCalificaciones: formData.fechaRegistroCalificaciones,
        fechaInicio: formData.fechaInicio,
        fechaFin: formData.fechaFin,
        esActual: true,
      };

      if (initialData) {
        // Editar ciclo existente
        const res = await axios.put(`${CICLO_API}/CicloEscolar/${formData.id}`, cicloPayload);
        if (res.data.flag) {
          setMessage("Elemento actualizado exitosamente.");
          if (onSuccess) onSuccess();
        } else {
          setMessage("Los datos ingresados no son válidos");
        }
      } else {
        // Crear nuevo ciclo
        const res = await axios.post(`${CICLO_API}/CicloEscolar`, cicloPayload);
        if (res.data.flag) {
          setMessage("Elemento registrado exitosamente.");
          setFormData({
            id: "",
            fechaRegistroCalificaciones: new Date().toISOString().slice(0, 10),
            fechaInicio: new Date().toISOString().slice(0, 10),
            fechaFin: new Date().toISOString().slice(0, 10),
            esActual: true,
          });
          if (onSuccess) onSuccess();
        } else {
          setMessage("Los datos ingresados no son válidos");
        }
      }
    } catch (error) {
      console.error("Error al crear/actualizar ciclo escolar:", error);
      setMessage("Error de conexión al servidor. Intenta nuevamente.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="register-container">
      <div className="register-content">
        <h2>{initialData ? "Modificar Ciclo Escolar Activo" : "Registrar Nuevo Ciclo Escolar"}</h2>
        <form onSubmit={handleSubmit}>
          <label>ID (ej: 2025A)</label>
          <input
            type="text"
            name="id"
            value={formData.id}
            onChange={handleChange}
            required
            disabled={!!initialData} // No dejar cambiar el ID al editar
          />

          <label>Fecha Registro Calificaciones</label>
          <input
            type="date"
            name="fechaRegistroCalificaciones"
            value={formData.fechaRegistroCalificaciones}
            onChange={handleChange}
            required
          />

          <label>Fecha Inicio</label>
          <input
            type="date"
            name="fechaInicio"
            value={formData.fechaInicio}
            onChange={handleChange}
            required
          />

          <label>Fecha Fin</label>
          <input
            type="date"
            name="fechaFin"
            value={formData.fechaFin}
            onChange={handleChange}
            required
          />

          <div className="checkbox-group">
            <input
              type="checkbox"
              name="esActual"
              checked={formData.esActual}
              onChange={handleChange}
              disabled
            />
            <label>¿Ciclo escolar activo?</label>
          </div>

          <button type="submit" disabled={loading}>
            {loading ? "Guardando..." : initialData ? "Actualizar" : "Registrar"}
          </button>

          <button type="button" onClick={onBack} style={{ marginLeft: "1rem" }}>
            Volver
          </button>

          {message && (
            <p style={{ marginTop: "1rem", color: message.includes("exitosamente") ? "green" : "red" }}>
              {message}
            </p>
          )}
        </form>
      </div>
    </div>
  );
};

export default AddSchoolYear;
