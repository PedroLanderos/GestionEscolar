import React, { useEffect, useState } from "react";
import axios from "axios";
import "./CSS/UsersTable.css";
import { AUTH_API } from "../Config/apiConfig";

const endpointMap = {
  alumnos: `${AUTH_API}/usuario/obtenerAlumnos`,
  docentes: `${AUTH_API}/usuario/obtenerDocentes`,
  tutores: `${AUTH_API}/usuario/obtenerTutores`,
  administradores: `${AUTH_API}/usuario/obtenerAdministradores`,
};

const titleMap = {
  alumnos: "Alumnos",
  docentes: "Docentes",
  tutores: "Tutores",
  administradores: "Administradores",
};

const UsersTable = ({ userType }) => {
  const [usuarios, setUsuarios] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchUsers = async () => {
      setLoading(true);
      try {
        const res = await axios.get(endpointMap[userType]);
        setUsuarios(res.data);
      } catch (error) {
        console.error("❌ Error al obtener usuarios:", error);
      } finally {
        setLoading(false);
      }
    };

    if (userType) {
      fetchUsers();
    }
  }, [userType]);

  return (
    <div className="users-table-container">
      <h2>Usuarios - {titleMap[userType] || "Sin tipo"}</h2>
      {loading ? (
        <p>Cargando usuarios...</p>
      ) : (
        <table>
          <thead>
            <tr>
              <th>ID</th>
              <th>Nombre</th>
              <th>Correo</th>
              <th>CURP</th>
              <th>Bloqueado</th>
              <th>Dado de Baja</th>
              <th>Última Sesión</th>
              <th>Rol</th>
            </tr>
          </thead>
          <tbody>
            {usuarios.map((u) => (
              <tr key={u.id}>
                <td>{u.id}</td>
                <td>{u.nombreCompleto}</td>
                <td>{u.correo}</td>
                <td>{u.curp}</td>
                <td>{u.cuentaBloqueada ? "Sí" : "No"}</td>
                <td>{u.dadoDeBaja ? "Sí" : "No"}</td>
                <td>{new Date(u.ultimaSesion).toLocaleDateString()}</td>
                <td>{u.rol}</td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
};

export default UsersTable;
