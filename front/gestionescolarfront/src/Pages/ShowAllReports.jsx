import React, { useState, useEffect } from "react";
import axios from "axios";
import CreateReport from "./CreateReport";
import "./CSS/UsersTable.css";

const ShowAllReports = () => {
  const [reports, setReports] = useState([]);
  const [loading, setLoading] = useState(true);
  const [editingReport, setEditingReport] = useState(null);
  const [error, setError] = useState(null);

  const fetchReports = async () => {
    setLoading(true);
    setError(null);
    try {
      const res = await axios.get("http://localhost:5004/api/reporte");
      setReports(res.data);
    } catch (error) {
      console.error("Error al obtener reportes:", error);
      setError("No existen datos para esta sección. Por favor, intente más tarde."); // ERR3
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (id) => {
    if (window.confirm("¿Está seguro que desea eliminar este reporte?")) {
      try {
        await axios.delete(`http://localhost:5004/api/reporte/${id}`);
        setReports((prev) => prev.filter((r) => r.id !== id));
      } catch (err) {
        console.error("Error al eliminar el reporte:", err);
        alert("Error de conexión al servidor. Intenta nuevamente."); // ERR6
      }
    }
  };

  useEffect(() => {
    fetchReports();
  }, []);

  return (
    <div className="users-table-container">
      <h2>Reportes Registrados</h2>

      {editingReport ? (
        <CreateReport
          onBack={() => {
            setEditingReport(null);
            fetchReports(); // Refrescar después de editar
          }}
          initialData={editingReport}
        />
      ) : loading ? (
        <p>Cargando reportes...</p>
      ) : error ? (
        <p style={{ color: "red" }}>{error}</p>
      ) : (
        <table>
          <thead>
            <tr>
              <th>Fecha</th>
              <th>ID Alumno</th>
              <th>Grupo</th>
              <th>Ciclo Escolar</th>
              <th>Tipo</th>
              <th>Acciones</th>
            </tr>
          </thead>
          <tbody>
            {reports.length > 0 ? (
              reports.map((r) => (
                <tr key={r.id}>
                  <td>{new Date(r.fecha).toLocaleDateString()}</td>
                  <td>{r.idAlumno}</td>
                  <td>{r.grupo}</td>
                  <td>{r.cicloEscolar}</td>
                  <td>{r.tipo}</td>
                  <td>
                    <button onClick={() => setEditingReport(r)}>Editar</button>
                    <button onClick={() => handleDelete(r.id)}>Eliminar</button>
                  </td>
                </tr>
              ))
            ) : (
              <tr>
                <td colSpan="6">No existen datos para esta sección. Por favor, intente más tarde.</td> {/* ERR3 */}
              </tr>
            )}
          </tbody>
        </table>
      )}
    </div>
  );
};

export default ShowAllReports;
