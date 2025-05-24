import React, { useEffect, useState, useContext } from "react";
import axios from "axios";
import { AuthContext } from "../Context/AuthContext";
import "./CSS/UsersTable.css";

const ShowGrades = () => {
  const { auth } = useContext(AuthContext);
  const userId = auth.user?.id;
  const userRole = auth.user?.role?.toLowerCase();

  const [grades, setGrades] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [activeCycle, setActiveCycle] = useState(null);

  useEffect(() => {
    if (!userId) return;

    const fetchGrades = async () => {
      setLoading(true);
      setError(null);
      try {
        // 1. Obtener ciclo activo
        const cycleRes = await axios.get("http://localhost:5004/api/CicloEscolar/actual");
        const cicloActual = cycleRes.data?.id;
        setActiveCycle(cicloActual);

        if (!cicloActual) {
          setError("No se encontró un ciclo escolar activo.");
          setGrades([]);
          setLoading(false);
          return;
        }

        // Si es tutor, obtener el id del alumno (hijo)
        let alumnoId = userId;
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

        // 2. Obtener calificaciones por alumno y ciclo
        const gradesRes = await axios.get(
          `http://localhost:5004/api/calificacion/alumno/${alumnoId}/ciclo/${cicloActual}`
        );
        setGrades(gradesRes.data);
      } catch (err) {
        console.error("Error al obtener calificaciones:", err);
        setError("Error al cargar las calificaciones. Intente nuevamente.");
      } finally {
        setLoading(false);
      }
    };

    fetchGrades();
  }, [userId, userRole]);

  if (loading) return <p>Cargando calificaciones...</p>;
  if (error) return <p style={{ color: "red" }}>{error}</p>;
  if (!grades.length) return <p>No se encontraron calificaciones para el ciclo escolar actual.</p>;

  return (
    <div className="users-table-container">
      <h2>Calificaciones - Ciclo Escolar {activeCycle}</h2>
      <table>
        <thead>
          <tr>
            <th>ID Materia</th>
            <th>Calificación Final</th>
            <th>Comentarios</th>
          </tr>
        </thead>
        <tbody>
          {grades.map((grade) => (
            <tr key={grade.id}>
              <td>{grade.idMateria}</td>
              <td>{grade.calificacionFinal}</td>
              <td>{grade.comentarios || "-"}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default ShowGrades;
