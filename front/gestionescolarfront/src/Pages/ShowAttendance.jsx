import React, { useEffect, useState, useContext } from "react";
import axios from "axios";
import "./CSS/UsersTable.css";
import { AuthContext } from "../Context/AuthContext";

const ShowAttendance = () => {
  const { auth } = useContext(AuthContext);
  const alumnoId = auth.user?.id;

  const [asistencias, setAsistencias] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    if (!alumnoId || auth.user?.role !== "Alumno") {
      setError("No autorizado para ver esta sección.");
      setLoading(false);
      return;
    }

    const fetchAsistencias = async () => {
      setLoading(true);
      setError(null);
      try {
        const res = await axios.get(`http://localhost:5004/api/asistencias/alumno/${alumnoId}`);
        setAsistencias(res.data);
      } catch (err) {
        console.error("Error al obtener asistencias:", err);
        setError("Error al cargar asistencias. Intenta más tarde.");
      } finally {
        setLoading(false);
      }
    };

    fetchAsistencias();
  }, [alumnoId, auth.user?.role]);

  if (loading) return <p>Cargando asistencias...</p>;
  if (error) return <p style={{ color: "red" }}>{error}</p>;
  if (asistencias.length === 0) return <p>No se encontraron asistencias registradas.</p>;

  return (
    <div className="users-table-container">
      <h2>Historial de Asistencias</h2>
      <table>
        <thead>
          <tr>
            <th>Fecha</th>
            <th>Asistió</th>
            <th>Justificación</th>
            <th>ID Profesor</th>
          </tr>
        </thead>
        <tbody>
          {asistencias.map((asistencia) => (
            <tr key={asistencia.id}>
              <td>{new Date(asistencia.fecha).toLocaleDateString()}</td>
              <td>{asistencia.asistio ? "Sí" : "No"}</td>
              <td>{asistencia.justificacion || "-"}</td>
              <td>{asistencia.idProfesor}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default ShowAttendance;
