import React, { useContext } from "react";
import "./Footer.css";
import { AuthContext } from "../../Context/AuthContext";
import { useNavigate } from "react-router-dom";

const Footer = () => {
  const { auth, logout } = useContext(AuthContext);
  const navigate = useNavigate();

  const handleLogout = () => {
    if (window.confirm("¿Deseas cerrar sesión?")) {
      logout();
      navigate("/login");
    }
  };

  return (
    <footer className="custom-footer">
      <div className="footer-left">
        {auth.isAuthenticated && (
          <button onClick={handleLogout} className="logout-button">
            🚪 Cerrar sesión
          </button>
        )}
      </div>

      <div className="footer-center">
        <p>© {new Date().getFullYear()} Escuela Superior de Cómputo - IPN</p>
      </div>

      <div className="footer-right">
        <p>Sitio desarrollado como parte de proyecto escolar</p>
      </div>
    </footer>
  );
};

export default Footer;
