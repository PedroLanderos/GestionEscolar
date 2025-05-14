import React, { useEffect, useState } from "react";
import axios from "axios";
import "./CSS/Schedule.css";

const days = ["Lunes", "Martes", "Miércoles", "Jueves", "Viernes"];
const periods = [
  { label: "Clase 1", horaInicio: "08:00" },
  { label: "Clase 2", horaInicio: "09:30" },
  { label: "Clase 3", horaInicio: "11:00" },
  { label: "Clase 4", horaInicio: "12:30" },
];

const Schedule = ({ schedule, onBack }) => {
  const horarioId = schedule?.id;

  const [subjects, setSubjects] = useState([]);
  const [asignaciones, setAsignaciones] = useState({});
  const [draggedSubjectId, setDraggedSubjectId] = useState("");

  useEffect(() => {
    const fetchSubjects = async () => {
      try {
        const res = await axios.get("http://localhost:5001/api/subjectassignment/obtenerAsignaciones");
        setSubjects(res.data.map((s) => s.id)); // Mostrar ID completo
      } catch (error) {
        console.error("❌ Error al obtener materias asignadas:", error);
      }
    };

    const fetchSchedule = async () => {
      try {
        const res = await axios.get(`http://localhost:5002/api/Schedule/obtenerHorarioCompleto/${horarioId}`);
        const asignacionesMap = {};
        res.data.forEach((a) => {
          asignacionesMap[`${a.dia}-${a.horaInicio}`] = a;
        });
        setAsignaciones(asignacionesMap);
      } catch (err) {
        console.error("❌ Error al cargar horario:", err);
      }
    };

    fetchSubjects();
    if (horarioId) fetchSchedule();
  }, [horarioId]);

  const handleDragStart = (e, subjectId) => {
    setDraggedSubjectId(subjectId);
  };

  const handleDrop = async (e, dia, horaInicio) => {
    e.preventDefault();
    if (!draggedSubjectId) return;

    const key = `${dia}-${horaInicio}`;
    const existing = asignaciones[key];

    const payload = {
      id: existing?.id || 0,
      idMateria: draggedSubjectId,
      idHorario: horarioId,
      horaInicio,
      dia,
    };

    try {
      if (existing) {
        await axios.put("http://localhost:5002/api/Schedule/actualizarAsignacion", payload);
      } else {
        await axios.post("http://localhost:5002/api/Schedule/asginarMateriaHorario", payload);
      }

      const res = await axios.get(`http://localhost:5002/api/Schedule/obtenerHorarioCompleto/${horarioId}`);
      const updatedMap = {};
      res.data.forEach((a) => {
        updatedMap[`${a.dia}-${a.horaInicio}`] = a;
      });
      setAsignaciones(updatedMap);
    } catch (err) {
      console.error("❌ Error al guardar asignación:", err);
    }
  };

  const handleDragOver = (e) => {
    e.preventDefault();
  };

  return (
  <div className="schedule-container">
    <div className="schedule-content">
      <h2 style={{ textAlign: "center", color: "#6e1530" }}>
        Horario de {schedule?.grado}° {schedule?.grupo}
      </h2>

      <button onClick={onBack} className="back-button">
        ⬅ Volver a la lista de horarios
      </button>

      <div className="schedule-table">
        <table>
          <thead>
            <tr>
              <th>Horario</th>
              {days.map((day) => (
                <th key={day}>{day}</th>
              ))}
            </tr>
          </thead>
          <tbody>
            {periods.map(({ label, horaInicio }, periodIndex) => (
              <tr key={periodIndex}>
                <td>{label}</td>
                {days.map((day) => {
                  const key = `${day}-${horaInicio}`;
                  const asignacion = asignaciones[key];

                  return (
                    <td
                      key={key}
                      onDrop={(e) => handleDrop(e, day, horaInicio)}
                      onDragOver={handleDragOver}
                      className="droppable-cell"
                    >
                      {asignacion?.idMateria || ""}
                    </td>
                  );
                })}
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>

    <div className="subject-list">
      <h3>Materias</h3>
      {subjects.map((subjectId, index) => (
        <div
          key={index}
          className="subject-item"
          draggable
          onDragStart={(e) => handleDragStart(e, subjectId)}
        >
          {subjectId}
        </div>
      ))}
    </div>
  </div>
);

};

export default Schedule;
