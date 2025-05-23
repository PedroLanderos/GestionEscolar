import React, { useEffect, useState } from "react";
import axios from "axios";
import "./CSS/UsersTable.css";

const SetAttendance = ({ claseProfesor, horarioId }) => {
  const [alumnos, setAlumnos] = useState([]);
  const [loading, setLoading] = useState(true);
  const [asistencias, setAsistencias] = useState({});
  const [error, setError] = useState(null);

  // Fecha de hoy en formato ISO sin zona (YYYY-MM-DD)
  const todayDate = new Date().toISOString().slice(0, 10);

  useEffect(() => {
    if (!claseProfesor || !horarioId) return;

    const fetchAlumnos = async () => {
      setLoading(true);
      setError(null);
      try {
        console.log(
          `Llamando a alumnosPorMateriaHorario con materiaProfesor=${claseProfesor} y horario=${horarioId}`
        );

        // 1. Obtener IDs alumnos inscritos
        const resIds = await axios.get(`http://localhost:5002/api/Schedule/alumnosPorMateriaHorario`, {
          params: { materiaProfesor: claseProfesor, horario: horarioId },
        });
        const alumnoIds = resIds.data;

        console.log("IDs de alumnos obtenidos:", alumnoIds);

        if (!alumnoIds.length) {
          setAlumnos([]);
          setLoading(false);
          return;
        }

        console.log("Enviando IDs para obtener datos de alumnos:", alumnoIds);

        // 2. Obtener datos de alumnos
        const resAlumnos = await axios.post("http://localhost:5000/api/usuario/obtenerusuariosporids", alumnoIds);
        const alumnosData = resAlumnos.data;

        // Inicializar estado de asistencias: { alumnoId: { asistio: false, justificacion: "" } }
        const initialAsistencias = {};
        alumnosData.forEach((a) => {
          initialAsistencias[a.id] = { asistio: false, justificacion: "" };
        });

        setAlumnos(alumnosData);
        setAsistencias(initialAsistencias);
      } catch (err) {
        console.error("Error cargando alumnos o asistencias:", err);
        setError("Error cargando datos, intenta más tarde.");
      } finally {
        setLoading(false);
      }
    };

    fetchAlumnos();
  }, [claseProfesor, horarioId]);

  const handleCheckboxChange = (id) => {
    setAsistencias((prev) => ({
      ...prev,
      [id]: { ...prev[id], asistio: !prev[id].asistio },
    }));
  };

  const handleJustificacionChange = (id, text) => {
    setAsistencias((prev) => ({
      ...prev,
      [id]: { ...prev[id], justificacion: text },
    }));
  };

  const handleGuardar = async () => {
    try {
      const requests = alumnos.map((a) => {
        const asistencia = asistencias[a.id];
        const payload = {
          id: 0,
          fecha: todayDate + "T00:00:00",
          asistio: asistencia.asistio,
          justificacion: asistencia.justificacion,
          idAlumno: a.id,
          idProfesor: claseProfesor, // idMateria-idProfesor
        };
        console.log("Enviando asistencia:", payload);
        return axios.post("http://localhost:5004/api/asistencias", payload);
      });

      await Promise.all(requests);
      alert("Asistencias guardadas correctamente.");
    } catch (err) {
      console.error("Error guardando asistencias:", err);
      alert("Error al guardar asistencias. Intenta nuevamente.");
    }
  };

  if (loading) return <p>Cargando alumnos...</p>;
  if (error) return <p style={{ color: "red" }}>{error}</p>;
  if (alumnos.length === 0) return <p>No hay alumnos inscritos para esta clase y horario.</p>;

  return (
    <div className="users-table-container">
      <h2>Registrar Asistencia - {claseProfesor} - {horarioId}</h2>
      <table>
        <thead>
          <tr>
            <th>Nombre del Alumno</th>
            <th>Fecha</th>
            <th>Asistió</th>
            <th>Justificación</th>
          </tr>
        </thead>
        <tbody>
          {alumnos.map((alumno) => (
            <tr key={alumno.id}>
              <td>{alumno.nombreCompleto}</td>
              <td>
                <input type="date" value={todayDate} disabled />
              </td>
              <td>
                <input
                  type="checkbox"
                  checked={asistencias[alumno.id]?.asistio || false}
                  onChange={() => handleCheckboxChange(alumno.id)}
                />
              </td>
              <td>
                <input
                  type="text"
                  value={asistencias[alumno.id]?.justificacion || ""}
                  onChange={(e) => handleJustificacionChange(alumno.id, e.target.value)}
                  placeholder="Justificación (opcional)"
                />
              </td>
            </tr>
          ))}
        </tbody>
      </table>
      <button onClick={handleGuardar} style={{ marginTop: 10 }}>
        Guardar Asistencias
      </button>
    </div>
  );
};

export default SetAttendance;
