import React, { useState, useEffect } from "react";
import axios from "axios";
import "./CSS/UsersTable.css";

const ShowAbsences = () => {
  const [absences, setAbsences] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    const fetchAbsences = async () => {
      setLoading(true);
      try {
        const res = await axios.get("http://localhost:5004/api/asistencias/inasistencias/ciclo-escolar");
        setAbsences(res.data);
      } catch (error) {
        console.error("Error al obtener inasistencias:", error);
        setError("No existen datos para esta sección. Por favor, intente más tarde."); // ERR3
      } finally {
        setLoading(false);
      }
    };

    fetchAbsences();
  }, []);

  const handleDelete = async (id) => {
    const confirmDelete = window.confirm("¿Está seguro que desea eliminar esta inasistencia?");
    if (confirmDelete) {
      try {
        await axios.delete(`http://localhost:5004/api/asistencias/${id}`);
        setAbsences(absences.filter((absence) => absence.id !== id));
      } catch (error) {
        console.error("Error al eliminar la inasistencia", error);
        setError("Error de conexión al servidor. Intenta nuevamente."); // ERR6
      }
    }
  };

  return (
    <div className="users-table-container">
      <h2>Inasistencias del ciclo escolar activo</h2>

      {loading ? (
        <p>Cargando inasistencias...</p>
      ) : error ? (
        <p style={{ color: "red" }}>{error}</p>
      ) : (
        <table>
          <thead>
            <tr>
              <th>Fecha</th>
              <th>Alumno</th>
              <th>Profesor</th>
              <th>Justificación</th>
              <th>Acciones</th>
            </tr>
          </thead>
          <tbody>
            {absences.length > 0 ? (
              absences.map((absence) => (
                <tr key={absence.id}>
                  <td>{new Date(absence.fecha).toLocaleDateString()}</td>
                  <td>{absence.idAlumno}</td>
                  <td>{absence.idProfesor}</td>
                  <td>{absence.justificacion || "No existen datos para esta sección. Por favor, intente más tarde." /* ERR3 */}</td>
                  <td>
                    <button onClick={() => handleDelete(absence.id)}>Eliminar</button>
                  </td>
                </tr>
              ))
            ) : (
              <tr>
                <td colSpan="5">No existen datos para esta sección. Por favor, intente más tarde.</td> {/* ERR3 */}
              </tr>
            )}
          </tbody>
        </table>
      )}
    </div>
  );
};

export default ShowAbsences;
