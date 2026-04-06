using Aduanas.Aci.Usuarios.Api.Errors.Permiso;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Data;
using UserManagementAPI.DTOs.Permiso;
using UserManagementAPI.Helpers;
using UserManagementAPI.Models;

namespace Aduanas.Aci.Usuarios.Api.Services.Implementatios
{
    public class PermisoService
    {
        private readonly UserManagementDbContext _context;
        private readonly IMapper _mapper;

        public PermisoService(UserManagementDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CreatePermisoDTO> CreatePermisoAsync(CreatePermisoDTO permiso)
        {
            var data = _mapper.Map<Permiso>(permiso);
            var validarCodigo = await _context.Permiso.AnyAsync(p => p.CodigoPermiso == permiso.CodigoPermiso);

            if (validarCodigo)
                throw new Exception(PermisoErrors.CodigoPermisoDuplicado);

            //Auditoria
            data.FechaCreacion = DateTime.Now;

            _context.Permiso.Add(data);
            await _context.SaveChangesAsync();
            return _mapper.Map<CreatePermisoDTO>(data);
        }

        public async Task<List<PermisoDTO>> GetPermisos()
        {
            var permisos = await _context.Permiso.OrderBy(p => p.IdPermiso).ProjectTo<PermisoDTO>(_mapper.ConfigurationProvider).ToListAsync();
            return permisos;
        }

        public async Task<PermisoDTO> GetPermisoById(int id)
        {
            var permiso = await _context.Permiso.Where(p => p.IdPermiso == id).OrderBy(p => p.IdPermiso).ProjectTo<PermisoDTO>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
            return permiso;
        }

        public async Task<UpdatePermisoDTO> UpdatePermiso(UpdatePermisoDTO permiso)
        {
            var data = await _context.Permiso.FirstOrDefaultAsync(p => p.IdPermiso == permiso.IdPermiso);
            if (data == null)
                throw new Exception("Permiso no encontrado");

            _mapper.Map(permiso, data);

            //Auditoria
            data.FechaModificacion = DateTime.Now;

            _context.Permiso.Update(data);
            await _context.SaveChangesAsync();
            return _mapper.Map<UpdatePermisoDTO>(data);

        }

        public async Task<DeactivatePermisoDTO> DeactivatePermisoAsync(DeactivatePermisoDTO permiso)
        {
            var data = await _context.Permiso.FirstOrDefaultAsync(p => p.IdPermiso == permiso.IdPermiso);
            if (data == null)
                throw new Exception("Permiso no encontrado");
            _mapper.Map(permiso, data);

            //Auditoria
            data.Activo = false;

            _context.Permiso.Update(data);
            await _context.SaveChangesAsync();
            return _mapper.Map<DeactivatePermisoDTO>(data);

        }
    }
}
