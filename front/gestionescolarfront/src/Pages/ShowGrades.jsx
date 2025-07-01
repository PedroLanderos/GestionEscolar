import React, { useEffect, useState, useContext } from "react";
import axios from "axios";
import { AuthContext } from "../Context/AuthContext";
import "./CSS/UsersTable.css";

const ShowGrades = ({ modo = "ciclo", onBack }) => {
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
        let cicloId = null;

        if (modo === "ciclo") {
          // Obtener ciclo escolar actual
          const resCiclo = await axios.get("http://localhost:5004/api/CicloEscolar/actual");
          cicloId = resCiclo.data?.id;

          if (!cicloId) {
            setError("No existen datos para esta sección. Por favor, intente más tarde.");
            setLoading(false);
            return;
          }
          setCicloActual(cicloId);
        }

        // Determinar ID del alumno (si es tutor o padre, buscar hijo)
        let alumnoId = userId;

        if (userRole === "tutor" || userRole === "padre") {
          const resAlumno = await axios.get(`http://localhost:5000/api/usuario/obtenerAlumnoPorTutor/${userId}`);
          if (resAlumno.data?.id) {
            alumnoId = resAlumno.data.id;
          } else {
            setError("No hay suficientes datos registrados para usar esta opción. Por favor, registre más datos e intente nuevamente.");
            setLoading(false);
            return;
          }
        }

        // Obtener calificaciones
        let resCalificaciones;
        if (modo === "ciclo") {
          resCalificaciones = await axios.get(
            `http://localhost:5004/api/calificacion/alumno/${alumnoId}/ciclo/${cicloId}`
          );
        } else {
          // historial completo
          resCalificaciones = await axios.get(
            `http://localhost:5004/api/calificacion/alumno/${alumnoId}`
          );
        }

        setCalificaciones(resCalificaciones.data || []);
      } catch (err) {
        console.error("Error obteniendo calificaciones:", err);
        setError("Error de conexión al servidor. Intenta nuevamente.");
      } finally {
        setLoading(false);
      }
    };

    if (userId) {
      fetchCalificaciones();
    }
  }, [userId, userRole, modo]);

  if (loading) return <p>Cargando calificaciones...</p>;
  if (error) return (
    <>
      <p style={{ color: "red" }}>{error}</p>
      {onBack && <button onClick={onBack}>Volver</button>}
    </>
  );
  if (calificaciones.length === 0)
    return (
      <>
        <p>No existen datos para esta sección. Por favor, intente más tarde.</p>
        {onBack && <button onClick={onBack}>Volver</button>}
      </>
    );

  return (
    <div className="users-table-container">
      <h2>Calificaciones {modo === "ciclo" ? `- Ciclo ${cicloActual}` : "- Historial Completo"}</h2>
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
              <td>{calif.comentarios || "No existen datos para esta sección. Por favor, intente más tarde."}</td>
            </tr>
          ))}
        </tbody>
      </table>
      {onBack && <button onClick={onBack} style={{ marginTop: "1rem" }}>Volver</button>}
    </div>
  );
};

export default ShowGrades;
