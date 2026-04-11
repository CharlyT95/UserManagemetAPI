using Aduanas.Aci.Usuarios.Api.DTOs.UsuarioRol;
using Aduanas.Aci.Usuarios.Api.Errors.UsuarioRol;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Data;
using UserManagementAPI.DTOs.Rol;
using UserManagementAPI.Models;

namespace Aduanas.Aci.Usuarios.Api.Services.Implementatios
{
    public class UsuarioRolService
    {
        private readonly UserManagementDbContext _context;
        private readonly IMapper _mapper;

        public UsuarioRolService(UserManagementDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CreateUsuarioRolDTO> CreateUsuarioRol(CreateUsuarioRolDTO usuarioRol)
        {
            // Validar Usuario existente y Rol existente
            var usuario = await _context.Usuario
                .Where(u => u.IdUsuario == usuarioRol.IdUsuario)
                .Select(u => u.IdUsuario)
                .FirstOrDefaultAsync();

            if (usuario == 0)
                throw new Exception(UsuarioRolErrors.UsuarioNoExiste);

            var rol = await _context.Rol
                .Where(r => r.IdRol == usuarioRol.IdRol && r.Activo == true)
                .Select(r => r.IdRol)
                .FirstOrDefaultAsync();

            if (rol == 0)
                throw new Exception(UsuarioRolErrors.RolNoExiste);

            // Validar si el rol ya fue asignado al usuario anteriormente
            var validarUsuarioRol = await _context.UsuarioRol
                .AnyAsync(p => p.IdRol == usuarioRol.IdRol && p.IdUsuario == usuarioRol.IdUsuario);

            if (validarUsuarioRol)
                throw new Exception(UsuarioRolErrors.RolYaAsignado);

            var data = _mapper.Map<UsuarioRol>(usuarioRol);

            // Auditoría correcta
            data.FechaCreacion = DateTime.Now;

            _context.UsuarioRol.Add(data);
            await _context.SaveChangesAsync();
            return _mapper.Map<CreateUsuarioRolDTO>(data);
        }

        public async Task<List<UsuarioRolDTO>> GetRolPorUsuario(int idUsuario)
        {
            if (idUsuario == null || idUsuario == 0)
                throw new Exception(UsuarioRolErrors.UsuarioNull);

            var getRoles = await _context.UsuarioRol
                .Include(u => u.Usuario).Include(r => r.Rol)
                .Where(ur => ur.IdUsuario == idUsuario).ProjectTo<UsuarioRolDTO>(_mapper.ConfigurationProvider).ToListAsync();
            return getRoles;

        }

        public async Task<bool> CambiarEstadoUsuarioRol(int idUsuarioRol, bool activo)
        {
            if (idUsuarioRol <= 0)
                throw new Exception(UsuarioRolErrors.UsuarioNull);

            var data = await _context.UsuarioRol
                .FirstOrDefaultAsync(ur => ur.IdUsuarioRol == idUsuarioRol);

            if (data == null)
                throw new Exception(UsuarioRolErrors.RolAsignadoNull);

            if (!data.Activo && activo == false)
                throw new Exception(UsuarioRolErrors.RolInactivoBoolInactivo); 
            
            if (data.Activo && activo == true)
                throw new Exception(UsuarioRolErrors.RolActivoBoolActivo);

            //Auditoria
            data.Activo = activo;

            await _context.SaveChangesAsync();
            return true;
        }


    }
}
