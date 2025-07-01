import React, { useEffect, useState } from "react";
import axios from "axios";
import "./CSS/UsersTable.css";

const SetGrades = ({ materiaProfesor, horarioId, onBack }) => {
  const [alumnos, setAlumnos] = useState([]);
  const [loading, setLoading] = useState(true);
  const [calificaciones, setCalificaciones] = useState({});
  const [error, setError] = useState(null);
  const [cicloActual, setCicloActual] = useState(null);

  useEffect(() => {
    if (!materiaProfesor) return;

    const fetchData = async () => {
      setLoading(true);
      setError(null);

      try {
        let resIds;
        if (!horarioId || horarioId === "null" || horarioId === "Taller") {
          resIds = await axios.get(`http://localhost:5002/api/Schedule/alumnosPorTaller/${materiaProfesor}`);
        } else {
          resIds = await axios.get("http://localhost:5002/api/Schedule/alumnosPorMateriaHorario", {
            params: { materiaProfesor, horario: horarioId },
          });
        }

        const alumnoIds = Array.from(new Set(resIds.data));
        if (!alumnoIds.length) {
          setAlumnos([]);
          setLoading(false);
          return;
        }

        const resAlumnos = await axios.post("http://localhost:5000/api/usuario/obtenerusuariosporids", alumnoIds);
        const alumnosData = resAlumnos.data;

        if (!alumnosData || alumnosData.length === 0) {
          setAlumnos([]);
          setLoading(false);
          return;
        }

        const resCiclo = await axios.get("http://localhost:5004/api/CicloEscolar/actual");
        const ciclo = resCiclo.data?.id || null;
        setCicloActual(ciclo);

        const resExistentes = await axios.post(
          `http://localhost:5004/api/calificacion/existentes/clase/${materiaProfesor}`,
          alumnoIds
        );
        const califExistentes = resExistentes.data || [];

        const initialCalificaciones = {};
        alumnosData.forEach(a => {
          const existente = califExistentes.find(c => c.idAlumno === a.id);
          initialCalificaciones[a.id] = {
            calificacionFinal: existente?.calificacionFinal ?? "",
            comentarios: existente?.comentarios ?? "",
            id: existente?.id ?? 0,
          };
        });

        setAlumnos(alumnosData);
        setCalificaciones(initialCalificaciones);
      } catch (err) {
        console.error("Error cargando alumnos o calificaciones:", err);
        setError("No existen datos para esta sección. Por favor, intente más tarde."); // ERR3
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [materiaProfesor, horarioId]);

  const handleCalificacionChange = (id, value) => {
    const val = value === "" ? "" : Number(value);
    if (val === "" || (val >= 0 && val <= 10)) {
      setCalificaciones(prev => ({
        ...prev,
        [id]: { ...prev[id], calificacionFinal: val },
      }));
    }
  };

  const handleComentariosChange = (id, text) => {
    setCalificaciones(prev => ({
      ...prev,
      [id]: { ...prev[id], comentarios: text },
    }));
  };

  const handleGuardar = async () => {
    if (!cicloActual) {
      alert("Los datos ingresados no son válidos"); // ERR1
      return;
    }

    try {
      const requests = alumnos.map(a => {
        const calif = calificaciones[a.id];
        if (calif.calificacionFinal === "" || calif.calificacionFinal === null) return null;

        const payload = {
          id: calif.id || 0,
          idMateria: materiaProfesor,
          idAlumno: a.id,
          calificacionFinal: calif.calificacionFinal,
          comentarios: calif.comentarios,
          idCiclo: cicloActual,
        };

        return calif.id && calif.id > 0
          ? axios.put(`http://localhost:5004/api/calificacion/${calif.id}`, payload)
          : axios.post("http://localhost:5004/api/calificacion", payload);
      }).filter(Boolean);

      await Promise.all(requests);
      alert("Elemento registrado exitosamente."); // MSG3
      if (onBack) onBack();
    } catch (err) {
      console.error("Error guardando calificaciones:", err);
      alert("Error de conexión al servidor. Intenta nuevamente."); // ERR6
    }
  };

  if (loading) return <p>Cargando alumnos y ciclo escolar...</p>;
  if (error) return <p style={{ color: "red" }}>{error}</p>;
  if (alumnos.length === 0) return <p>No existen datos para esta sección. Por favor, intente más tarde.</p>; // ERR3

  return (
    <div className="users-table-container">
      <h2>Registrar Calificaciones - {materiaProfesor} - Horario {horarioId || "Taller"}</h2>
      <table>
        <thead>
          <tr>
            <th>Nombre del Alumno</th>
            <th>Calificación (0-10)</th>
            <th>Comentarios</th>
          </tr>
        </thead>
        <tbody>
          {alumnos.map(alumno => (
            <tr key={alumno.id}>
              <td>{alumno.nombreCompleto}</td>
              <td>
                <input
                  type="number"
                  min="0"
                  max="10"
                  step="0.1"
                  value={calificaciones[alumno.id]?.calificacionFinal || ""}
                  onChange={e => handleCalificacionChange(alumno.id, e.target.value)}
                  placeholder="Ej: 8.5"
                />
              </td>
              <td>
                <input
                  type="text"
                  value={calificaciones[alumno.id]?.comentarios || ""}
                  onChange={e => handleComentariosChange(alumno.id, e.target.value)}
                  placeholder="Comentarios (opcional)"
                />
              </td>
            </tr>
          ))}
        </tbody>
      </table>

      <button onClick={handleGuardar} style={{ marginTop: 10 }}>
        Guardar Calificaciones
      </button>
      <button onClick={onBack} style={{ marginTop: 10, marginLeft: 10 }}>
        Volver
      </button>
    </div>
  );
};

export default SetGrades;
