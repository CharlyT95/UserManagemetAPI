using Aduanas.Aci.Usuarios.Api.DTOs.UsuarioRol;
using Aduanas.Aci.Usuarios.Api.Errors.UsuarioRol;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Data;
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

        public async Task<UsuarioRolDTO> CreateUsuarioRol(CreateUsuarioRolDTO dto)
        {
            var usuario = await _context.Usuario
                .Where(u => u.IdUsuario == dto.IdUsuario && u.Activo)
                .Select(u => new { u.Nombres, u.Apellidos })
                .FirstOrDefaultAsync();

            if (usuario == null)
                throw new Exception(UsuarioRolErrors.UsuarioNoExiste);

            var rol = await _context.Rol
                .Where(r => r.IdRol == dto.IdRol && r.Activo)
                .Select(r => new { r.Nombre })
                .FirstOrDefaultAsync();

            if (rol == null)
                throw new Exception(UsuarioRolErrors.RolNoExiste);

            var existe = await _context.UsuarioRol
                .AnyAsync(x => x.IdUsuario == dto.IdUsuario
                            && x.IdRol == dto.IdRol
                            && x.Activo);

            if (existe)
                throw new Exception(UsuarioRolErrors.RolYaAsignado);

            var entity = _mapper.Map<UsuarioRol>(dto);
            entity.FechaCreacion = DateTime.Now;
            entity.Activo = true;

            _context.UsuarioRol.Add(entity);
            await _context.SaveChangesAsync();

            return new UsuarioRolDTO
            {
                usuario = $"{usuario.Nombres} {usuario.Apellidos}",
                Rol = rol.Nombre
            };
        }

        public async Task<List<UsuarioRolGetDTO>> GetRolPorUsuario(int idUsuario)
        {
            if (idUsuario == null || idUsuario == 0)
                throw new Exception(UsuarioRolErrors.UsuarioNull);

            var getRoles = await _context.UsuarioRol
                .Include(u => u.Usuario).Include(r => r.Rol)
                .Where(ur => ur.IdUsuario == idUsuario && ur.Activo && ur.Rol.Activo).ProjectTo<UsuarioRolGetDTO>(_mapper.ConfigurationProvider).ToListAsync();
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
