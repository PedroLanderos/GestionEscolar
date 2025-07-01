import React, { useState, useEffect } from "react";
import axios from "axios";
import AddSanction from "./AddSanction";
import "./CSS/UsersTable.css";

const ShowSanctions = () => {
  const [sanctions, setSanctions] = useState([]);
  const [loading, setLoading] = useState(true);
  const [selectedSanction, setSelectedSanction] = useState(null);
  const [isEditing, setIsEditing] = useState(false);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchSanctions = async () => {
      setLoading(true);
      setError(null);
      try {
        const res = await axios.get("http://localhost:5004/api/sancion");
        setSanctions(res.data);
      } catch (error) {
        console.error("Error al obtener sanciones:", error);
        setError("No existen datos para esta sección. Por favor, intente más tarde."); // ERR3
      } finally {
        setLoading(false);
      }
    };

    fetchSanctions();
  }, []);

  const handleDelete = async (id) => {
    const confirmDelete = window.confirm("¿Está seguro que desea eliminar esta sanción?");
    if (confirmDelete) {
      try {
        const sanctionToDelete = sanctions.find((s) => s.id === id);
        await axios.delete("http://localhost:5004/api/sancion", {
          data: sanctionToDelete,
        });
        setSanctions(sanctions.filter((s) => s.id !== id));
      } catch (error) {
        console.error("Error al eliminar la sanción", error);
        setError("Error de conexión al servidor. Intenta nuevamente."); // ERR6
      }
    }
  };

  const handleEdit = (sanction) => {
    setSelectedSanction(sanction);
    setIsEditing(true);
  };

  return (
    <div className="users-table-container">
      <h2>Sanciones Registradas</h2>

      {isEditing ? (
        <AddSanction
          onBack={() => setIsEditing(false)}
          initialData={selectedSanction}
        />
      ) : loading ? (
        <p>Cargando sanciones...</p>
      ) : error ? (
        <p style={{ color: "red" }}>{error}</p>
      ) : sanctions.length > 0 ? (
        <table>
          <thead>
            <tr>
              <th>Tipo de Sanción</th>
              <th>Descripción</th>
              <th>Fecha</th>
              <th>Alumno</th>
              <th>Profesor</th>
              <th>Acciones</th>
            </tr>
          </thead>
          <tbody>
            {sanctions.map((sanction) => (
              <tr key={sanction.id}>
                <td>{sanction.tipoSancion}</td>
                <td>{sanction.descripcion}</td>
                <td>{new Date(sanction.fecha).toLocaleDateString()}</td>
                <td>{sanction.idAlumno}</td>
                <td>{sanction.idProfesor}</td>
                <td>
                  <button onClick={() => handleEdit(sanction)}>Editar</button>
                  <button onClick={() => handleDelete(sanction.id)}>Eliminar</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      ) : (
        <p>No existen datos para esta sección. Por favor, intente más tarde.</p> // ERR3
      )}
    </div>
  );
};

export default ShowSanctions;
