import React, { useEffect, useState, useContext } from "react";
import axios from "axios";
import { AuthContext } from "../Context/AuthContext";
import "./CSS/UsersTable.css";

const ShowGrades = () => {
  const { auth } = useContext(AuthContext);
  const userId = auth.user?.id;
  const userRole = auth.user?.role?.toLowerCase();

  const [calificaciones, setCalificaciones] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [cicloActual, setCicloActual] = useState(null);

  useEffect(() => {
    const fetchCalificaciones = async () => {
      setLoading(true);
      setError(null);

      try {
        // 1. Obtener ciclo escolar actual
        const resCiclo = await axios.get("http://localhost:5004/api/CicloEscolar/actual");
        const cicloId = resCiclo.data?.id;

        if (!cicloId) {
          setError("No se encontró un ciclo escolar actual.");
          setLoading(false);
          return;
        }

        setCicloActual(cicloId);
        console.log("📘 Ciclo escolar actual:", cicloId);

        // 2. Determinar ID del alumno (si es tutor, buscar hijo)
        let alumnoId = userId;

        if (userRole === "tutor" || userRole === "padre") {
          const resAlumno = await axios.get(`http://localhost:5000/api/usuario/obtenerAlumnoPorTutor/${userId}`);
          if (resAlumno.data?.id) {
            alumnoId = resAlumno.data.id;
          } else {
            setError("No se encontró un alumno asociado al tutor.");
            setLoading(false);
            return;
          }
        }

        // 3. Obtener calificaciones del alumno para el ciclo
        const resCalificaciones = await axios.get(
          `http://localhost:5004/api/calificacion/alumno/${alumnoId}/ciclo/${cicloId}`
        );
        console.log("📄 Calificaciones:", resCalificaciones.data);

        setCalificaciones(resCalificaciones.data || []);
      } catch (err) {
        console.error("❌ Error obteniendo calificaciones:", err);
        setError("Error al obtener calificaciones.");
      } finally {
        setLoading(false);
      }
    };

    if (userId) {
      fetchCalificaciones();
    }
  }, [userId, userRole]);

  if (loading) return <p>Cargando calificaciones...</p>;
  if (error) return <p style={{ color: "red" }}>{error}</p>;
  if (calificaciones.length === 0) return <p>No se encontraron calificaciones para el ciclo actual.</p>;

  return (
    <div className="users-table-container">
      <h2>Calificaciones - Ciclo {cicloActual}</h2>
      <table>
        <thead>
          <tr>
            <th>ID Materia</th>
            <th>Calificación</th>
            <th>Comentarios</th>
          </tr>
        </thead>
        <tbody>
          {calificaciones.map((calif) => (
            <tr key={calif.id}>
              <td>{calif.idMateria}</td>
              <td>{calif.calificacionFinal}</td>
              <td>{calif.comentarios || "-"}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default ShowGrades;
