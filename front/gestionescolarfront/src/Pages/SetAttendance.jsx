import React, { useEffect, useState } from "react";
import axios from "axios";
import "./CSS/UsersTable.css";

const SetAttendance = ({ claseProfesor, horarioId }) => {
  const [alumnos, setAlumnos] = useState([]);
  const [loading, setLoading] = useState(true);
  const [asistencias, setAsistencias] = useState({});
  const [error, setError] = useState(null);
  const [selectedDate, setSelectedDate] = useState(new Date().toISOString().slice(0, 10));

  useEffect(() => {
    if (!claseProfesor) return;

    const fetchAsistencias = async () => {
      setLoading(true);
      setError(null);

      try {
        let resIds;
        if (!horarioId || horarioId === "null" || horarioId === "Taller") {
          resIds = await axios.get(`http://localhost:5002/api/Schedule/alumnosPorTaller/${claseProfesor}`);
        } else {
          resIds = await axios.get(`http://localhost:5002/api/Schedule/alumnosPorMateriaHorario`, {
            params: { materiaProfesor: claseProfesor, horario: horarioId },
          });
        }

        const alumnoIds = Array.from(new Set(resIds.data));
        if (!alumnoIds || alumnoIds.length === 0) {
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

        const initialAsistencias = {};
        alumnosData.forEach(a => {
          initialAsistencias[a.id] = { asistio: false, justificacion: "" };
        });

        setAlumnos(alumnosData);
        setAsistencias(initialAsistencias);

        const resAsistencias = await axios.post(
          `http://localhost:5004/api/asistencias/profesor/${claseProfesor}/fecha/${selectedDate}`,
          alumnoIds
        );

        if (resAsistencias.data?.length > 0) {
          const asistenciasMap = {};
          resAsistencias.data.forEach(asistencia => {
            asistenciasMap[asistencia.idAlumno] = {
              asistio: asistencia.asistio,
              justificacion: asistencia.justificacion || "",
              id: asistencia.id,
            };
          });
          setAsistencias(asistenciasMap);
        }
      } catch (err) {
        console.error("Error cargando datos de alumnos o asistencias:", err);
        setError("No existen datos para esta sección. Por favor, intente más tarde."); // ERR3
      } finally {
        setLoading(false);
      }
    };

    fetchAsistencias();
  }, [claseProfesor, horarioId, selectedDate]);

  const handleCheckboxChange = id => {
    setAsistencias(prev => ({
      ...prev,
      [id]: { ...prev[id], asistio: !prev[id].asistio },
    }));
  };

  const handleJustificacionChange = (id, text) => {
    setAsistencias(prev => ({
      ...prev,
      [id]: { ...prev[id], justificacion: text },
    }));
  };

  const handleDateChange = e => setSelectedDate(e.target.value);

  const handleGuardar = async () => {
    try {
      const requests = alumnos.map(a => {
        const asistencia = asistencias[a.id];
        const payload = {
          id: asistencia.id || 0,
          fecha: selectedDate + "T00:00:00",
          asistio: asistencia.asistio,
          justificacion: asistencia.justificacion,
          idAlumno: a.id,
          idProfesor: claseProfesor,
        };
        return asistencia.id
          ? axios.put("http://localhost:5004/api/asistencias", payload)
          : axios.post("http://localhost:5004/api/asistencias", payload);
      });

      await Promise.all(requests);
      alert("Elemento registrado exitosamente."); // MSG3
    } catch (err) {
      console.error("Error guardando asistencias:", err);
      alert("Error de conexión al servidor. Intenta nuevamente."); // ERR6
    }
  };

  if (loading) return <p>Cargando alumnos...</p>;
  if (error) return <p style={{ color: "red" }}>{error}</p>;
  if (alumnos.length === 0) return <p>No existen datos para esta sección. Por favor, intente más tarde.</p>; // ERR3

  return (
    <div className="users-table-container">
      <h2>Registrar Asistencia - {claseProfesor} - {horarioId || "Taller"}</h2>

      <label>Seleccionar Fecha</label>
      <input type="date" value={selectedDate} onChange={handleDateChange} />

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
          {alumnos.map(alumno => (
            <tr key={alumno.id}>
              <td>{alumno.nombreCompleto}</td>
              <td><input type="date" value={selectedDate} disabled /></td>
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
                  onChange={e => handleJustificacionChange(alumno.id, e.target.value)}
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
