import React, { useEffect, useState } from "react";
import axios from "axios";
import { AUTH_API } from "../Config/apiConfig";
import "./CSS/UsersTable.css"; // Reutilizamos el mismo CSS

const ShowAssignments = () => {
  const [assignments, setAssignments] = useState([]);
  const [loading, setLoading] = useState(true);
  const [message, setMessage] = useState("");
  const [error, setError] = useState("");

  // Obtener asignaciones
  useEffect(() => {
    const fetchAssignments = async () => {
      setLoading(true);
      try {
        const res = await axios.get("http://localhost:5001/api/subjectassignment/obtenerAsignaciones");
        setAssignments(res.data);
      } catch (error) {
        console.error("❌ Error al obtener asignaciones:", error);
        setError("Error al cargar asignaciones.");
      } finally {
        setLoading(false);
      }
    };

    fetchAssignments();
  }, []);

  // Función para eliminar asignación
  const handleDelete = async (id) => {
    const confirmDelete = window.confirm("¿Estás seguro de que deseas eliminar esta asignación?");
    if (confirmDelete) {
      try {
        await axios.delete(`http://localhost:5001/api/subjectassignment/eliminarAsignacion/${id}`);
        setMessage("Asignación eliminada correctamente.");
        setAssignments(assignments.filter((assignment) => assignment.id !== id)); // Eliminar de la lista local
      } catch (error) {
        console.error("❌ Error al eliminar la asignación", error);
        setError("Error al eliminar la asignación.");
      }
    }
  };

  return (
    <div className="users-table-container">
      <h2>Asignaciones</h2>
      {message && <p className="success-message">{message}</p>}
      {error && <p className="error">{error}</p>}

      {loading ? (
        <p>Cargando asignaciones...</p>
      ) : (
        <table>
          <thead>
            <tr>
              <th>Docente</th>
              <th>Materia</th>
              <th>Acciones</th>
            </tr>
          </thead>
          <tbody>
            {assignments.length > 0 ? (
              assignments.map((assignment) => (
                <tr key={assignment.id}>
                  <td>{assignment.userId }</td>
                  <td>{assignment.subjectId }</td>
                  <td>
                    <button onClick={() => handleDelete(assignment.id)}>Eliminar</button>
                  </td>
                </tr>
              ))
            ) : (
              <tr>
                <td colSpan="3">No hay asignaciones disponibles.</td>
              </tr>
            )}
          </tbody>
        </table>
      )}
    </div>
  );
};

export default ShowAssignments;
