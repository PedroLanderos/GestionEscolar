using AuthenticationApi.Application.DTOs;
using AuthenticationApi.Application.Interfaces;
using Llaveremos.SharedLibrary.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class UsuarioController(IUser userService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult<Response>> Register([FromBody] UsuarioDTO usuarioDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await userService.Register(usuarioDTO);
            return result.Flag ? Ok(result) : BadRequest(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<Response>> Login([FromBody] IniciarSesionDTO loginDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await userService.Login(loginDTO);
            return result.Flag ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<ActionResult<ObtenerUsuarioDTO>> GetUser(int id)
        {
            if (id <= 0)
                return BadRequest("ID invalido");

            var user = await userService.GetUser(id);
            return user is not null ? Ok(user) : NotFound("Usuario no encontrado");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<ObtenerUsuarioDTO>>> GetAllUsers()
        {
            var users = await userService.GetAllUsers();
            return users.Any() ? Ok(users) : NotFound("No se encontraron usuarios");
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Response>> EditUser([FromBody] EditarUsuarioDTO dto)
        {
            if (dto.Id <= 0)
                return BadRequest("ID invalido");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await userService.EditUserById(dto);
            return result.Flag ? Ok(result) : BadRequest(result);
        }
    }
}
