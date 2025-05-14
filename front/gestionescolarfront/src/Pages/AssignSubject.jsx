import React, { useEffect, useState } from "react";
import axios from "axios";
import "./CSS/AssignSubject.css";

const AssignSubject = () => {
  const [teachers, setTeachers] = useState([]);
  const [subjects, setSubjects] = useState([]);
  const [selectedTeacher, setSelectedTeacher] = useState(null);
  const [selectedSubject, setSelectedSubject] = useState(null);
  const [message, setMessage] = useState("");
  const [error, setError] = useState("");

  useEffect(() => {
    const fetchTeachers = async () => {
      try {
        const res = await axios.get("http://localhost:5000/api/usuario/obtenerDocentes");
        setTeachers(res.data);
      } catch (err) {
        console.error("Error al cargar docentes", err);
        setError("Error al cargar docentes.");
      }
    };

    const fetchSubjects = async () => {
      try {
        const res = await axios.get("http://localhost:5001/api/Subject/obtenerMaterias");
        setSubjects(res.data);
      } catch (err) {
        console.error("Error al cargar materias", err);
        setError("Error al cargar materias.");
      }
    };

    fetchTeachers();
    fetchSubjects();
  }, []);

  const handleAssign = async () => {
    if (!selectedTeacher || !selectedSubject) {
      setError("Selecciona un docente y una materia.");
      setMessage("");
      return;
    }

    try {
      await axios.post("http://localhost:5001/api/subjectassignment/crearAsignacion", {
        id: "1", // Este valor se reescribirá por el backend, pero debe enviarse
        subjectId: selectedSubject.codigo,
        userId: selectedTeacher.id
      });

      setMessage("Asignación realizada exitosamente.");
      setError("");
      setSelectedTeacher(null);
      setSelectedSubject(null);
    } catch (err) {
      console.error("Error al asignar materia", err);
      setError("Error al asignar la materia.");
      setMessage("");
    }
  };

  return (
    <div className="assign-container">
      <h2>Asignar Materia a Docente</h2>

      <div className="lists-wrapper">
        <div className="list-box">
          <h3>Docentes</h3>
          <ul>
            {teachers.map((teacher) => (
              <li
                key={teacher.id}
                className={selectedTeacher?.id === teacher.id ? "selected" : ""}
                onClick={() => setSelectedTeacher(teacher)}
              >
                {teacher.nombreCompleto}
              </li>
            ))}
          </ul>
        </div>

        <div className="list-box">
          <h3>Materias</h3>
          <ul>
            {subjects.map((subject) => (
              <li
                key={subject.codigo}
                className={selectedSubject?.codigo === subject.codigo ? "selected" : ""}
                onClick={() => setSelectedSubject(subject)}
              >
                {subject.nombre}
              </li>
            ))}
          </ul>
        </div>
      </div>

      <button onClick={handleAssign}>Asignar Materia</button>

      {message && <p className="success-message">{message}</p>}
      {error && <p className="error">{error}</p>}
    </div>
  );
};

export default AssignSubject;
