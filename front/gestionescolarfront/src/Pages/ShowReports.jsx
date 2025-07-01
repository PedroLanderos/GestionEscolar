import React, { useEffect, useState, useContext } from "react";
import axios from "axios";
import { AuthContext } from "../Context/AuthContext";
import "./CSS/UsersTable.css";

const API_BASE = "http://localhost:5004/api";

const ShowReport = ({ modo, onBack }) => {
  const { auth } = useContext(AuthContext);
  const userId = auth.user?.id;
  const userRole = auth.user?.role?.toLowerCase();

  const [reportes, setReportes] = useState([]);
  const [loading, setLoading] = useState(false);
  const [mensaje, setMensaje] = useState("");

  const obtenerLunesSemana = () => {
    const hoy = new Date();
    const lunes = new Date(hoy);
    lunes.setDate(hoy.getDate() - ((hoy.getDay() + 6) % 7));
    return lunes.toISOString().split("T")[0];
  };

  const obtenerHoy = () => {
    return new Date().toISOString().split("T")[0];
  };

  const cargarReportes = async () => {
    setLoading(true);
    setMensaje("");

    try {
      let alumnoId = userId;

      if (userRole === "tutor" || userRole === "padre") {
        const resAlumno = await axios.get(`http://localhost:5000/api/usuario/obtenerAlumnoPorTutor/${userId}`);
        if (resAlumno.data?.id) {
          alumnoId = resAlumno.data.id;
        } else {
          setMensaje("No hay suficientes datos registrados para usar esta opción. Por favor, registre más datos e intente nuevamente."); // ERR2
          setLoading(false);
          return;
        }
      }

      if (modo === "ciclo") {
        const ciclo = await axios.get(`${API_BASE}/CicloEscolar/actual`);
        const cicloId = ciclo.data.id;

        const response = await axios.get(`${API_BASE}/reporte/ciclo/${cicloId}/alumno/${alumnoId}`);
        setReportes(response.data);
      } else if (modo === "semana") {
        const inicio = obtenerLunesSemana();
        const fin = obtenerHoy();

        const response = await axios.get(`${API_BASE}/reporte/alumno/${alumnoId}/fechas?inicio=${inicio}&fin=${fin}`);
        setReportes(response.data);
      }
    } catch (error) {
      console.error("Error cargando reportes:", error);
      setMensaje("No existen datos para esta sección. Por favor, intente más tarde."); // ERR3
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    cargarReportes();
  }, [modo]);

  return (
    <div className="users-table-container">
      <button onClick={onBack} style={{ marginBottom: "1rem" }}>← Regresar</button>
      <h2>Reportes {modo === "ciclo" ? "del Ciclo Escolar" : "Semanales"}</h2>

      {loading ? (
        <p>Cargando reportes...</p>
      ) : mensaje ? (
        <p style={{ color: mensaje.includes("No existen") ? "red" : "black" }}>{mensaje}</p>
      ) : reportes.length > 0 ? (
        <table>
          <thead>
            <tr>
              <th>Fecha</th>
              <th>Tipo</th>
              <th>Grupo</th>
              <th>Ciclo Escolar</th>
            </tr>
          </thead>
          <tbody>
            {reportes.map((r) => (
              <tr key={r.id}>
                <td>{new Date(r.fecha).toLocaleDateString()}</td>
                <td>{r.tipo}</td>
                <td>{r.grupo || "No existen datos para esta sección. Por favor, intente más tarde." /* ERR3 */}</td>
                <td>{r.cicloEscolar}</td>
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

export default ShowReport;
