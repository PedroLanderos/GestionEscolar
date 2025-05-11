import React, { createContext, useState, useEffect, useCallback } from "react";
import { jwtDecode } from "jwt-decode";
import { useIdleTimer } from "react-idle-timer";

export const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const [auth, setAuth] = useState({
    isAuthenticated: false,
    token: null,
    user: null,
  });

  const logout = useCallback(() => {
    console.log("⏳ Sesión cerrada por inactividad");
    localStorage.removeItem("token");
    localStorage.removeItem("user");
    setAuth({ isAuthenticated: false, token: null, user: null });
  }, []);

  useIdleTimer({
    timeout: 1000 * 60 * 5,
    onIdle: logout,
    debounce: 500,
  });

  const checkSession = () => {
    const token = localStorage.getItem("token");
    const user = JSON.parse(localStorage.getItem("user"));

    if (token && user) {
      setAuth({
        isAuthenticated: true,
        token,
        user,
      });
    } else {
      logout();
    }
  };

  useEffect(() => {
    checkSession();
  }, [logout]);

  const login = (token) => {
    if (!token || typeof token !== "string") {
      console.error("Token inválido al intentar decodificar:", token);
      return;
    }

    try {
      const decodedToken = jwtDecode(token);
      console.log("🔍 Token decodificado:", decodedToken);

      const user = {
        id: decodedToken.UserId || null,
        name:
          decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"] ||
          decodedToken.name ||
          "Usuario",
        email:
          decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"] ||
          decodedToken.email ||
          "Sin correo",
        role:
          decodedToken["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] ||
          decodedToken.role ||
          "Normal",
      };

      localStorage.setItem("token", token);
      localStorage.setItem("user", JSON.stringify(user));

      setAuth({
        isAuthenticated: true,
        token,
        user,
      });

      console.log("✅ Usuario autenticado correctamente:", user);
    } catch (error) {
      console.error("❌ Error al decodificar el token:", error);
      logout();
    }
  };

  return (
    <AuthContext.Provider value={{ auth, login, logout, checkSession }}>
      {children}
    </AuthContext.Provider>
  );
};
