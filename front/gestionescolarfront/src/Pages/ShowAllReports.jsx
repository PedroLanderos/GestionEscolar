// src/components/ShowAllReports.jsx
import React, { useState, useEffect } from "react";
import axios from "axios";
import CreateReport from "./CreateReport";
import "./CSS/UsersTable.css";

const ShowAllReports = () => {
  const [reports, setReports] = useState([]);
  const [loading, setLoading] = useState(true);
  const [editingReport, setEditingReport] = useState(null);

  const fetchReports = async () => {
    setLoading(true);
    try {
      const res = await axios.get("http://localhost:5004/api/reporte");
      setReports(res.data);
    } catch (error) {
      console.error("❌ Error al obtener reportes:", error);
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (id) => {
    if (window.confirm("¿Estás seguro de eliminar este reporte?")) {
      try {
        await axios.delete(`http://localhost:5004/api/reporte/${id}`);
        setReports((prev) => prev.filter((r) => r.id !== id));
      } catch (err) {
        console.error("❌ Error al eliminar el reporte:", err);
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
                <td colSpan="6">No hay reportes registrados.</td>
              </tr>
            )}
          </tbody>
        </table>
      )}
    </div>
  );
};

export default ShowAllReports;
