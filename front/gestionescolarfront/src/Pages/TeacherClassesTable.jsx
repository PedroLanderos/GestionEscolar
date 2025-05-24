import React, { useEffect, useState, useContext } from "react";
import axios from "axios";
import "./CSS/UsersTable.css";
import { AuthContext } from "../Context/AuthContext";

const TeacherClassesTable = ({ onRegistrarAsistencia, onRegistrarCalificacion }) => {
  const { auth } = useContext(AuthContext);
  const teacherId = auth.user?.id;

  const [classes, setClasses] = useState([]); // [{id, nombre, horarios: [{id, grado, grupo}]}]
  const [loading, setLoading] = useState(true);
  const [selectedHorarios, setSelectedHorarios] = useState({}); // Guardará el string "1A", "2B", etc.

  useEffect(() => {
    if (!teacherId) return;

    const fetchTeacherClassesAndSchedules = async () => {
      setLoading(true);
      try {
        const scheduleRes = await axios.get(`http://localhost:5002/api/Schedule/horarioDocente/${teacherId}`);

        const materiasMap = new Map();
        scheduleRes.data.forEach((item) => {
          const codigoMateria = item.idMateria.split("-")[0];
          if (!materiasMap.has(codigoMateria)) {
            materiasMap.set(codigoMateria, {
              id: codigoMateria,
              horariosIds: new Set(),
            });
          }
          materiasMap.get(codigoMateria).horariosIds.add(item.idHorario);
        });

        const materias = await Promise.all(
          Array.from(materiasMap.values()).map(async (materia) => {
            let nombre = "Nombre no disponible";
            try {
              const res = await axios.get(`http://localhost:5001/api/Subject/obtenerPorCodigo/${materia.id}`);
              nombre = res.data.nombre || nombre;
            } catch {}

            const horarios = await Promise.all(
              Array.from(materia.horariosIds).map(async (idHorario) => {
                try {
                  const resHorario = await axios.get(`http://localhost:5002/api/Schedule/${idHorario}`);
                  return {
                    id: idHorario,
                    grado: resHorario.data.grado,
                    grupo: resHorario.data.grupo,
                  };
                } catch {
                  return null;
                }
              })
            );

            const horariosValidos = horarios.filter(h => h !== null);

            return {
              id: materia.id,
              nombre,
              horarios: horariosValidos,
            };
          })
        );

        setClasses(materias);

        // Inicializar el horario seleccionado por materia con el string grado+grupo del primer horario válido
        const initialSelected = {};
        materias.forEach(m => {
          if (m.horarios.length > 0) initialSelected[m.id] = `${m.horarios[0].grado}${m.horarios[0].grupo}`;
        });
        setSelectedHorarios(initialSelected);
      } catch (error) {
        console.error("❌ Error al obtener las clases y horarios del maestro:", error);
        setClasses([]);
      } finally {
        setLoading(false);
      }
    };

    fetchTeacherClassesAndSchedules();
  }, [teacherId]);

  const handleHorarioChange = (materiaId, horarioString) => {
    setSelectedHorarios(prev => ({
      ...prev,
      [materiaId]: horarioString, // Guarda "1A", "2B", etc.
    }));
  };

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
              <th>Horarios</th>
              <th>Acciones</th>
            </tr>
          </thead>
          <tbody>
            {classes.length === 0 ? (
              <tr>
                <td colSpan={4}>No se encontraron clases para este maestro.</td>
              </tr>
            ) : (
              classes.map((clase) => (
                <tr key={clase.id}>
                  <td>{clase.id}</td>
                  <td>{clase.nombre}</td>
                  <td>
                    <select
                      value={selectedHorarios[clase.id] || (clase.horarios[0] && `${clase.horarios[0].grado}${clase.horarios[0].grupo}`)}
                      onChange={(e) => handleHorarioChange(clase.id, e.target.value)}
                    >
                      {clase.horarios.map((horario) => (
                        <option key={horario.id} value={`${horario.grado}${horario.grupo}`}>
                          {`${horario.grado}${horario.grupo}`}
                        </option>
                      ))}
                    </select>
                  </td>
                  <td>
                    <button
                      onClick={() => {
                        const horario = selectedHorarios[clase.id];
                        if (!horario) {
                          alert("Selecciona un horario antes de registrar asistencia.");
                          return;
                        }
                        if (typeof onRegistrarAsistencia === "function") {
                          onRegistrarAsistencia(`${clase.id}-${teacherId}`, horario);
                        }
                      }}
                    >
                      Registrar asistencia
                    </button>
                    <button
                      onClick={() => {
                        const horario = selectedHorarios[clase.id];
                        if (!horario) {
                          alert("Selecciona un horario antes de registrar calificación.");
                          return;
                        }
                        if (typeof onRegistrarCalificacion === "function") {
                          onRegistrarCalificacion(`${clase.id}-${teacherId}`, horario);
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
