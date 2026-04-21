using Aduanas.Aci.Usuarios.Api.Errors.Permiso;
using Aduanas.Aci.Usuarios.Api.Errors.Usuario;
using Aduanas.Aci.Usuarios.Api.Extensions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Data;
using UserManagementAPI.DTOs.Usuario;
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
                throw new Exception(UsuarioErrors.LoginUsuarioDuplicado);

            // Auditoría
            data.FechaCreacion = DateTime.Now;

            _context.Usuario.Add(data);
            await _context.SaveChangesAsync();

            return _mapper.Map<UsuarioDTO>(data);
        }

        public async Task<List<UsuarioDTO>> GetUsuariosAsync()
        {
            var ListaUsuarios = await _context.Usuario.Where(usuario => usuario.Activo).OrderBy(usuario => usuario.IdUsuario).ProjectTo<UsuarioDTO>(_mapper.ConfigurationProvider).ToListAsync();
            return ListaUsuarios;
        }

        public async Task<UsuarioDTO> GetUsuarioByIdAsync(int id)
        {
            var validarActivo = await _context.Usuario.AnyAsync(usuario => usuario.IdUsuario == id && usuario.Activo);
            if (!validarActivo)
                throw new Exception(UsuarioErrors.UsuarioInactivoBoolInactivo);

            var usuario = await _context.Usuario.Where(usuario => usuario.IdUsuario == id).ProjectTo<UsuarioDTO>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
            return usuario;
        }

        public async Task<UsuarioDTO> UpdateUsuarioAsync(UpdateUsuarioDTO usuario)
        {
            var nombreNormalizado = usuario.UsuarioLogin.NormalizarTexto();
            var correoNormalizado = usuario.CorreoElectronico.NormalizarTexto();

            var data = await _context.Usuario.FirstOrDefaultAsync(x => x.IdUsuario == usuario.IdUsuario && x.Activo);
            if (data == null)
                throw new Exception(UsuarioErrors.UsuarioNoEncontrado);

            var usuarioDuplicado = await _context.Usuario
                 .Where(p => p.IdUsuario != usuario.IdUsuario && p.Activo)
                 .Select(p => new
                 {
                     UsuarioLogin = (p.UsuarioLogin ?? "").Trim().Replace(" ", "").ToLower(),
                     Correo = (p.CorreoElectronico ?? "").Trim().Replace(" ", "").ToLower()
                 })
                 .FirstOrDefaultAsync(p =>
                     p.UsuarioLogin == nombreNormalizado ||
                     p.Correo == correoNormalizado);

            if (usuarioDuplicado != null)
            {
                if (usuarioDuplicado.UsuarioLogin == nombreNormalizado)
                    throw new Exception(UsuarioErrors.UsuarioLoginYaExiste);

                if (usuarioDuplicado.Correo == correoNormalizado)
                    throw new Exception(UsuarioErrors.CorreoDuplicado);
            }

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
