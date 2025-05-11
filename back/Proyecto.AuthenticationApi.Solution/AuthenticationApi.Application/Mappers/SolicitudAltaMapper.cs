using AuthenticationApi.Application.DTOs;
using AuthenticationApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationApi.Application.Mappers
{
    public static class SolicitudAltaMapper
    {
        public static SolicitudAltaDTO ToDTO(SolicitudAlta solicitudAlta)
        {
            if (solicitudAlta == null) return null!;
            return new SolicitudAltaDTO
            {
                NombreAlumno = solicitudAlta.NombreAlumno,
                CurpAlumno = solicitudAlta.CurpAlumno.ToUpper(),
                Grado = solicitudAlta.Grado,
                NombrePadre = solicitudAlta.NombrePadre,
                Telefono = solicitudAlta.Telefono,
                CorreoPadre = solicitudAlta.CorreoPadre
            };
        }
        public static IEnumerable<SolicitudAltaDTO> ToDTO(IEnumerable<SolicitudAlta> solicitudes)
        {
            if (solicitudes == null || !solicitudes.Any()) return null!;
            return solicitudes.Select(solicitud => ToDTO(solicitud));
        }
        public static SolicitudAlta ToEntity(SolicitudAltaDTO dto)
        {
            if (dto == null) return null!;

            return new SolicitudAlta
            {
                NombreAlumno = dto.NombreAlumno,
                CurpAlumno = dto.CurpAlumno.ToUpper(),
                Grado = dto.Grado,
                NombrePadre = dto.NombrePadre,
                Telefono = dto.Telefono,
                CorreoPadre = dto.CorreoPadre
            };
        }

        public static IEnumerable<SolicitudAlta> ToEntity(IEnumerable<SolicitudAltaDTO> dtos)
        {
            if (dtos == null || !dtos.Any()) return null!;
            return dtos.Select(dto => ToEntity(dto));
        }
    }
}
