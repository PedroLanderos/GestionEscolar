import React from "react";
import "./CSS/RegisterRequest.css";

const sampleRequests = [
  {
    nombreAlumno: "Juan Pérez",
    curpAlumno: "PEPJ800101HDFLRN09",
    grado: 1,
    nombrePadre: "Carlos Pérez",
    telefono: "5512345678",
    correoPadre: "carlos.perez@example.com",
  },
  {
    nombreAlumno: "María García",
    curpAlumno: "GARM950202MDFLRA01",
    grado: 2,
    nombrePadre: "Ana López",
    telefono: "5523456789",
    correoPadre: "ana.lopez@example.com",
  },
];

const RegisterRequest = ({ onRegisterClick }) => {
  return (
    <div className="register-request-container">
      <h2>Solicitudes de Registro</h2>
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
          {sampleRequests.map((req, index) => (
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
    </div>
  );
};

export default RegisterRequest;
