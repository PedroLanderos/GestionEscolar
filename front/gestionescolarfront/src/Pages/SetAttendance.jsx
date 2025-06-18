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
        console.log("📥 ClaseProfesor:", claseProfesor);
        console.log("📥 HorarioId:", horarioId);

        let resIds;
        if (!horarioId || horarioId === "null" || horarioId === "Taller") {
          console.log("🔍 Obteniendo alumnos por taller...");
          resIds = await axios.get(`http://localhost:5002/api/Schedule/alumnosPorTaller/${claseProfesor}`);
        } else {
          console.log("🔍 Obteniendo alumnos por materia y horario...");
          resIds = await axios.get(`http://localhost:5002/api/Schedule/alumnosPorMateriaHorario`, {
            params: { materiaProfesor: claseProfesor, horario: horarioId },
          });
        }

        console.log("📦 IDs obtenidos:", resIds.data);

        const alumnoIds = Array.from(new Set(resIds.data));
        console.log("📋 IDs únicos:", alumnoIds);

        if (!alumnoIds || alumnoIds.length === 0) {
          console.warn("⚠️ No se encontraron IDs de alumnos.");
          setAlumnos([]);
          setLoading(false);
          return;
        }

        console.log("🔄 Obteniendo datos de alumnos...");
        const resAlumnos = await axios.post("http://localhost:5000/api/usuario/obtenerusuariosporids", alumnoIds);
        const alumnosData = resAlumnos.data;
        console.log("👥 Datos de alumnos recibidos:", alumnosData);

        if (!alumnosData || alumnosData.length === 0) {
          console.warn("⚠️ No se encontraron datos de alumnos.");
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

        console.log("📅 Buscando asistencias previas...");
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
          console.log("✅ Asistencias anteriores cargadas:", asistenciasMap);
        } else {
          console.log("ℹ️ No hay asistencias anteriores registradas.");
        }
      } catch (err) {
        console.error("❌ Error cargando datos de alumnos o asistencias:", err);
        setError("Error cargando datos, intenta más tarde.");
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
      alert("✅ Asistencias guardadas correctamente.");
    } catch (err) {
      console.error("❌ Error guardando asistencias:", err);
      alert("Error al guardar asistencias. Intenta nuevamente.");
    }
  };

  if (loading) return <p>Cargando alumnos...</p>;
  if (error) return <p style={{ color: "red" }}>{error}</p>;
  if (alumnos.length === 0) return <p>No hay alumnos inscritos para esta clase y horario.</p>;

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
