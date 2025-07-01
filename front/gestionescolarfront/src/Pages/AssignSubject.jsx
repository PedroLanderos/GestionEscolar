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
        setError("No existen datos para esta sección. Por favor, intente más tarde."); // ERR3
      }
    };

    const fetchSubjects = async () => {
      try {
        const res = await axios.get("http://localhost:5001/api/Subject/obtenerMaterias");
        setSubjects(res.data);
      } catch (err) {
        console.error("Error al cargar materias", err);
        setError("No existen datos para esta sección. Por favor, intente más tarde."); // ERR3
      }
    };

    fetchTeachers();
    fetchSubjects();
  }, []);

  const handleAssign = async () => {
    if (!selectedTeacher || !selectedSubject) {
      setError("Los datos ingresados no son válidos"); // ERR1
      setMessage("");
      return;
    }

    try {
      await axios.post("http://localhost:5001/api/subjectassignment/crearAsignacion", {
        id: "1",
        subjectId: selectedSubject.codigo,
        userId: selectedTeacher.id,
      });

      setMessage("Elemento registrado exitosamente."); // MSG3
      setError("");
      setSelectedTeacher(null);
      setSelectedSubject(null);
    } catch (err) {
      console.error("Error al asignar materia", err);
      setError("Error de conexión al servidor. Intenta nuevamente."); // ERR6
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
