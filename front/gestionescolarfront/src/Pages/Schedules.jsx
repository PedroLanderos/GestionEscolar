import React, { useEffect, useState } from "react";
import axios from "axios";
import "./CSS/RegisterRequest.css";
import { SCHEDULE_API } from "../Config/apiConfig";

const Schedules = () => {
    const [schedules, setSchedules] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
      const [editingSchedule, setEditingSchedule] = useState(null);

    const fetchSchedules = async () =>{
        try {
            const response = await axios.get(`${SCHEDULE_API}/Schedule/obtenerHorarios`);
            setSchedules(response.data);
        } catch (error) {
            console.error("❌ Error al obtener solicitudes:", error);
            setError("Error al obtener las solicitudes.");
        }finally{
            setLoading(false);
        }
    }

    useEffect(() => {
        fetchSchedules();
    }, []);

    return (
        <div>
            <h2>Horarios</h2>


            {loading && <p>Cargando materias...</p>}
            {error && <p style={{ color: "red" }}>{error}</p>}

            {!loading && !error &&(
            <table>
                <thead>
                    <tr>
                    <th>Grado</th>
                    <th>Grupo</th>
                    </tr>
                </thead>
                <tbody>
                    {subjects.map((schedule, index) => (
                    <tr key={index}>
                        <td>{schedule.grado}</td>
                        <td>{schedule.grupo}</td>
                        <td>
                            <button onClick={() => setEditingSchedule(schedule)}>Consultar horario</button>
                        </td>
                    </tr>
                    ))}
                </tbody>
            </table>
        )}

        {!loading && !error && subjects.length === 0 && (
            <p>No hay materias registradas.</p>
        )}
        </div>
    );
};

export default Schedules;   