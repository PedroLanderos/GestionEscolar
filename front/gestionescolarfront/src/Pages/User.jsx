import React, { useEffect, useState } from "react";
import axios from "axios";
import { AUTH_API } from "../Config/apiConfig";
import "./CSS/User.css";

const User = ({ id, mode, onBack }) => {
  const isEdit = mode === "edit";
  const isView = mode === "view";

  const [usuario, setUsuario] = useState({
    id: "",
    nombreCompleto: "",
    correo: "",
    contrasena: "",
    curp: "",
    cuentaBloqueada: false,
    dadoDeBaja: false,
    ultimaSesion: new Date().toISOString().split("T")[0],
    rol: "",
  });

  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");

  useEffect(() => {
    const fetchUsuario = async () => {
      try {
        const res = await axios.get(`${AUTH_API}/usuario/obtenerUsuarioPorId/${id}`);
        const data = res.data;
        setUsuario({
          id: data.id,
          nombreCompleto: data.nombreCompleto,
          correo: data.correo,
          contrasena: "",
          curp: data.curp,
          cuentaBloqueada: data.cuentaBloqueada,
          dadoDeBaja: data.dadoDeBaja,
          ultimaSesion: new Date(data.ultimaSesion).toISOString().split("T")[0],
          rol: data.rol,
        });
      } catch (error) {
        console.error("Los datos ingresados no son válidos");
        setError("Los datos ingresados no son válidos");
      }
    };
    fetchUsuario();
  }, [id]);

  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    setUsuario({
      ...usuario,
      [name]: type === "checkbox" ? checked : value,
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");
    setSuccess("");
    try {
      const payload = {
        ...usuario,
        ultimaSesion: new Date(usuario.ultimaSesion).toISOString(),
      };
      await axios.put(`${AUTH_API}/usuario/editarUsuario`, payload);
      setSuccess("Elemento actualizado exitosamente.");
      onBack();
    } catch (error) {
      console.error("Estructura de datos incorrecta. Por favor, revise y vuelva a intentar.");
      setError("Estructura de datos incorrecta. Por favor, revise y vuelva a intentar.");
    }
  };

  return (
    <div className="user-form-container">
      <h2>{isEdit ? "Editar Usuario" : "Detalle del Usuario"}</h2>
      <form onSubmit={handleSubmit}>
        <label>ID</label>
        <input name="id" value={usuario.id} disabled />

        {(isEdit || isView) && (
          <>
            <label>Nombre Completo</label>
            <input
              name="nombreCompleto"
              value={usuario.nombreCompleto}
              onChange={handleChange}
              disabled={isView}
            />

            <label>Correo</label>
            <input
              name="correo"
              value={usuario.correo}
              onChange={handleChange}
              disabled={isView}
            />
          </>
        )}

        {/* En ambos modos se muestra contraseña editable */}
        <label>Contraseña {isEdit ? "(opcional)" : ""}</label>
        <input
          name="contrasena"
          type="password"
          value={usuario.contrasena}
          onChange={handleChange}
        />

        {(isEdit || isView) && (
          <>
            <label>CURP</label>
            <input
              name="curp"
              value={usuario.curp}
              onChange={handleChange}
              disabled={isView}
            />

            <div className="checkbox-group">
              <label>
                <input
                  type="checkbox"
                  name="cuentaBloqueada"
                  checked={usuario.cuentaBloqueada}
                  onChange={handleChange}
                  disabled={isView}
                />
                Cuenta Bloqueada
              </label>
            </div>

            <div className="checkbox-group">
              <label>
                <input
                  type="checkbox"
                  name="dadoDeBaja"
                  checked={usuario.dadoDeBaja}
                  onChange={handleChange}
                  disabled={isView}
                />
                Dado de Baja
              </label>
            </div>

            <label>Última Sesión</label>
            <input
              name="ultimaSesion"
              type="date"
              value={usuario.ultimaSesion}
              onChange={handleChange}
              disabled={isView}
            />

            <label>Rol</label>
            <input
              name="rol"
              value={usuario.rol}
              onChange={handleChange}
              disabled={isView}
            />
          </>
        )}

        {(isEdit || isView) && <button type="submit">Guardar Cambios</button>}

        <button type="button" onClick={onBack} className="back-button">
          Volver
        </button>

        {success && <p style={{ color: "green", marginTop: "1rem" }}>{success}</p>}
        {error && <p style={{ color: "red", marginTop: "1rem" }}>{error}</p>}
      </form>
    </div>
  );
};

export default User;
