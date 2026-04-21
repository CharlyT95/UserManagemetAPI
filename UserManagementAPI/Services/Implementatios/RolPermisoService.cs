using Aduanas.Aci.Usuarios.Api.DTOs.RolPermiso;
using Aduanas.Aci.Usuarios.Api.Errors.RolPermiso;
using Aduanas.Aci.Usuarios.Api.Errors.UsuarioRol;
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

            var validarPermiso = await _context.Permiso.AnyAsync(permiso => permiso.IdPermiso == rolpermiso.IdPermiso && permiso.Activo);
            if (!validarPermiso)
                throw new Exception(RolPermisoErrors.PermisoNoEncontrado);

            var validarRol = await _context.Rol.AnyAsync(rol => rol.IdRol == rolpermiso.IdRol && rol.Activo);
            if (!validarRol)
                throw new Exception(RolPermisoErrors.RolNoEncontrado);

            var validarAsignacion = await _context.RolPermiso.AnyAsync(rp => rp.IdRol == rolpermiso.IdRol && rp.IdPermiso == rolpermiso.IdPermiso && rp.Activo);
            if (validarAsignacion)
                throw new Exception(RolPermisoErrors.PermisoYaAsignado);


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
                .FirstOrDefaultAsync(rp => rp.IdRolPermiso == rolpermiso.IdRolPermiso && rp.Activo);

            if (entity == null)
                throw new Exception(RolPermisoErrors.RolPermisoNoEncontrado);

            var validarPermiso = await _context.Permiso
                .AnyAsync(p => p.IdPermiso == rolpermiso.IdPermiso && p.Activo == true);

            if (!validarPermiso)
                throw new Exception(RolPermisoErrors.PermisoNoEncontrado);

            var validarRol = await _context.Rol
                .AnyAsync(r => r.IdRol == rolpermiso.IdRol && r.Activo == true);

            if (!validarRol)
                throw new Exception(RolPermisoErrors.RolNoEncontrado);


            _mapper.Map(rolpermiso, entity);

            // Auditoría
            entity.FechaModificacion = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<List<RolPermisoDTO>> GetRolPermiso(int idRol)
        {
            var validarRol = await _context.Rol.AnyAsync(r => r.IdRol == idRol);
            if (!validarRol)
                throw new Exception(RolPermisoErrors.RolNoEncontrado);

            var validarPermisos = await _context.RolPermiso
                .Where(r => r.IdRol == idRol &&
                            r.Activo &&
                            r.Permiso.Activo)
                .Include(r => r.Permiso)
                .ToListAsync();

            if (validarPermisos.Count == 0)
                throw new Exception(RolPermisoErrors.RolSinPermisos);

            var resultado = validarPermisos.Select
                (
                    r => new RolPermisoDTO
                    {
                        IdRolPermiso = r.IdRolPermiso,
                        CodigoPermiso = r.Permiso.CodigoPermiso,
                        Descripcion = r.Permiso.Descripcion
                    }
                ).ToList();

            return resultado;
        }

        public async Task<bool> CambiarEstadoRolPermisol(int idURolPermiso, bool activo)
        {
            if (idURolPermiso <= 0)
                throw new Exception(RolPermisoErrors.RolPermisoNoEncontrado);

            var data = await _context.RolPermiso
                .FirstOrDefaultAsync(ur => ur.IdRolPermiso == idURolPermiso);

            if (data == null)
                throw new Exception(RolPermisoErrors.RolPermisoNoEncontrado);

            if (!data.Activo && activo == false)
                throw new Exception(RolPermisoErrors.InactivoBoolInactivo);

            if (data.Activo && activo == true)
                throw new Exception(RolPermisoErrors.ActivoBoolActivo);

            //Auditoria
            data.Activo = activo;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
