import React, { useEffect, useState } from "react";
import axios from "axios";
import "./CSS/RegisterRequest.css";
import { AUTH_API } from "../Config/apiConfig";

const PasswordResetRequests = () => {
  const [requests, setRequests] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const fetchRequests = async () => {
    try {
      const response = await axios.get(`${AUTH_API}/SolicitudContrasena/MostrarSolicitudesSinProcesar`);
      setRequests(response.data);
    } catch (err) {
      console.error("Error al obtener solicitudes:", err);
      setError("No existen datos para esta sección. Por favor, intente más tarde."); // ERR3
    } finally {
      setLoading(false);
    }
  };

  const handleProcess = async (userId) => {
    try {
      await axios.post(`${AUTH_API}/SolicitudContrasena/ProcesarSolicitud/${userId}`);
      fetchRequests();
    } catch (err) {
      console.error(`Error al procesar la solicitud del usuario ${userId}:`, err);
      alert("Error de conexión al servidor. Intenta nuevamente."); // ERR6
    }
  };

  useEffect(() => {
    fetchRequests();
  }, []);

  return (
    <div className="register-request-container">
      <h2>Solicitudes de Restablecimiento de Contraseña</h2>

      {loading && <p>Cargando solicitudes...</p>}
      {error && <p style={{ color: "red" }}>{error}</p>}

      {!loading && !error && (
        <table>
          <thead>
            <tr>
              <th>ID Solicitud</th>
              <th>ID Usuario</th>
              <th>Estado</th>
              <th>Acciones</th>
            </tr>
          </thead>
          <tbody>
            {requests.map((req) => (
              <tr key={req.id}>
                <td>{req.id}</td>
                <td>{req.userId}</td>
                <td>{req.procesada ? "Procesada" : "Pendiente"}</td>
                <td>
                  <div className="action-buttons">
                    <button onClick={() => handleProcess(req.userId)}>Procesar</button>
                  </div>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
};

export default PasswordResetRequests;
