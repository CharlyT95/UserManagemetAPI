using Aduanas.Aci.Usuarios.Api.Errors.Rol;
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
            var roles = await _context.Rol.OrderBy(r => r.IdRol).ProjectTo<RolDTO>(_mapper.ConfigurationProvider).ToListAsync();
            return roles;
        }

        public async Task<RolDTO> getRolById(int id)
        {
            var rol = await _context.Rol.Where(r => r.IdRol == id).ProjectTo<RolDTO>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
            return rol;
        }

        public async Task<CreateRolDTO> CreateRol(CreateRolDTO rol)
        {
            var data = _mapper.Map<Rol>(rol);
            var validarNombre = await _context.Rol.AnyAsync(r => r.Nombre == rol.Nombre);

            if (validarNombre)
                throw new Exception(RolErrors.NombreDuplicado);

            //Auditoria
            data.FechaCreacion = DateTime.Now;

            _context.Rol.Add(data);
            await _context.SaveChangesAsync();
            return _mapper.Map<CreateRolDTO>(data);
        }

        public async Task<UpdateRolDTO> UpdateRol(UpdateRolDTO rol)
        {
            var data = await _context.Rol.Where(r => r.IdRol == rol.IdRol).FirstOrDefaultAsync();
            if (data == null)
                throw new Exception("rol no encontrado");

            _context.Rol.Add(data);
            await _context.SaveChangesAsync();
            return _mapper.Map<UpdateRolDTO>(data);
        }

        public async Task<DeactivateRol> DeactivateRol(DeactivateRol rol)
        {
            var data = await _context.Rol.Where(r => r.IdRol == rol.IdRol).FirstOrDefaultAsync();
            if (data == null)
                throw new Exception("rol no encontrado");

            //Auditoria
            data.Activo = false;

            _context.Rol.Update(data);
            await _context.SaveChangesAsync();
            return _mapper.Map<DeactivateRol>(data);

        }


    }
}
