import React, { useState, useEffect } from "react";
import axios from "axios";
import AddSanction from "./AddSanction";
import "./CSS/UsersTable.css"; // Reutilizamos el mismo CSS

const ShowSanctions = () => {
  const [sanctions, setSanctions] = useState([]);
  const [loading, setLoading] = useState(true);
  const [selectedSanction, setSelectedSanction] = useState(null);
  const [isEditing, setIsEditing] = useState(false);

  // Obtener las sanciones
  useEffect(() => {
    const fetchSanctions = async () => {
      setLoading(true);
      try {
        const res = await axios.get("http://localhost:5004/api/sancion");
        setSanctions(res.data);
      } catch (error) {
        console.error("❌ Error al obtener sanciones:", error);
      } finally {
        setLoading(false);
      }
    };

    fetchSanctions();
  }, []);

  // Función para eliminar una sanción
  const handleDelete = async (id) => {
    const confirmDelete = window.confirm("¿Estás seguro de que deseas eliminar esta sanción?");
    if (confirmDelete) {
      try {
        // Enviamos el cuerpo necesario para eliminar
        const sanctionToDelete = sanctions.find((s) => s.id === id);
        await axios.delete("http://localhost:5004/api/sancion", {
          data: sanctionToDelete,
        });

        // Eliminar de la lista local
        setSanctions(sanctions.filter((s) => s.id !== id));
      } catch (error) {
        console.error("❌ Error al eliminar la sanción", error);
      }
    }
  };

  // Función para iniciar el proceso de edición
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
          initialData={selectedSanction} // Pasamos los datos a editar
        />
      ) : (
        <>
          {loading ? (
            <p>Cargando sanciones...</p>
          ) : (
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
                {sanctions.length > 0 ? (
                  sanctions.map((sanction) => (
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
                  ))
                ) : (
                  <tr>
                    <td colSpan="6">No hay sanciones registradas.</td>
                  </tr>
                )}
              </tbody>
            </table>
          )}
        </>
      )}
    </div>
  );
};

export default ShowSanctions;
