import React, { useEffect, useState } from "react";
import axios from "axios";
import "./CSS/RegisterRequest.css";
import { SUBJ_API } from "../Config/apiConfig";
import AddSubject from "./AddSubject";

const Subjects = () => {
  const [subjects, setSubjects] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [editingSubject, setEditingSubject] = useState(null);

  const fetchSubjects = async () => {
    try {
      const response = await axios.get(`${SUBJ_API}/Subject/obtenerMaterias`);
      setSubjects(response.data);
    } catch (error) {
      console.error("Error al obtener materias:", error);
      setError("Error al obtener las materias");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchSubjects();
  }, []);

  return (
    <div className="register-request-container">
      <h2>Materias registradas</h2>

      {editingSubject && (
        <AddSubject
          subject={editingSubject}
          onSuccess={() => {
            setEditingSubject(null);
            fetchSubjects();
          }}
        />
      )}

      {loading && <p>Cargando materias...</p>}
      {error && <p style={{ color: "red" }}>{error}</p>}

      {!loading && !error && subjects.length > 0 && (
        <table>
          <thead>
            <tr>
              <th>Nombre</th>
              <th>Código</th>
              <th>Tipo</th>
              <th>Grado</th>
              <th>Fecha de creación</th>
              <th>Acciones</th>
            </tr>
          </thead>
          <tbody>
            {subjects.map((subject, index) => (
              <tr key={index}>
                <td>{subject.nombre}</td>
                <td>{subject.codigo}</td>
                <td>{subject.tipo}</td>
                <td>{subject.grado}</td>
                <td>{new Date(subject.fechaCreacion).toLocaleDateString()}</td>
                <td>
                  <button onClick={() => setEditingSubject(subject)}>Editar materia</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}

      {!loading && !error && subjects.length === 0 && (
        <p>No hay materias registradas.</p>
      )}
    </div>
  );
};

export default Subjects;
