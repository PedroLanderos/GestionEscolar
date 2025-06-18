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
        console.log("📥 materiaProfesor:", materiaProfesor);
        console.log("📥 horarioId:", horarioId);

        let resIds;
        if (!horarioId || horarioId === "null" || horarioId === "Taller") {
          console.log("🔍 Obteniendo alumnos por taller...");
          resIds = await axios.get(`http://localhost:5002/api/Schedule/alumnosPorTaller/${materiaProfesor}`);
        } else {
          console.log("🔍 Obteniendo alumnos por materia y horario...");
          resIds = await axios.get("http://localhost:5002/api/Schedule/alumnosPorMateriaHorario", {
            params: { materiaProfesor, horario: horarioId },
          });
        }

        const alumnoIds = Array.from(new Set(resIds.data));
        console.log("📦 IDs únicos de alumnos:", alumnoIds);

        if (!alumnoIds.length) {
          console.warn("⚠️ No se encontraron IDs de alumnos.");
          setAlumnos([]);
          setLoading(false);
          return;
        }

        const resAlumnos = await axios.post("http://localhost:5000/api/usuario/obtenerusuariosporids", alumnoIds);
        const alumnosData = resAlumnos.data;
        console.log("👥 Alumnos recibidos:", alumnosData);

        if (!alumnosData || alumnosData.length === 0) {
          console.warn("⚠️ No se encontraron datos de alumnos.");
          setAlumnos([]);
          setLoading(false);
          return;
        }

        const resCiclo = await axios.get("http://localhost:5004/api/CicloEscolar/actual");
        const ciclo = resCiclo.data?.id || null;
        if (!ciclo) console.error("❌ No se encontró un ciclo escolar activo.");
        console.log("📅 Ciclo escolar actual:", ciclo);
        setCicloActual(ciclo);

        const resExistentes = await axios.post(
          `http://localhost:5004/api/calificacion/existentes/clase/${materiaProfesor}`,
          alumnoIds
        );
        const califExistentes = resExistentes.data || [];
        console.log("📄 Calificaciones existentes:", califExistentes);

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
        console.error("❌ Error cargando alumnos o calificaciones:", err);
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

        console.log("💾 Enviando payload:", payload);

        return calif.id && calif.id > 0
          ? axios.put(`http://localhost:5004/api/calificacion/${calif.id}`, payload)
          : axios.post("http://localhost:5004/api/calificacion", payload);
      }).filter(Boolean);

      await Promise.all(requests);
      alert("✅ Calificaciones guardadas correctamente.");
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
