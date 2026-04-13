using Aduanas.Aci.Usuarios.Api.Errors.Rol;
using Aduanas.Aci.Usuarios.Api.Errors.Usuario;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Data;
using UserManagementAPI.DTOs.Usuario;
using UserManagementAPI.Helpers;
using UserManagementAPI.Models;

namespace Aduanas.Aci.Usuarios.Api.Services.Implementatios
{
    public class UsuarioService
    {
        private readonly UserManagementDbContext _context;
        private readonly IMapper _mapper;

        public UsuarioService(UserManagementDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UsuarioDTO> CreateUserAsync(CreateUsuarioDTO usuario)
        {
            var data = _mapper.Map<Usuario>(usuario);
            var validarCorreo = await _context.Usuario.AnyAsync(c => c.CorreoElectronico == usuario.CorreoElectronico);
            var validarLogin = await _context.Usuario.AnyAsync(c => c.UsuarioLogin == usuario.UsuarioLogin);

            if (validarCorreo)
                throw new Exception(UsuarioErrors.CorreoDuplicado);

            if (validarLogin)
                throw new Exception(UsuarioErrors.LginUsuarioDuplicado);

            // Auditoría
            data.FechaCreacion = DateTime.Now;

            _context.Usuario.Add(data);
            await _context.SaveChangesAsync();

            return _mapper.Map<UsuarioDTO>(data);
        }

        public async Task<List<UsuarioDTO>> GetUsuariosAsync()
        {
            var ListaUsuarios = await _context.Usuario.OrderBy(usuario => usuario.IdUsuario).ProjectTo<UsuarioDTO>(_mapper.ConfigurationProvider).ToListAsync();
            return ListaUsuarios;
        }

        public async Task<UsuarioDTO> GetUsuarioByIdAsync(int id)
        {
            var usuario = await _context.Usuario.Where(usuario => usuario.IdUsuario == id).ProjectTo<UsuarioDTO>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
            return usuario;
        }

        public async Task<UsuarioDTO> UpdateUsuarioAsync(UpdateUsuarioDTO usuario)
        {
            var data = await _context.Usuario.FirstOrDefaultAsync(x => x.IdUsuario == usuario.IdUsuario);

            if (usuario == null)
                throw new Exception("Usuario no encontrado");

            _mapper.Map(usuario, data);

            //Auditoria
            data.FechaModificacion = DateTime.Now;

            _context.Usuario.Update(data);
            await _context.SaveChangesAsync();

            return _mapper.Map<UsuarioDTO>(data);
        }

        public async Task<bool> CambiarEstadoUsuario(int idUsuario, bool activo)
        {
            if (idUsuario <= 0)
                throw new Exception(UsuarioErrors.UsuarioNoEncontrado);

            var data = await _context.Usuario
                .FirstOrDefaultAsync(ur => ur.IdUsuario == idUsuario);

            if (data == null)
                throw new Exception(UsuarioErrors.UsuarioNoEncontrado);

            if (!data.Activo && activo == false)
                throw new Exception(UsuarioErrors.UsuarioInactivoBoolInactivo);

            if (data.Activo && activo == true)
                throw new Exception(UsuarioErrors.UsuarioActivoBoolActivo);

            //Auditoria
            data.Activo = activo;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
