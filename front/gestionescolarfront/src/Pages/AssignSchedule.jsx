import React, { useEffect, useState } from "react";
import axios from "axios";
import "./CSS/AssignSubject.css";

const AssignSchedule = ({ schedule, onClose }) => {
  const [students, setStudents] = useState([]);
  const [selectedStudents, setSelectedStudents] = useState([]);
  const [message, setMessage] = useState("");
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    const fetchStudents = async () => {
      try {
        const res = await axios.get(`http://localhost:5000/api/usuario/filtrarPorGrado/${schedule.grado}`);
        setStudents(res.data);
      } catch (err) {
        console.error("Error al cargar alumnos", err);
        setError("No existen datos para esta sección. Por favor, intente más tarde."); // ERR3
      }
    };

    if (schedule) {
      fetchStudents();
    }
  }, [schedule]);

  const toggleStudentSelection = (studentId) => {
    setSelectedStudents((prev) =>
      prev.includes(studentId)
        ? prev.filter((id) => id !== studentId)
        : [...prev, studentId]
    );
  };

  const handleAssign = async () => {
    if (selectedStudents.length === 0) {
      setError("Los datos ingresados no son válidos"); // ERR1
      setMessage("");
      return;
    }

    setLoading(true);
    setMessage("");
    setError("");

    try {
      const requests = selectedStudents.map((idUser) =>
        axios.post("http://localhost:5002/api/Schedule/asignarAlumnoHorario", {
          id: 1,
          idUser,
          idSchedule: schedule.id,
        })
      );

      await Promise.all(requests);

      setMessage("Elemento registrado exitosamente."); // MSG3
      setSelectedStudents([]);
    } catch (err) {
      console.error("Error al asignar horario", err);
      setError("Error de conexión al servidor. Intenta nuevamente."); // ERR6
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="assign-container">
      <h2>Asignar Horario a Alumnos - Grado {schedule.grado}, Grupo {schedule.grupo}</h2>

      <div className="lists-wrapper">
        <div className="list-box">
          <h3>Alumnos</h3>
          <ul>
            {students.map((student) => (
              <li
                key={student.id}
                className={selectedStudents.includes(student.id) ? "selected" : ""}
                onClick={() => toggleStudentSelection(student.id)}
              >
                {student.id} - {student.nombreCompleto}
              </li>
            ))}
          </ul>
        </div>
      </div>

      <button onClick={handleAssign} disabled={loading || students.length === 0}>
        {loading ? "Asignando..." : "Asignar Horario"}
      </button>
      <button onClick={onClose} style={{ marginTop: "10px", backgroundColor: "#999" }}>
        Cancelar
      </button>

      {message && <p className="success-message">{message}</p>}
      {error && <p className="error">{error}</p>}
    </div>
  );
};

export default AssignSchedule;
