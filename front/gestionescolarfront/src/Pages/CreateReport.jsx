import React, { useEffect, useState } from "react";
import axios from "axios";
import "./CSS/Register.css";

const CreateReport = ({ onBack }) => {
  const [formData, setFormData] = useState({
    fecha: new Date().toISOString().split("T")[0],
    idAlumno: "",
    grupo: "",
    cicloEscolar: "", // será asignado automáticamente
    tipo: "Inasistencia",
  });

  const [success, setSuccess] = useState("");
  const [error, setError] = useState("");

  // 🔄 Obtener ciclo escolar actual al cargar
  useEffect(() => {
    const fetchCiclo = async () => {
      try {
        const response = await axios.get("http://localhost:5004/api/CicloEscolar/actual");
        const ciclo = response.data?.id;
        if (ciclo) {
          setFormData(prev => ({ ...prev, cicloEscolar: ciclo }));
        } else {
          setError("No se encontró un ciclo escolar activo.");
        }
      } catch (err) {
        console.error("❌ Error obteniendo ciclo escolar:", err);
        setError("Error al obtener el ciclo escolar actual.");
      }
    };

    fetchCiclo();
  }, []);

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
        fecha: new Date(formData.fecha).toISOString(),
      };

      await axios.post("http://localhost:5004/api/reporte", payload);

      setSuccess("✅ Reporte creado exitosamente.");
      setFormData({
        fecha: new Date().toISOString().split("T")[0],
        idAlumno: "",
        grupo: "",
        cicloEscolar: formData.cicloEscolar, // mantenemos el ciclo ya obtenido
        tipo: "Inasistencia",
      });
    } catch (err) {
      console.error("❌ Error al crear el reporte:", err);
      setError("❌ Ocurrió un error al guardar el reporte.");
    }
  };

  return (
    <div className="register-container">
      <div className="register-content">
        <h2>Crear Reporte</h2>
        <form onSubmit={handleSubmit}>
          <label>Fecha</label>
          <input
            type="date"
            name="fecha"
            value={formData.fecha}
            onChange={handleChange}
            required
          />

          <label>ID Alumno</label>
          <input
            type="text"
            name="idAlumno"
            value={formData.idAlumno}
            onChange={handleChange}
            required
          />

          <label>Grupo</label>
          <input
            type="text"
            name="grupo"
            value={formData.grupo}
            onChange={handleChange}
            required
          />

          {/* 👇 Campo oculto o automático para el ciclo escolar */}
          <input
            type="hidden"
            name="cicloEscolar"
            value={formData.cicloEscolar}
            readOnly
          />

          <label>Tipo</label>
          <select name="tipo" value={formData.tipo} onChange={handleChange} required>
            <option value="Inasistencia">Inasistencia</option>
            <option value="Conducta">Conducta</option>
          </select>

          <button type="submit">Guardar Reporte</button>
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

export default CreateReport;
