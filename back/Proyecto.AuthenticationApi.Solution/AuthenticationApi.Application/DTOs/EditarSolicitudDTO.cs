namespace AuthenticationApi.Application.DTOs
{
    public class EditarSolicitudDTO
    {
        public int Id { get; set; } 

        public string? NombreAlumno { get; set; }
        public string? CurpAlumno { get; set; }
        public int? Grado { get; set; }
        public string? NombrePadre { get; set; }
        public string? Telefono { get; set; }
        public string? CorreoPadre { get; set; }
        public bool? Procesado { get; set; }
    }
}
