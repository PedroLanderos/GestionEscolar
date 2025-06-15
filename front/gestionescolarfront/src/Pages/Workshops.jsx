import React, { useEffect, useState, useContext } from "react";
import axios from "axios";
import { AuthContext } from "../Context/AuthContext";
import "./CSS/AssignSubject.css"; // Reutilizamos estilo de asignaciones

const Workshops = () => {
  const { auth } = useContext(AuthContext);
  const [grado, setGrado] = useState(null);
  const [talleres, setTalleres] = useState([]);
  const [puedeInscribirse, setPuedeInscribirse] = useState(false);
  const [mensaje, setMensaje] = useState("");
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchGradoYCiclo = async () => {
      try {
        const userId = auth.user?.id;

        // 1. Obtener grado del alumno
        const gradoRes = await axios.get(`http://localhost:5000/api/usuario/ObtenerGradoDeAlumno/${userId}`);
        const gradoAlumno = gradoRes.data;
        setGrado(gradoAlumno);

        // 2. Obtener ciclo escolar actual
        const cicloRes = await axios.get("http://localhost:5004/api/CicloEscolar/actual");
        const ciclo = cicloRes.data;

        const fechaInicio = new Date(ciclo.fechainicio);
        const hoy = new Date();
        const diferenciaDias = Math.floor((hoy - fechaInicio) / (1000 * 60 * 60 * 24));

        // 3. Validar si han pasado al menos 14 días
        if (diferenciaDias < 14) {
          setPuedeInscribirse(false);
          setMensaje("❌ Aún no es tiempo de inscribirse a talleres. Espera al menos 2 semanas desde el inicio del ciclo.");
          return;
        }

        setPuedeInscribirse(true);

        // 4. Obtener talleres por grado
        const talleresRes = await axios.get(`http://localhost:5001/api/SubjectAssignment/obtenerTalleresPorGrado/${gradoAlumno}`);
        setTalleres(talleresRes.data);
      } catch (err) {
        console.error(err);
        setError("Error al cargar la información. Intenta nuevamente más tarde.");
      } finally {
        setLoading(false);
      }
    };

    fetchGradoYCiclo();
  }, [auth.user?.id]);

  const handleAsignarTaller = async (courseId) => {
    try {
      const userId = auth.user?.id;

      await axios.post(`http://localhost:5002/api/Schedule/asignarTallerEspaciosLibres/${userId}/${courseId}`);

      setMensaje("✅ Taller asignado exitosamente.");
      setError("");
    } catch (err) {
      console.error(err);
      setError("❌ No se pudo asignar el taller. Puede que no haya espacios libres o ya estés inscrito.");
      setMensaje("");
    }
  };

  if (loading) return <p>Cargando información...</p>;

  return (
    <div className="assign-container">
      <h2>Talleres Disponibles</h2>

      {mensaje && <p className="success-message">{mensaje}</p>}
      {error && <p className="error">{error}</p>}

      {!puedeInscribirse ? (
        <p style={{ textAlign: "center", color: "#999" }}>{mensaje}</p>
      ) : talleres.length === 0 ? (
        <p style={{ textAlign: "center" }}>No hay talleres disponibles para tu grado actualmente.</p>
      ) : (
        <div className="list-box">
          <ul>
            {talleres.map((taller) => (
              <li key={taller.id}>
                <strong>{taller.nombre}</strong> — {taller.descripcion || "Sin descripción"}
                <br />
                <button style={{ marginTop: "0.5rem" }} onClick={() => handleAsignarTaller(taller.subjectId)}>
                  Asignarme a este taller
                </button>
              </li>
            ))}
          </ul>
        </div>
      )}
    </div>
  );
};

export default Workshops;
