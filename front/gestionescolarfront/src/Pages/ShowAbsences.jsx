import React, { useState, useEffect } from "react";
import axios from "axios";
import "./CSS/UsersTable.css"; // Reutilizamos el CSS de UsersTable

const ShowAbsences = () => {
  const [absences, setAbsences] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  // Obtener inasistencias
  useEffect(() => {
    const fetchAbsences = async () => {
      setLoading(true);
      try {
        const res = await axios.get("http://localhost:5004/api/asistencias/inasistencias/ciclo-escolar");
        setAbsences(res.data);
      } catch (error) {
        console.error("❌ Error al obtener inasistencias:", error);
        setError("Error al cargar las inasistencias.");
      } finally {
        setLoading(false);
      }
    };

    fetchAbsences();
  }, []);

  // Eliminar inasistencia
  const handleDelete = async (id) => {
    const confirmDelete = window.confirm("¿Estás seguro de que deseas eliminar esta inasistencia?");
    if (confirmDelete) {
      try {
        await axios.delete(`http://localhost:5004/api/asistencias/${id}`);
        setAbsences(absences.filter((absence) => absence.id !== id)); // Eliminar de la lista local
      } catch (error) {
        console.error("❌ Error al eliminar la inasistencia", error);
        setError("Error al eliminar la inasistencia.");
      }
    }
  };

  return (
    <div className="users-table-container">
      <h2>Absences of the Active School Cycle</h2>

      {loading ? (
        <p>Loading absences...</p>
      ) : error ? (
        <p style={{ color: "red" }}>{error}</p>
      ) : (
        <table>
          <thead>
            <tr>
              <th>Date</th>
              <th>Student</th>
              <th>Teacher</th>
              <th>Justification</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {absences.length > 0 ? (
              absences.map((absence) => (
                <tr key={absence.id}>
                  <td>{new Date(absence.fecha).toLocaleDateString()}</td>
                  <td>{absence.idAlumno}</td>
                  <td>{absence.idProfesor}</td>
                  <td>{absence.justificacion || "No justification"}</td>
                  <td>
                    <button onClick={() => handleDelete(absence.id)}>Delete</button>
                  </td>
                </tr>
              ))
            ) : (
              <tr>
                <td colSpan="5">No absences recorded.</td>
              </tr>
            )}
          </tbody>
        </table>
      )}
    </div>
  );
};

export default ShowAbsences;
