import React, { useState, useEffect } from "react";
import axios from "axios";
import "./CSS/Register.css";
import { CICLO_API } from "../Config/apiConfig"; // Define esta URL en tu config

const AddSchoolYear = ({ onSuccess }) => {
  const [formData, setFormData] = useState({
    id: "",
    fechaRegistroCalificaciones: new Date().toISOString().slice(0,10),
    fechaInicio: new Date().toISOString().slice(0,10),
    fechaFin: new Date().toISOString().slice(0,10),
    esActual: true,
  });

  const [loading, setLoading] = useState(false);
  const [message, setMessage] = useState("");
  const [activeCycleExists, setActiveCycleExists] = useState(false);

  // Cargar ciclo activo al montar para saber si existe alguno
  useEffect(() => {
    const fetchActiveCycle = async () => {
      try {
        const res = await axios.get(`${CICLO_API}/CicloEscolar/actual`);
        if (res.status === 200 && res.data) {
          setActiveCycleExists(true);
        } else {
          setActiveCycleExists(false);
        }
      } catch (error) {
        // Si no hay ciclo activo o error, asumimos que no hay ciclo activo
        setActiveCycleExists(false);
      }
    };
    fetchActiveCycle();
  }, []);

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
      if (activeCycleExists) {
        // 1. Obtener ciclo activo (para desactivarlo)
        const activeRes = await axios.get(`${CICLO_API}/CicloEscolar/actual`);
        if (activeRes.status === 200 && activeRes.data) {
          const activeCycle = activeRes.data;

          // 2. Desactivar ciclo activo
          await axios.put(`${CICLO_API}/CicloEscolar/${activeCycle.id}`, {
            ...activeCycle,
            esActual: false,
          });
        }
      }

      // 3. Crear nuevo ciclo marcado como activo
      const newCycle = {
        id: formData.id,
        fechaRegistroCalificaciones: formData.fechaRegistroCalificaciones,
        fechaInicio: formData.fechaInicio,
        fechaFin: formData.fechaFin,
        esActual: true,
      };

      const createRes = await axios.post(`${CICLO_API}/CicloEscolar`, newCycle);

      if (createRes.data.flag) {
        setMessage("✅ Ciclo escolar creado y activado correctamente.");
        setFormData({
          id: "",
          fechaRegistroCalificaciones: new Date().toISOString().slice(0,10),
          fechaInicio: new Date().toISOString().slice(0,10),
          fechaFin: new Date().toISOString().slice(0,10),
          esActual: true,
        });
        setActiveCycleExists(true); // Ahora hay un ciclo activo
        if(onSuccess) onSuccess();
      } else {
        setMessage(`❌ Error: ${createRes.data.message || "No se pudo crear el ciclo escolar."}`);
      }
    } catch (error) {
      console.error("❌ Error al crear ciclo escolar:", error);
      setMessage("❌ Error al crear ciclo escolar. Revisa el servidor.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="register-container">
      <div className="register-content">
        <h2>Registrar Nuevo Ciclo Escolar</h2>
        <form onSubmit={handleSubmit}>
          <label>ID (ej: 2025A)</label>
          <input
            type="text"
            name="id"
            value={formData.id}
            onChange={handleChange}
            required
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
              disabled // siempre true para nuevo ciclo, no editable
            />
            <label>¿Ciclo escolar activo?</label>
          </div>

          <button type="submit" disabled={loading}>
            {loading ? "Guardando..." : "Registrar"}
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

export default AddSchoolYear;