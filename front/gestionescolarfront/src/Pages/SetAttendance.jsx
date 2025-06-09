import React, { useEffect, useState } from "react";
import axios from "axios";
import "./CSS/UsersTable.css";

const SetAttendance = ({ claseProfesor, horarioId }) => {
  const [alumnos, setAlumnos] = useState([]);
  const [loading, setLoading] = useState(true);
  const [asistencias, setAsistencias] = useState({});
  const [error, setError] = useState(null);
  const [selectedDate, setSelectedDate] = useState(new Date().toISOString().slice(0, 10)); // Fecha por defecto es hoy

  useEffect(() => {
    if (!claseProfesor || !horarioId) return;

    const fetchAsistencias = async () => {
      setLoading(true);
      setError(null);
      try {
        // Obtener IDs de alumnos inscritos
        const resIds = await axios.get(`http://localhost:5002/api/Schedule/alumnosPorMateriaHorario`, {
          params: { materiaProfesor: claseProfesor, horario: horarioId },
        });
        const alumnoIds = resIds.data;

        if (!alumnoIds.length) {
          setAlumnos([]);
          setLoading(false);
          return;
        }

        // Obtener datos de los alumnos
        const resAlumnos = await axios.post("http://localhost:5000/api/usuario/obtenerusuariosporids", alumnoIds);
        const alumnosData = resAlumnos.data;

        // Inicializar el estado de asistencias
        const initialAsistencias = {};
        alumnosData.forEach((a) => {
          initialAsistencias[a.id] = { asistio: false, justificacion: "" };
        });

        setAlumnos(alumnosData);
        setAsistencias(initialAsistencias);

        // Ahora buscamos si ya existen asistencias para esta fecha
        const resAsistencias = await axios.post("http://localhost:5004/api/asistencias/profesor/" + claseProfesor + "/fecha/" + selectedDate, alumnoIds);
        
        // Si ya hay asistencias, las ponemos en el estado
        if (resAsistencias.data && resAsistencias.data.length > 0) {
          const asistenciasMap = {};
          resAsistencias.data.forEach((asistencia) => {
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
        setError("Error cargando datos, intenta más tarde.");
      } finally {
        setLoading(false);
      }
    };

    fetchAsistencias();
  }, [claseProfesor, horarioId, selectedDate]);

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

  const handleDateChange = (e) => {
    setSelectedDate(e.target.value);
  };

  const handleGuardar = async () => {
    try {
      const requests = alumnos.map((a) => {
        const asistencia = asistencias[a.id];
        const payload = {
          id: asistencia.id || 0,  // Si la asistencia no tiene ID, es nueva
          fecha: selectedDate + "T00:00:00", // Fecha completa con hora
          asistio: asistencia.asistio,
          justificacion: asistencia.justificacion,
          idAlumno: a.id,
          idProfesor: claseProfesor, // idMateria-idProfesor
        };

        if (asistencia.id) {
          // Si ya existe una asistencia (ID presente), se debe actualizar
          return axios.put("http://localhost:5004/api/asistencias", payload);
        } else {
          // Si no existe (ID no presente), se debe crear
          return axios.post("http://localhost:5004/api/asistencias", payload);
        }
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
          {alumnos.map((alumno) => (
            <tr key={alumno.id}>
              <td>{alumno.nombreCompleto}</td>
              <td>
                <input type="date" value={selectedDate} disabled />
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