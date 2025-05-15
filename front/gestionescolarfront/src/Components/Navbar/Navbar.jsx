import React from "react";
import "./Navbar.css";

const Navbar = () => {
  return (
    <header className="custom-navbar">
      <div className="nav-left">
        <img src="/logoinstitucion.png" alt="Logo IPN" className="logo-left" />
      </div>

      <div className="nav-center">
        <h1 className="nav-title">SISTEMA DE GESTION ESCOLAR</h1>
      </div>

      <div className="nav-right">
        <img src="/logoescuela.png" alt="Logo ESCOM" className="logo-right" />
      </div>
    </header>
  );
};

export default Navbar;
