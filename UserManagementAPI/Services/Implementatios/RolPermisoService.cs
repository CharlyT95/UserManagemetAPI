using Aduanas.Aci.Usuarios.Api.DTOs.RolPermiso;
using Aduanas.Aci.Usuarios.Api.Errors.RolPermiso;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Data;
using UserManagementAPI.DTOs.Permiso;
using UserManagementAPI.Models;

namespace Aduanas.Aci.Usuarios.Api.Services.Implementatios
{
    public class RolPermisoService
    {
        private readonly UserManagementDbContext _context;
        private readonly IMapper _mapper;

        public RolPermisoService(UserManagementDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> AsignarRolPermiso(CreateRolPermisoDTO rolpermiso)
        {
            var data = _mapper.Map<RolPermiso>(rolpermiso);

            var validarPermiso = await _context.Permiso.AnyAsync(permiso => permiso.IdPermiso == rolpermiso.IdPermiso);
            if (!validarPermiso)
                throw new Exception(RolPermisoErrors.PermisoNoEncontrado);

            var validarRol = await _context.Rol.AnyAsync(rol => rol.IdRol == rolpermiso.IdRol);
            if (!validarRol)
                throw new Exception(RolPermisoErrors.RolNoEncontrado);

            //Auditoria
            data.FechaCreacion = DateTime.Now;
            data.Activo = true;

            _context.RolPermiso.Add(data);
            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<bool> ModificarRolPermiso(UpdateRolPermisoDTO rolpermiso)
        {
            var entity = await _context.RolPermiso
                .FirstOrDefaultAsync(rp => rp.IdRolPermiso == rolpermiso.IdRolPermiso);

            if (entity == null)
                throw new Exception(RolPermisoErrors.RolPermisoNoEncontrado);

            var validarPermiso = await _context.Permiso
                .AnyAsync(p => p.IdPermiso == rolpermiso.IdPermiso);

            if (!validarPermiso)
                throw new Exception(RolPermisoErrors.PermisoNoEncontrado);

            var validarRol = await _context.Rol
                .AnyAsync(r => r.IdRol == rolpermiso.IdRol);

            if (!validarRol)
                throw new Exception(RolPermisoErrors.RolNoEncontrado);

            _mapper.Map(rolpermiso, entity);

            // Auditoría
            entity.FechaModificacion = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
