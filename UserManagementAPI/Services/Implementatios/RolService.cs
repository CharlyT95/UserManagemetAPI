using Aduanas.Aci.Usuarios.Api.Errors.Rol;
using Aduanas.Aci.Usuarios.Api.Errors.UsuarioRol;
using Aduanas.Aci.Usuarios.Api.Extensions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Data;
using UserManagementAPI.DTOs.Rol;
using UserManagementAPI.Models;

namespace Aduanas.Aci.Usuarios.Api.Services.Implementatios
{
    public class RolService
    {
        private readonly UserManagementDbContext _context;
        private readonly IMapper _mapper;

        public RolService(UserManagementDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<RolDTO>> getRoles()
        {
            var roles = await _context.Rol.Where(rol => rol.Activo == true).OrderBy(r => r.IdRol).ProjectTo<RolDTO>(_mapper.ConfigurationProvider).ToListAsync();
            return roles;
        }

        public async Task<RolDTO> getRolById(int id)
        {
            var rol = await _context.Rol.Where(r => r.IdRol == id && r.Activo == true).ProjectTo<RolDTO>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
            return rol;
        }

        public async Task<RolDTO> CreateRol(CreateRolDTO rol)
        {
            var nombreNormalizado = rol.Nombre.NormalizarTexto();
            var data = _mapper.Map<Rol>(rol);

            var validarNombre = await _context.Rol
                .AnyAsync(r => r.Activo &&
                    r.Nombre.Trim().Replace(" ", "").ToLower() == nombreNormalizado);

            if (validarNombre)
                throw new Exception(RolErrors.NombreDuplicado);

            //Auditoria
            data.FechaCreacion = DateTime.Now;

            _context.Rol.Add(data);
            await _context.SaveChangesAsync();
            return _mapper.Map<RolDTO>(data);
        }

        public async Task<RolDTO> UpdateRol(UpdateRolDTO rol)
        {
            var nombreNormalizado = rol.Nombre.NormalizarTexto();

            var data = await _context.Rol
                .FirstOrDefaultAsync(r => r.IdRol == rol.IdRol && r.Activo);
            if (data == null)
                throw new Exception(RolErrors.RolNoEncontrado);

            var validarNombre = await _context.Rol
                .AnyAsync(r => r.IdRol != rol.IdRol && r.Activo &&
                    r.Nombre.Trim().Replace(" ", "").ToLower() == nombreNormalizado);
            if (validarNombre)
                throw new Exception(RolErrors.NombreDuplicado);

            _mapper.Map(rol, data);

            // Auditoría
            data.FechaModificacion = DateTime.Now;

            await _context.SaveChangesAsync();
            return _mapper.Map<RolDTO>(data);
        }

        public async Task<bool> CambiarEstadoRol(int idRol, bool activo)
        {
            if (idRol <= 0)
                throw new Exception(RolErrors.RolNoEncontrado);

            var data = await _context.Rol
                .FirstOrDefaultAsync(ur => ur.IdRol == idRol);

            if (data == null)
                throw new Exception(RolErrors.RolNoEncontrado);

            if (!data.Activo && activo == false)
                throw new Exception(RolErrors.RolInactivoBoolInactivo);

            if (data.Activo && activo == true)
                throw new Exception(RolErrors.RolActivoBoolActivo);

            //Auditoria
            data.Activo = activo;

            await _context.SaveChangesAsync();
            return true;
        }


    }
}
