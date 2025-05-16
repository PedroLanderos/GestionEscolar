import './App.css';
import { BrowserRouter, Route, Routes, Navigate } from 'react-router-dom';
import LoginSignup from "./Pages/LoginSignup";
import MainPage from './Pages/MainPage';
import Register from './Pages/Register';
import Subjects from './Pages/Subjects';
import Schedule from './Pages/Schedule';
import { useContext } from 'react';
import { AuthContext } from './Context/AuthContext';
import Navbar from './Components/Navbar/Navbar';
import Footer from './Components/Footer/Footer';
import UsersTable from './Pages/UsersTable'; 
import User from './Pages/User'; 

const PrivateRoute = ({ children, allowedRoles }) => {
  const { auth } = useContext(AuthContext);

  if (!auth.isAuthenticated) {
    return <Navigate to="/login" />;
  }
  if (allowedRoles && !allowedRoles.includes(auth.user.role)) {
    return <Navigate to="/MenuPrincipal" />;
  }

  return children;
};

function App() {
  const { auth } = useContext(AuthContext);

  return (
    <BrowserRouter>
      <Navbar />

      <Routes>
        <Route path="/login" element={auth.isAuthenticated ? <Navigate to="/MenuPrincipal" /> : <LoginSignup />} />
        <Route path="/" element={<Navigate to="/login" />} />

        <Route
          path="/MenuPrincipal"
          element={
            <PrivateRoute>
              <MainPage />
            </PrivateRoute>
          }
        />

        <Route
          path="/Materias"
          element={
            <PrivateRoute allowedRoles={["Administrador"]}>
              <Subjects />
            </PrivateRoute>
          }
        />

        <Route
          path="/RegistrarAlumno"
          element={
            <PrivateRoute allowedRoles={["Administrador"]}>
              <Register />
            </PrivateRoute>
          }
        />

        <Route
          path="/schedule"
          element={
            <PrivateRoute allowedRoles={["Administrador"]}>
              <Schedule />
            </PrivateRoute>
          }
        />

        {/* ✅ Rutas nuevas */}
        <Route
          path="/usuarios/:userType"
          element={
            <PrivateRoute allowedRoles={["Administrador"]}>
              <UsersTable />
            </PrivateRoute>
          }
        />

        <Route
          path="/usuario/:id"
          element={
            <PrivateRoute allowedRoles={["Administrador"]}>
              <User />
            </PrivateRoute>
          }
        />
      </Routes>

      <Footer />
    </BrowserRouter>
  );
}

export default App;