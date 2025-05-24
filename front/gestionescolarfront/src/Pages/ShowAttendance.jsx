import React, { useEffect, useState, useContext } from "react";
import axios from "axios";
import "./CSS/UsersTable.css";
import { AuthContext } from "../Context/AuthContext";

const ShowAttendance = () => {
  const { auth } = useContext(AuthContext);
  const userId = auth.user?.id;
  const userRole = auth.user?.role?.toLowerCase();

  const [asistencias, setAsistencias] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    if (!userId) {
      setError("No autorizado para ver esta sección.");
      setLoading(false);
      return;
    }

    const fetchAsistencias = async () => {
      setLoading(true);
      setError(null);

      try {
        let alumnoId = userId;

        // Si es tutor, obtener id de alumno (hijo)
        if (userRole === "tutor" || userRole === "padre") {
          try {
            const res = await axios.get(`http://localhost:5000/api/usuario/obtenerAlumnoPorTutor/${userId}`);
            if (res.data && res.data.id) {
              alumnoId = res.data.id;
            } else {
              setError("No se encontró alumno asociado al tutor");
              setLoading(false);
              return;
            }
          } catch (error) {
            setError("Error al obtener alumno para tutor");
            setLoading(false);
            return;
          }
        }

        // Solo alumnos y tutores pueden ver asistencias
        if (!(userRole === "alumno" || userRole === "tutor" || userRole === "padre")) {
          setError("No autorizado para ver esta sección.");
          setLoading(false);
          return;
        }

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
  }, [userId, userRole]);

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
