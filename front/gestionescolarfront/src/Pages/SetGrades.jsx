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
    if (!materiaProfesor || !horarioId) {
      console.warn("⚠️ No se recibió materiaProfesor u horarioId");
      return;
    }

    const fetchData = async () => {
      setLoading(true);
      setError(null);

      console.log("🔄 Iniciando carga de datos para calificaciones...");
      console.log("📘 MateriaProfesor:", materiaProfesor);
      console.log("📘 HorarioId:", horarioId);

      try {
        // 1. Obtener IDs de alumnos
        const resIds = await axios.get("http://localhost:5002/api/Schedule/alumnosPorMateriaHorario", {
          params: { materiaProfesor, horario: horarioId },
        });
        const alumnoIds = resIds.data;
        console.log("🧑‍🎓 IDs de alumnos obtenidos:", alumnoIds);

        if (!alumnoIds.length) {
          console.warn("⚠️ No hay alumnos para esta materia y horario.");
          setAlumnos([]);
          setLoading(false);
          return;
        }

        // 2. Obtener datos de alumnos
        const resAlumnos = await axios.post("http://localhost:5000/api/usuario/obtenerusuariosporids", alumnoIds);
        const alumnosData = resAlumnos.data;
        console.log("📄 Datos de alumnos:", alumnosData);

        // 3. Obtener ciclo escolar actual
        const resCiclo = await axios.get("http://localhost:5004/api/CicloEscolar/actual");
        console.log("📘 Respuesta de ciclo actual:", resCiclo.data);
        const ciclo = resCiclo.data?.id || null;

        if (!ciclo) {
          console.error("❌ No se encontró un ciclo escolar activo.");
        }

        setCicloActual(ciclo);

        // Inicializar estado de calificaciones
        const initialCalificaciones = {};
        alumnosData.forEach(a => {
          initialCalificaciones[a.id] = { calificacionFinal: "", comentarios: "" };
        });

        setAlumnos(alumnosData);
        setCalificaciones(initialCalificaciones);
      } catch (err) {
        console.error("❌ Error cargando alumnos o ciclo escolar:", err);
        setError("Error cargando datos, intenta más tarde.");
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
      alert("No se pudo determinar el ciclo escolar actual.");
      console.warn("❌ No se guardó nada: cicloActual es null");
      return;
    }

    console.log("💾 Iniciando guardado de calificaciones...");
    console.log("🧾 Ciclo escolar actual:", cicloActual);

    try {
      for (const a of alumnos) {
        const calif = calificaciones[a.id];
        const payload = {
          id: 0,
          idMateria: materiaProfesor,
          idAlumno: a.id,
          calificacionFinal: calif.calificacionFinal === "" ? null : calif.calificacionFinal,
          comentarios: calif.comentarios,
          idCiclo: cicloActual,
        };

        console.log(`📤 Enviando calificación para ${a.nombreCompleto} (${a.id}):`, payload);

        await axios.post("http://localhost:5004/api/calificacion", payload);
      }

      alert("Calificaciones guardadas correctamente.");
      if (onBack) onBack();
    } catch (err) {
      console.error("❌ Error guardando calificaciones:", err);
      alert("Error al guardar calificaciones. Intenta nuevamente.");
    }
  };

  if (loading) return <p>Cargando alumnos y ciclo escolar...</p>;
  if (error) return <p style={{ color: "red" }}>{error}</p>;
  if (alumnos.length === 0) return <p>No hay alumnos inscritos para esta clase y horario.</p>;

  return (
    <div className="users-table-container">
      <h2>Registrar Calificaciones - {materiaProfesor} - Horario {horarioId}</h2>
      <table>
        <thead>
          <tr>
            <th>Nombre del Alumno</th>
            <th>Calificación (0-10)</th>
            <th>Comentarios</th>
          </tr>
        </thead>
        <tbody>
          {alumnos.map((alumno) => (
            <tr key={alumno.id}>
              <td>{alumno.nombreCompleto}</td>
              <td>
                <input
                  type="number"
                  min="0"
                  max="10"
                  step="0.1"
                  value={calificaciones[alumno.id]?.calificacionFinal || ""}
                  onChange={(e) => handleCalificacionChange(alumno.id, e.target.value)}
                  placeholder="Ej: 8.5"
                />
              </td>
              <td>
                <input
                  type="text"
                  value={calificaciones[alumno.id]?.comentarios || ""}
                  onChange={(e) => handleComentariosChange(alumno.id, e.target.value)}
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
