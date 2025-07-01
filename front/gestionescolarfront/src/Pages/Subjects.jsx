import React, { useEffect, useState } from "react";
import axios from "axios";
import "./CSS/RegisterRequest.css";
import { SUBJ_API } from "../Config/apiConfig";

const Subjects = ({ onEdit }) => {
  const [subjects, setSubjects] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const fetchSubjects = async () => {
    setLoading(true);
    setError(null);
    try {
      const response = await axios.get(`${SUBJ_API}/Subject/obtenerMaterias`);
      setSubjects(response.data);
    } catch (error) {
      console.error("Error al obtener materias:", error);
      setError("No existen datos para esta sección. Por favor, intente más tarde."); // ERR3
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
                  <button onClick={() => onEdit(subject)}>Editar materia</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}

      {!loading && !error && subjects.length === 0 && (
        <p>No existen datos para esta sección. Por favor, intente más tarde.</p> // ERR3
      )}
    </div>
  );
};

export default Subjects;
