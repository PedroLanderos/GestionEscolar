import React, { useEffect, useState, useContext } from "react";
import axios from "axios";
import { AuthContext } from "../Context/AuthContext";
import "./CSS/AssignSubject.css";

const Workshops = () => {
  const { auth } = useContext(AuthContext);
  const [grado, setGrado] = useState(null);
  const [talleres, setTalleres] = useState([]);
  const [puedeInscribirse, setPuedeInscribirse] = useState(false);
  const [mensaje, setMensaje] = useState("");
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(true);
  const [tooltip, setTooltip] = useState({ visible: false, content: "", x: 0, y: 0 });

  useEffect(() => {
    const fetchGradoYCiclo = async () => {
      try {
        const userId = auth.user?.id;

        const gradoRes = await axios.get(`http://localhost:5000/api/usuario/ObtenerGradoDeAlumno/${userId}`);
        const gradoAlumno = gradoRes.data;
        setGrado(gradoAlumno);

        const cicloRes = await axios.get("http://localhost:5004/api/CicloEscolar/actual");
        const ciclo = cicloRes.data;

        const fechaInicio = new Date(ciclo.fechainicio);
        const hoy = new Date();
        const diferenciaDias = Math.floor((hoy - fechaInicio) / (1000 * 60 * 60 * 24));

        if (diferenciaDias < 14) {
          setPuedeInscribirse(false);
          setMensaje("❌ Aún no es tiempo de inscribirse a talleres.");
          return;
        }

        setPuedeInscribirse(true);

        const talleresRes = await axios.get(`http://localhost:5001/api/SubjectAssignment/obtenerTalleresPorGrado/${gradoAlumno}`);
        const rawTalleres = talleresRes.data;

        const talleresEnriquecidos = await Promise.all(
          rawTalleres.map(async (t) => {
            try {
              const [subjectRes, userRes] = await Promise.all([
                axios.get(`http://localhost:5001/api/Subject/obtenerPorCodigo/${t.subjectId}`),
                axios.get(`http://localhost:5000/api/usuario/obtenerUsuarioPorId/${t.userId}`),
              ]);

              return {
                ...t,
                nombreMateria: subjectRes.data?.nombre || "Materia desconocida",
                nombreDocente: userRes.data?.nombreCompleto || "Docente desconocido",
                courseId: `${t.subjectId}-${t.userId}`, // <- clave
              };
            } catch (err) {
              console.error("❌ Error enriqueciendo taller:", err);
              return {
                ...t,
                nombreMateria: "Desconocido",
                nombreDocente: "Desconocido",
                courseId: `${t.subjectId}-${t.userId}`,
              };
            }
          })
        );

        setTalleres(talleresEnriquecidos);
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
      setError("❌ No se pudo asignar el taller.");
      setMensaje("");
    }
  };

  const showTooltip = (event, taller) => {
    setTooltip({
      visible: true,
      content: `Materia: ${taller.nombreMateria}\nDocente: ${taller.nombreDocente}`,
      x: event.clientX + 10,
      y: event.clientY + 10,
    });
  };

  const hideTooltip = () => {
    setTooltip({ visible: false, content: "", x: 0, y: 0 });
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
              <li
                key={taller.courseId}
                onMouseEnter={(e) => showTooltip(e, taller)}
                onMouseMove={(e) => tooltip.visible && setTooltip((prev) => ({ ...prev, x: e.clientX + 10, y: e.clientY + 10 }))}
                onMouseLeave={hideTooltip}
              >
                <strong>{taller.courseId}</strong>
                <br />
                <button
                  style={{ marginTop: "0.5rem" }}
                  onClick={() => handleAsignarTaller(taller.courseId)}
                >
                  Asignarme a este taller
                </button>
              </li>
            ))}
          </ul>
        </div>
      )}

      {tooltip.visible && (
        <div
          className="tooltip"
          style={{
            top: tooltip.y,
            left: tooltip.x,
            position: "fixed",
            background: "#f9f9f9",
            border: "1px solid #ccc",
            padding: "6px 10px",
            borderRadius: "4px",
            whiteSpace: "pre-line",
            zIndex: 999,
            fontSize: "0.9rem",
            maxWidth: "250px",
          }}
        >
          {tooltip.content}
        </div>
      )}
    </div>
  );
};

export default Workshops;
