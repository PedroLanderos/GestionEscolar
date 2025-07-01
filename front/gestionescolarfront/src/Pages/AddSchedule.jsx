import React, { useState } from "react";
import axios from "axios";
import "./CSS/Register.css";

const AddSchedule = () => {
  const [formData, setFormData] = useState({
    id: 0,
    grado: 1,
    grupo: "",
  });

  const [loading, setLoading] = useState(false);
  const [message, setMessage] = useState("");

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: name === "grado" ? parseInt(value) : value,
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setMessage("");
    setLoading(true);

    try {
      const res = await axios.post("http://localhost:5002/api/Schedule/crearHorario", formData);

      if (res.data.flag || res.status === 200) {
        setMessage("Elemento creado exitosamente."); // MSG1
        setFormData({ id: 0, grado: 1, grupo: "" });
      } else {
        // Usamos un error genérico de validación
        setMessage("Los datos ingresados no son válidos"); // ERR1
      }
    } catch (err) {
      console.error("Error al crear horario:", err);
      setMessage("Error de conexión al servidor. Intenta nuevamente."); // ERR6
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="register-container">
      <div className="register-content">
        <h2>Crear Horario</h2>
        <form onSubmit={handleSubmit}>
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

          <label>Grupo</label>
          <input
            type="text"
            name="grupo"
            value={formData.grupo}
            onChange={handleChange}
            required
          />

          <button type="submit" disabled={loading}>
            {loading ? "Creando..." : "Crear Horario"}
          </button>

          {message && (
            <p
              style={{
                marginTop: "1rem",
                color:
                  message === "Elemento creado exitosamente."
                    ? "green"
                    : "red",
              }}
            >
              {message}
            </p>
          )}
        </form>
      </div>
    </div>
  );
};

export default AddSchedule;
