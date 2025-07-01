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
      setError("Acción no permitida. No tiene los permisos suficientes."); // ERR4
      setLoading(false);
      return;
    }

    const fetchAsistencias = async () => {
      setLoading(true);
      setError(null);

      try {
        let alumnoId = userId;

        // Si es tutor o padre, obtener id de alumno (hijo)
        if (userRole === "tutor" || userRole === "padre") {
          try {
            const res = await axios.get(`http://localhost:5000/api/usuario/obtenerAlumnoPorTutor/${userId}`);
            if (res.data && res.data.id) {
              alumnoId = res.data.id;
            } else {
              setError("No hay suficientes datos registrados para usar esta opción. Por favor, registre más datos e intente nuevamente."); // ERR2
              setLoading(false);
              return;
            }
          } catch {
            setError("Los datos ingresados no son válidos"); // ERR1
            setLoading(false);
            return;
          }
        }

        // Solo alumnos, tutores y padres pueden ver asistencias
        if (!(userRole === "alumno" || userRole === "tutor" || userRole === "padre")) {
          setError("Acción no permitida. No tiene los permisos suficientes."); // ERR4
          setLoading(false);
          return;
        }

        const res = await axios.get(`http://localhost:5004/api/asistencias/alumno/${alumnoId}`);
        setAsistencias(res.data);
      } catch (err) {
        console.error("Error al obtener asistencias:", err);
        setError("Error de conexión al servidor. Intenta nuevamente."); // ERR6
      } finally {
        setLoading(false);
      }
    };

    fetchAsistencias();
  }, [userId, userRole]);

  if (loading) return <p>Cargando asistencias...</p>;
  if (error) return <p style={{ color: "red" }}>{error}</p>;
  if (asistencias.length === 0) return <p>No existen datos para esta sección. Por favor, intente más tarde.</p>; // ERR3

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
              <td>{asistencia.justificacion || "No existen datos para esta sección. Por favor, intente más tarde." /* ERR3 */}</td>
              <td>{asistencia.idProfesor}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default ShowAttendance;
