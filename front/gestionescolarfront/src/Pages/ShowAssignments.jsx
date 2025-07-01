import React, { useEffect, useState } from "react";
import axios from "axios";
import { AUTH_API } from "../Config/apiConfig";
import "./CSS/UsersTable.css";

const ShowAssignments = () => {
  const [assignments, setAssignments] = useState([]);
  const [loading, setLoading] = useState(true);
  const [message, setMessage] = useState("");
  const [error, setError] = useState("");

  useEffect(() => {
    const fetchAssignments = async () => {
      setLoading(true);
      setError("");
      try {
        const res = await axios.get("http://localhost:5001/api/subjectassignment/obtenerAsignaciones");
        setAssignments(res.data);
      } catch (error) {
        console.error("Error al obtener asignaciones:", error);
        setError("No existen datos para esta sección. Por favor, intente más tarde."); // ERR3
      } finally {
        setLoading(false);
      }
    };

    fetchAssignments();
  }, []);

  const handleDelete = async (id) => {
    const confirmDelete = window.confirm("¿Está seguro que desea eliminar esta asignación?");
    if (confirmDelete) {
      try {
        await axios.delete(`http://localhost:5001/api/subjectassignment/eliminarAsignacion/${id}`);
        setMessage("Elemento registrado exitosamente."); // MSG3 usado como confirmación
        setAssignments(assignments.filter((assignment) => assignment.id !== id));
      } catch (error) {
        console.error("Error al eliminar la asignación", error);
        setError("Error de conexión al servidor. Intenta nuevamente."); // ERR6
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
                  <td>{assignment.userId}</td>
                  <td>{assignment.subjectId}</td>
                  <td>
                    <button onClick={() => handleDelete(assignment.id)}>Eliminar</button>
                  </td>
                </tr>
              ))
            ) : (
              <tr>
                <td colSpan="3">No existen datos para esta sección. Por favor, intente más tarde.</td> {/* ERR3 */}
              </tr>
            )}
          </tbody>
        </table>
      )}
    </div>
  );
};

export default ShowAssignments;
