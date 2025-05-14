import React, { useState } from "react";
import "./CSS/Schedule.css";

const initialSubjects = ["Matemáticas", "Física", "Química", "Historia", "Biología"];

const days = ["Lunes", "Martes", "Miércoles", "Jueves", "Viernes"];
const periods = ["Clase 1", "Clase 2", "Clase 3", "Clase 4"];

const Schedule = () => {
  const [subjects, setSubjects] = useState(initialSubjects);
  const [schedule, setSchedule] = useState({});

  const handleDragStart = (e, subject) => {
    e.dataTransfer.setData("text/plain", subject);
  };

  const handleDrop = (e, day, periodIndex) => {
    e.preventDefault();
    const subject = e.dataTransfer.getData("text/plain");
    const key = `${day}-${periodIndex}`;

    setSchedule((prev) => ({
      ...prev,
      [key]: subject,
    }));
  };

  const handleDragOver = (e) => {
    e.preventDefault();
  };

  return (
    <div className="schedule-container">
      <div className="subject-list">
        <h3>Materias</h3>
        {subjects.map((subject, index) => (
          <div
            key={index}
            className="subject-item"
            draggable
            onDragStart={(e) => handleDragStart(e, subject)}
          >
            {subject}
          </div>
        ))}
      </div>

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
            {periods.map((period, periodIndex) => (
              <tr key={periodIndex}>
                <td>{period}</td>
                {days.map((day) => {
                  const key = `${day}-${periodIndex}`;
                  return (
                    <td
                      key={key}
                      onDrop={(e) => handleDrop(e, day, periodIndex)}
                      onDragOver={handleDragOver}
                      className="droppable-cell"
                    >
                      {schedule[key] || ""}
                    </td>
                  );
                })}
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
};

export default Schedule;
