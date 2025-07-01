import React, { useEffect, useState, useContext } from "react";
import axios from "axios";
import "./CSS/UsersTable.css";
import { AuthContext } from "../Context/AuthContext";

const TeacherClassesTable = ({ onRegistrarAsistencia, onRegistrarCalificacion }) => {
  const { auth } = useContext(AuthContext);
  const teacherId = auth.user?.id;

  const [classes, setClasses] = useState([]);
  const [loading, setLoading] = useState(true);
  const [selectedHorarios, setSelectedHorarios] = useState({});
  const [error, setError] = useState(null);

  useEffect(() => {
    if (!teacherId) return;

    const fetchTeacherClassesAndSchedules = async () => {
      setLoading(true);
      setError(null);
      try {
        const scheduleRes = await axios.get(`http://localhost:5002/api/Schedule/horarioDocente/${teacherId}`);

        const materiasMap = new Map();
        scheduleRes.data.forEach((item) => {
          const codigoMateria = item.idMateria.split("-")[0];
          const idMateriaCompleto = `${codigoMateria}-${teacherId}`;

          if (!materiasMap.has(idMateriaCompleto)) {
            materiasMap.set(idMateriaCompleto, {
              id: idMateriaCompleto,
              codigoBase: codigoMateria,
              horariosIds: new Set(),
            });
          }

          if (item.idHorario) {
            materiasMap.get(idMateriaCompleto).horariosIds.add(item.idHorario);
          }
        });

        const materias = await Promise.all(
          Array.from(materiasMap.values()).map(async (materia) => {
            let nombre = "Nombre no disponible";
            try {
              const res = await axios.get(`http://localhost:5001/api/Subject/obtenerPorCodigo/${materia.codigoBase}`);
              nombre = res.data.nombre || nombre;
            } catch (err) {
              // Mensaje adaptado
              console.warn("No existen datos para esta sección. Por favor, intente más tarde."); // ERR3
            }

            const horarios = await Promise.all(
              Array.from(materia.horariosIds).map(async (idHorario) => {
                try {
                  const resHorario = await axios.get(`http://localhost:5002/api/Schedule/${idHorario}`);
                  return {
                    id: idHorario,
                    grado: resHorario.data.grado,
                    grupo: resHorario.data.grupo,
                  };
                } catch (err) {
                  console.warn("No existen datos para esta sección. Por favor, intente más tarde."); // ERR3
                  return null;
                }
              })
            );

            const horariosValidos = horarios.filter((h) => h !== null);

            return {
              id: materia.id,
              nombre,
              horarios: horariosValidos,
            };
          })
        );

        if (materias.length === 0) {
          setError("No existen datos para esta sección. Por favor, intente más tarde."); // ERR3
        }

        setClasses(materias);

        const initialSelected = {};
        materias.forEach((m) => {
          if (m.horarios.length > 0) {
            initialSelected[m.id] = `${m.horarios[0].grado}${m.horarios[0].grupo}`;
          }
        });
        setSelectedHorarios(initialSelected);
      } catch (error) {
        console.error("No existen datos para esta sección. Por favor, intente más tarde."); // ERR3
        setError("No existen datos para esta sección. Por favor, intente más tarde.");
        setClasses([]);
      } finally {
        setLoading(false);
      }
    };

    fetchTeacherClassesAndSchedules();
  }, [teacherId]);

  const handleHorarioChange = (materiaId, horarioString) => {
    setSelectedHorarios((prev) => ({
      ...prev,
      [materiaId]: horarioString,
    }));
  };

  return (
    <div className="users-table-container">
      <h2>Clases del maestro</h2>
      {loading ? (
        <p>Cargando clases...</p>
      ) : error ? (
        <p style={{ color: "red" }}>{error}</p>
      ) : (
        <table>
          <thead>
            <tr>
              <th>ID Materia</th>
              <th>Nombre de la Materia</th>
              <th>Horarios</th>
              <th>Acciones</th>
            </tr>
          </thead>
          <tbody>
            {classes.length === 0 ? (
              <tr>
                <td colSpan={4}>No existen datos para esta sección. Por favor, intente más tarde.</td> {/* ERR3 */}
              </tr>
            ) : (
              classes.map((clase) => (
                <tr key={clase.id}>
                  <td>{clase.id}</td>
                  <td>{clase.nombre}</td>
                  <td>
                    {clase.horarios.length > 0 ? (
                      <select
                        value={selectedHorarios[clase.id] || `${clase.horarios[0].grado}${clase.horarios[0].grupo}`}
                        onChange={(e) => handleHorarioChange(clase.id, e.target.value)}
                      >
                        {clase.horarios.map((horario) => (
                          <option key={horario.id} value={`${horario.grado}${horario.grupo}`}>
                            {`${horario.grado}${horario.grupo}`}
                          </option>
                        ))}
                      </select>
                    ) : (
                      <span>Taller</span>
                    )}
                  </td>
                  <td>
                    <button
                      onClick={() => {
                        const horarioStr =
                          clase.horarios.length > 0 ? selectedHorarios[clase.id] : null;
                        if (typeof onRegistrarAsistencia === "function") {
                          onRegistrarAsistencia(clase.id, horarioStr);
                        }
                      }}
                    >
                      Registrar asistencia
                    </button>
                    <button
                      onClick={() => {
                        const horarioStr =
                          clase.horarios.length > 0 ? selectedHorarios[clase.id] : null;
                        if (typeof onRegistrarCalificacion === "function") {
                          onRegistrarCalificacion(clase.id, horarioStr);
                        }
                      }}
                    >
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
