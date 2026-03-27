using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Data;
using UserManagementAPI.DTOs.Usuario;
using UserManagementAPI.Helpers;
using UserManagementAPI.Models;

namespace UserManagementAPI.Services
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

        public async Task<CreateUsuarioDTO> CreateUserAsync(CreateUsuarioDTO usuario)
        {
            var data = _mapper.Map<Usuario>(usuario);

            // Auditoría
            data.FechaCreacion = DateTime.Now;

            _context.Usuario.Add(data);
            await _context.SaveChangesAsync();

            return _mapper.Map<CreateUsuarioDTO>(usuario);
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

        public async Task<UpdateUsuarioDTO> UpdateUsuarioAsync(UpdateUsuarioDTO usuario)
        {
            var data = await _context.Usuario.FirstOrDefaultAsync(x => x.IdUsuario == usuario.IdUsuario);

            if (usuario == null)
                throw new Exception("Usuario no encontrado");

            _mapper.Map(usuario, data);

            //Auditoria
            data.FechaModificacion = DateTime.Now;

            _context.Usuario.Update(data);
            await _context.SaveChangesAsync();

            return _mapper.Map<UpdateUsuarioDTO>(usuario);
        }

        public async Task<DeactivateUsuarioDTO> DeactivateeUsuarioAsync(DeactivateUsuarioDTO usuario)
        {
            var data = await _context.Usuario.FirstOrDefaultAsync(x => x.IdUsuario == usuario.IdUsuario);

            if (usuario == null)
                throw new Exception("Usuario no encontrado");

            _mapper.Map(usuario, data);

            //Auditoria
            data.Activo = false;

            _context.Usuario.Update(data);
            await _context.SaveChangesAsync();

            return _mapper.Map<DeactivateUsuarioDTO>(usuario);
        }
    }
}
