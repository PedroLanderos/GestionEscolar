using ClassroomApi.Application.DTOs;
using ClassroomApi.Application.Interfaces;
using ClassroomApi.Domain.Entities;
using ClassroomApi.Presentation.Controllers;
using FakeItEasy;
using FluentAssertions;
using Llaveremos.SharedLibrary.Responses;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.ClassroomApi.Controllers
{
    public class ClassroomControllerTest
    {
        private readonly ICicloEscolar cicloService;
        private readonly CicloEscolarController controller;

        public ClassroomControllerTest()
        {
            cicloService = A.Fake<ICicloEscolar>();
            controller = new CicloEscolarController(cicloService);
        }

        [Fact]
        public async Task ObtenerTodos200()
        {
            // Arrange
            var ciclos = new List<CicloEscolarDTO>
            {
                new("1", DateTime.Now, DateTime.Now, DateTime.Now.AddMonths(6), true)
            };
            A.CallTo(() => cicloService.ObtenerCiclosEscolares()).Returns(ciclos);

            // Act
            var result = await controller.ObtenerTodos();

            // Assert
            var ok = result as OkObjectResult;
            ok.Should().NotBeNull();
            ok?.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task ObtenerPorId200()
        {
            // Arrange
            var dto = new CicloEscolarDTO("1", DateTime.Now, DateTime.Now, DateTime.Now.AddMonths(6), true);
            A.CallTo(() => cicloService.ObtenerCicloEscolarPorId("1")).Returns(dto);

            // Act
            var result = await controller.ObtenerPorId("1");

            // Assert
            var ok = result as OkObjectResult;
            ok.Should().NotBeNull();
            ok?.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task ObtenerPorId404()
        {
            // Arrange
            A.CallTo(() => cicloService.ObtenerCicloEscolarPorId("x")).Returns((CicloEscolarDTO)null);

            // Act
            var result = await controller.ObtenerPorId("x");

            // Assert
            var notFound = result as NotFoundObjectResult;
            notFound.Should().NotBeNull();
            notFound?.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task Crear200()
        {
            // Arrange
            var dto = new CicloEscolarDTO("1", DateTime.Now, DateTime.Now, DateTime.Now.AddMonths(6), true);
            var response = new Response(true, "Creado");
            A.CallTo(() => cicloService.AgregarCicloEscolar(dto)).Returns(response);

            // Act
            var result = await controller.Crear(dto);

            // Assert
            var ok = result as OkObjectResult;
            ok.Should().NotBeNull();
            ok?.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task Crear400_ModelState()
        {
            // Arrange
            var dto = new CicloEscolarDTO("", DateTime.Now, DateTime.Now, DateTime.Now.AddMonths(6), true);
            controller.ModelState.AddModelError("Id", "Required");

            // Act
            var result = await controller.Crear(dto);

            // Assert
            var badRequest = result as BadRequestObjectResult;
            badRequest.Should().NotBeNull();
            badRequest?.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task Actualizar200()
        {
            // Arrange
            var dto = new CicloEscolarDTO("1", DateTime.Now, DateTime.Now, DateTime.Now.AddMonths(6), false);
            var response = new Response(true, "Actualizado");
            A.CallTo(() => cicloService.ActualizarCicloEscolar(dto)).Returns(response);

            // Act
            var result = await controller.Actualizar("1", dto);

            // Assert
            var ok = result as OkObjectResult;
            ok.Should().NotBeNull();
            ok?.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task Actualizar400_IdMismatch()
        {
            // Arrange
            var dto = new CicloEscolarDTO("2", DateTime.Now, DateTime.Now, DateTime.Now.AddMonths(6), false);

            // Act
            var result = await controller.Actualizar("1", dto);

            // Assert
            var badRequest = result as BadRequestObjectResult;
            badRequest.Should().NotBeNull();
            badRequest?.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task Actualizar404()
        {
            // Arrange
            var dto = new CicloEscolarDTO("1", DateTime.Now, DateTime.Now, DateTime.Now.AddMonths(6), false);
            var response = new Response(false, "No encontrado");
            A.CallTo(() => cicloService.ActualizarCicloEscolar(dto)).Returns(response);

            // Act
            var result = await controller.Actualizar("1", dto);

            // Assert
            var notFound = result as NotFoundObjectResult;
            notFound.Should().NotBeNull();
            notFound?.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task Eliminar200()
        {
            // Arrange
            var dto = new CicloEscolarDTO("1", default, default, default, false);
            var response = new Response(true, "Eliminado");
            A.CallTo(() => cicloService.EliminarCicloEscolar(dto)).Returns(response);

            // Act
            var result = await controller.Eliminar("1");

            // Assert
            var ok = result as OkObjectResult;
            ok.Should().NotBeNull();
            ok?.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task Eliminar404()
        {
            // Arrange
            var dto = new CicloEscolarDTO("2", default, default, default, false);
            var response = new Response(false, "No encontrado");
            A.CallTo(() => cicloService.EliminarCicloEscolar(dto)).Returns(response);

            // Act
            var result = await controller.Eliminar("2");

            // Assert
            var notFound = result as NotFoundObjectResult;
            notFound.Should().NotBeNull();
            notFound?.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task ObtenerActual200()
        {
            // Arrange
            var entidad = new CicloEscolar
            {
                Id = "1",
                FechaRegistroCalificaciones = DateTime.Now,
                FechaInicio = DateTime.Today,
                FechaFin = DateTime.Today.AddMonths(6),
                EsActual = true
            };

            A.CallTo(() => cicloService.GetManyBy(A<Expression<Func<CicloEscolar, bool>>>._))
                .Returns(new List<CicloEscolar> { entidad });

            // Act
            var result = await controller.ObtenerActual();

            // Assert
            var ok = result as OkObjectResult;
            ok.Should().NotBeNull();
            ok?.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task ObtenerActual404()
        {
            // Arrange
            A.CallTo(() => cicloService.GetManyBy(A<Expression<Func<CicloEscolar, bool>>>._))
                .Returns(new List<CicloEscolar>());

            // Act
            var result = await controller.ObtenerActual();

            // Assert
            var notFound = result as NotFoundObjectResult;
            notFound.Should().NotBeNull();
            notFound?.StatusCode.Should().Be(404);
        }

    }
}
