import React, { useEffect, useState, useContext } from "react";
import axios from "axios";
import { AUTH_API } from "../Config/apiConfig";
import "./CSS/UsersTable.css";
import { AuthContext } from "../Context/AuthContext";

const TeacherClassesTable = () => {
  const { auth } = useContext(AuthContext);
  const teacherId = auth.user?.id;

  const [classes, setClasses] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    if (!teacherId) return;

    const fetchTeacherClasses = async () => {
      setLoading(true);
      try {
        // Obtener horario docente del endpoint correcto
        const scheduleRes = await axios.get(`http://localhost:5002/api/Schedule/horarioDocente/${teacherId}`);

        // Extraer códigos únicos de materia (parte antes del guion)
        const materiaCodesSet = new Set(
          scheduleRes.data.map((item) => item.idMateria.split("-")[0])
        );

        // Para cada código obtener nombre materia del endpoint correcto
        const materias = await Promise.all(
          Array.from(materiaCodesSet).map(async (codigo) => {
            try {
              const res = await axios.get(`http://localhost:5001/api/Subject/obtenerPorCodigo/${codigo}`);
              return {
                id: codigo,
                nombre: res.data.nombre || "Nombre no disponible",
              };
            } catch {
              return {
                id: codigo,
                nombre: "Nombre no disponible",
              };
            }
          })
        );

        setClasses(materias);
      } catch (error) {
        console.error("❌ Error al obtener las clases del maestro:", error);
        setClasses([]);
      } finally {
        setLoading(false);
      }
    };

    fetchTeacherClasses();
  }, [teacherId]);

  return (
    <div className="users-table-container">
      <h2>Clases del maestro</h2>
      {loading ? (
        <p>Cargando clases...</p>
      ) : (
        <table>
          <thead>
            <tr>
              <th>ID Materia</th>
              <th>Nombre de la Materia</th>
              <th>Acciones</th>
            </tr>
          </thead>
          <tbody>
            {classes.length === 0 ? (
              <tr>
                <td colSpan={3}>No se encontraron clases para este maestro.</td>
              </tr>
            ) : (
              classes.map((clase) => (
                <tr key={clase.id}>
                  <td>{clase.id}</td>
                  <td>{clase.nombre}</td>
                  <td>
                    <button onClick={() => alert("Registrar asistencia aún no implementado")}>
                      Registrar asistencia
                    </button>
                    <button onClick={() => alert("Registrar calificación aún no implementado")}>
                      Registrar calificación
                    </button>
                  </td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      )}
    </div>
  );
};

export default TeacherClassesTable;
