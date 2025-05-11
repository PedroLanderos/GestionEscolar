import React from "react";
import "./CSS/Register.css";

const Register = ({ data }) => {
  const nombre = data?.nombreAlumno || "";
  const curp = data?.curpAlumno || "";
  const correo = data?.correoPadre || "";

  return (
    <div className="register-container">
      <div className="register-content">
        <h2>Registro de Alumno</h2>
        <form>
          <label>Nombre del Alumno</label>
          <input type="text" value={nombre} disabled />

          <label>CURP</label>
          <input type="text" value={curp} disabled />

          <label>Correo del Padre/Tutor</label>
          <input type="email" value={correo} disabled />

          <button type="submit" disabled>
            Registrar (aún no implementado)
          </button>
        </form>
      </div>
    </div>
  );
};

export default Register;
