import React, { useEffect, useState } from "react";
import axios from "axios";
import "./CSS/RegisterRequest.css";
import { SCHEDULE_API } from "../Config/apiConfig";

const Schedules = ({ onViewSchedule, onAssignSchedule }) => {
  const [schedules, setSchedules] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const fetchSchedules = async () => {
    try {
      const response = await axios.get(`${SCHEDULE_API}/Schedule/obtenerHorarios`);
      setSchedules(response.data);
    } catch (error) {
      console.error("Error al obtener horarios:", error);
      setError("No existen datos para esta sección. Por favor, intente más tarde."); // ERR3
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchSchedules();
  }, []);

  return (
    <div className="register-request-container">
      <h2>Horarios registrados</h2>

      {loading && <p>Cargando horarios...</p>}
      {error && <p style={{ color: "red" }}>{error}</p>}

      {!loading && !error && schedules.length > 0 && (
        <table>
          <thead>
            <tr>
              <th>Grado</th>
              <th>Grupo</th>
              <th>Acciones</th>
            </tr>
          </thead>
          <tbody>
            {schedules.map((schedule, index) => (
              <tr key={index}>
                <td>{schedule.grado}</td>
                <td>{schedule.grupo}</td>
                <td>
                  <div className="action-buttons">
                    <button onClick={() => onViewSchedule(schedule)}>Consultar horario</button>
                    <button onClick={() => onAssignSchedule(schedule)}>Asignar horario</button>
                  </div>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}

      {!loading && !error && schedules.length === 0 && (
        <p>No hay horarios registrados.</p> // Aquí no hay mensaje en tu tabla, así que queda así.
      )}
    </div>
  );
};

export default Schedules;
