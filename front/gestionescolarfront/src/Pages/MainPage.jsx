import React from "react";
import "./CSS/MainPage.css";

const MainPage = () => {
  return (
    <>
      <div className="top-nav">
        <ul>
          <li className="nav-item">Inicio</li>
          <li className="nav-item active">Alumnos</li>
          <li className="nav-item">Administrador</li>
        </ul>
      </div>

      <div className="main-layout">
        <aside className="sidebar-left">
          <ul>
            <li>Datos Personales</li>
            <li>Datos Médicos</li>
            <li>Datos Extracurricular</li>
            <li>Pase Digital</li>
            <li>Expediente DAE</li>
            <li>Cambios de Carrera</li>
            <li>Trámites</li>
            <li className="submenu">- Solicitud</li>
            <li className="submenu">- Seguimiento</li>
            <li className="submenu">- Citas</li>
            <li>Datos Académicos</li>
            <li className="submenu">- Kárdex</li>
            <li className="submenu">- Estado General</li>
            <li className="submenu">- Calificaciones</li>
          </ul>
        </aside>

        <main className="main-content">
          <h2>MENÚ PRINCIPAL DE ALUMNOS</h2>
          <h3>BUENOS DÍAS</h3>
          <h3>PEDRO JONAS LANDEROS CORTES</h3>
          <p className="notice">
            <strong>AVISO:</strong> Recuerda que la credencial del IPN te reconoce como alumno y es la forma de identificarte dentro del Instituto.
          </p>
          <div className="alert">
            <h4>ALERTA</h4>
            <p>
              Ningún empleado del Instituto está facultado para solicitar datos personales por redes sociales. <strong>¡DENUNCIA!</strong>
            </p>
          </div>
        </main>

        <aside className="sidebar-right">
          <h4>Accesos rápidos</h4>
          <ul>
            <li><a href="#">Agenda escolar</a></li>
            <li><a href="#">Horarios de clase</a></li>
            <li><a href="#">Horarios de ETS</a></li>
          </ul>
        </aside>
      </div>
    </>
  );
};

export default MainPage;
