import React, { useEffect, useState } from "react";
import axios from "axios";
import "./CSS/RegisterRequest.css";

const RegisterRequest = ({ onRegisterClick }) => {
  const [requests, setRequests] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const fetchRequests = async () => {
    try {
      const response = await axios.get("http://localhost:5000/api/solicitud/obtenerSolicitudes");
      setRequests(response.data); 
    } catch (err) {
      console.error("❌ Error al obtener solicitudes:", err);
      setError("Error al obtener las solicitudes.");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchRequests();
  }, []);

  return (
    <div className="register-request-container">
      <h2>Solicitudes de Registro</h2>

      {loading && <p>Cargando solicitudes...</p>}
      {error && <p style={{ color: "red" }}>{error}</p>}

      {!loading && !error && (
        <table>
          <thead>
            <tr>
              <th>Alumno</th>
              <th>CURP</th>
              <th>Grado</th>
              <th>Padre/Tutor</th>
              <th>Teléfono</th>
              <th>Correo</th>
              <th>Acciones</th>
            </tr>
          </thead>
          <tbody>
            {requests.map((req, index) => (
              <tr key={index}>
                <td>{req.nombreAlumno}</td>
                <td>{req.curpAlumno}</td>
                <td>{req.grado}</td>
                <td>{req.nombrePadre}</td>
                <td>{req.telefono}</td>
                <td>{req.correoPadre}</td>
                <td>
                  <button onClick={() => onRegisterClick(req)}>Registrar</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
};

export default RegisterRequest;