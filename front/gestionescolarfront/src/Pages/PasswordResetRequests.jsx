import React, { useEffect, useState } from "react";
import axios from "axios";
import "./CSS/RegisterRequest.css"; // Reutilizando estilos existentes
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
      console.error("❌ Error al obtener solicitudes:", err);
      setError("Error al obtener las solicitudes.");
    } finally {
      setLoading(false);
    }
  };

  const handleProcess = async (userId) => {
    try {
      await axios.post(`${AUTH_API}/SolicitudContrasena/ProcesarSolicitud/${userId}`);
      
      fetchRequests();
    } catch (err) {
      console.error(`❌ Error al procesar la solicitud del usuario ${userId}:`, err);
      alert("Hubo un error al procesar la solicitud.");
    }
  };

  useEffect(() => {
    fetchRequests();
  }, []);

  return (
    <div className="register-request-container">
      <h2>Password Reset Requests</h2>

      {loading && <p>Loading requests...</p>}
      {error && <p style={{ color: "red" }}>{error}</p>}

      {!loading && !error && (
        <table>
          <thead>
            <tr>
              <th>Request ID</th>
              <th>User ID</th>
              <th>Status</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {requests.map((req) => (
              <tr key={req.id}>
                <td>{req.id}</td>
                <td>{req.userId}</td>
                <td>{req.procesada ? "Processed" : "Pending"}</td>
                <td>
                  <div className="action-buttons">
                    <button onClick={() => handleProcess(req.userId)}>Process</button>
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
