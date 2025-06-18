import React, { useContext, useEffect, useState } from "react";
import axios from "axios";
import "./CSS/Schedule.css";
import { AuthContext } from "../Context/AuthContext";

const days = ["Lunes", "Martes", "Miércoles", "Jueves", "Viernes"];
const periods = [
  { horaInicio: "08:00", horaFin: "09:30" },
  { horaInicio: "09:30", horaFin: "11:00" },
  { horaInicio: "11:00", horaFin: "12:30" },
  { horaInicio: "12:30", horaFin: "14:00" },
];

const ShowSchedule = () => {
  const { auth } = useContext(AuthContext);
  const [asignaciones, setAsignaciones] = useState({});
  const [tooltip, setTooltip] = useState({ visible: false, content: "", x: 0, y: 0 });

  useEffect(() => {
    const fetchSchedule = async () => {
      if (!auth?.user?.id || !auth?.user?.role) return;

      const role = auth.user.role.toLowerCase();
      let userId = auth.user.id;

      // Si es tutor, obtener id de su alumno (hijo)
      if (role === "tutor" || role === "padre") {
        try {
          const res = await axios.get(`http://localhost:5000/api/usuario/obtenerAlumnoPorTutor/${userId}`);
          if (res.data && res.data.id) {
            userId = res.data.id;
          } else {
            console.warn("No se encontró alumno asociado al tutor");
            return;
          }
        } catch (error) {
          console.error("Error al obtener alumno para tutor:", error);
          return;
        }
      }

      const isDocente = role === "docente";
      const endpoint = isDocente
        ? `http://localhost:5002/api/Schedule/horarioDocente/${userId}`
        : `http://localhost:5002/api/Schedule/horarioAlumno/${userId}`;

      try {
        const res = await axios.get(endpoint);
        const asignacionesMap = {};

        res.data.forEach((a) => {
          let horaKey = a.horaInicio;

          // Ajustar hora para talleres en formato "8:00-9:30" → "08:00"
          if (horaKey.includes("-")) {
            horaKey = horaKey.split("-")[0].padStart(5, "0");
          }

          const key = `${a.dia}-${horaKey}`;
          asignacionesMap[key] = a;
        });

        setAsignaciones(asignacionesMap);
      } catch (err) {
        console.error("❌ Error al obtener el horario:", err);
      }
    };

    fetchSchedule();
  }, [auth]);

  const showTooltip = async (subjectId, event) => {
    if (!subjectId.includes("-")) {
      setTooltip({
        visible: true,
        content: `Taller: ${subjectId}`,
        x: event.clientX + 10,
        y: event.clientY + 10,
      });
      return;
    }

    const [subjectCode, userId] = subjectId.split("-");

    try {
      const [subjectRes, userRes] = await Promise.all([
        axios.get(`http://localhost:5001/api/Subject/obtenerPorCodigo/${subjectCode}`),
        axios.get(`http://localhost:5000/api/usuario/obtenerUsuarioPorId/${userId}`),
      ]);

      const nombreMateria = subjectRes.data?.nombre || "Materia desconocida";
      const nombreDocente = userRes.data?.nombreCompleto || "Docente desconocido";

      setTooltip({
        visible: true,
        content: `Materia: ${nombreMateria}\nDocente: ${nombreDocente}`,
        x: event.clientX + 10,
        y: event.clientY + 10,
      });
    } catch (error) {
      console.error("❌ Error al cargar datos del tooltip:", error);
    }
  };

  const hideTooltip = () => {
    setTooltip({ visible: false, content: "", x: 0, y: 0 });
  };

  return (
    <div className="schedule-container">
      <div className="schedule-content">
        <h2 style={{ textAlign: "center", color: "#6e1530" }}>
          Mi Horario ({auth.user?.role})
        </h2>

        <div className="schedule-table">
          <table>
            <thead>
              <tr>
                <th>Hora</th>
                {days.map((day) => (
                  <th key={day}>{day}</th>
                ))}
              </tr>
            </thead>
            <tbody>
              {periods.map(({ horaInicio, horaFin }, index) => (
                <tr key={index}>
                  <td>{horaInicio} - {horaFin}</td>
                  {days.map((day) => {
                    const key = `${day}-${horaInicio}`;
                    const asignacion = asignaciones[key];
                    const isTaller = asignacion?.idHorario === null;

                    return (
                      <td
                        key={key}
                        className={`droppable-cell ${isTaller ? "taller-cell" : ""}`}
                        onMouseEnter={(e) => asignacion?.idMateria && showTooltip(asignacion.idMateria, e)}
                        onMouseMove={(e) => tooltip.visible && setTooltip(prev => ({ ...prev, x: e.clientX + 10, y: e.clientY + 10 }))}
                        onMouseLeave={hideTooltip}
                      >
                        {asignacion?.idMateria
                          ? isTaller
                            ? `Taller: ${asignacion.idMateria}`
                            : asignacion.idMateria
                          : ""}
                      </td>
                    );
                  })}
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>

      {tooltip.visible && (
        <div className="tooltip" style={{ top: tooltip.y, left: tooltip.x }}>
          {tooltip.content}
        </div>
      )}
    </div>
  );
};

export default ShowSchedule;
