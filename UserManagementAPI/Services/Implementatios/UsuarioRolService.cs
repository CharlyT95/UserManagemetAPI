using Aduanas.Aci.Usuarios.Api.DTOs.UsuarioRol;
using Aduanas.Aci.Usuarios.Api.Errors.UsuarioRol;
using AutoMapper;
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

        public async Task<CreateUsuarioRolDTO> CreateUsuarioRolAsync(CreateUsuarioRolDTO usuarioRol)
        {
            // Validar Usuario existente y Rol existente
            var validarUsuarioExistente = _context.Usuario
                .AnyAsync(u => u.IdUsuario == usuarioRol.IdUsuario);
            var validarRolExistente = _context.Rol
                .AnyAsync(r => r.IdRol == usuarioRol.IdRol);

            await Task.WhenAll(validarUsuarioExistente, validarRolExistente);
            if (!validarUsuarioExistente.Result)
                throw new Exception(UsuarioRolErrors.UsuarioNoExiste);

            if (!validarRolExistente.Result)
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


    }
}
