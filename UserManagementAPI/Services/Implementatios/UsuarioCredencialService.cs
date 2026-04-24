using Aduanas.Aci.Usuarios.Api.DTOs.UsuarioCredencial;
using Aduanas.Aci.Usuarios.Api.Errors.UsuarioCredencial;
using Aduanas.Aci.Usuarios.Api.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Data;
using UserManagementAPI.DTOs.Permiso;
using UserManagementAPI.DTOs.Rol;
using UserManagementAPI.DTOs.Usuario;
using UserManagementAPI.Models;

namespace Aduanas.Aci.Usuarios.Api.Services.Implementatios
{
    public class UsuarioCredencialService
    {
        private readonly UserManagementDbContext _context;
        private readonly IMapper _mapper;
        private readonly IPasswordService _passwordService;

        public UsuarioCredencialService(UserManagementDbContext context, IMapper mapper, IPasswordService passwordService)
        {
            _context = context;
            _mapper = mapper;
            _passwordService = passwordService;
        }

        public async Task<bool> CreateCredeciales(CreateUsuarioCredencialDTO uc)
        {
            var data = _mapper.Map<UsuarioCredencial>(uc);
            //Validar que el usuario exista
            var validarUsuario = await _context.Usuario.AnyAsync(u => u.IdUsuario == uc.IdUsuario && u.Activo == true);
            if (!validarUsuario)
                throw new Exception(UsuarioCredencialErrors.UsuarioNoExistente);

            //Validar que el usuario ya posee credenciales
            var validarCredecialesExistentes = await _context.UsuarioCredencial.AnyAsync(ucl => ucl.IdUsuario == uc.IdUsuario);
            if (validarCredecialesExistentes)
                throw new Exception(UsuarioCredencialErrors.CredencialExistente);

            //Almacenar contraseña utilizando HMACSHA512
            _passwordService.CreatePassword(uc.Password, out byte[] hash, out byte[] salt);

            data.PasswordHash = hash;
            data.PasswordSalt = salt;
            data.Iteraciones = 1000;
            data.FechaUltimoCambio = DateTime.Now;
            data.IntentosFallidos = 0;
            data.BloqueoTemporal = false;

            _context.UsuarioCredencial.Add(data);
            await _context.SaveChangesAsync();
            // return _mapper.Map<CreateUsuarioCredencialDTO>(data);
            return true;

        }

        public async Task<bool> ChangePassword(CambiarPasswordDTO passwordDTO)
        {
            var validaractivo = await _context.Usuario.AnyAsync(u => u.IdUsuario == passwordDTO.IdUsuario && u.Activo == false);
            if (validaractivo)
                throw new Exception(UsuarioCredencialErrors.UsuarioInactivo);
            
            var usuario = await _context.UsuarioCredencial
                .FirstOrDefaultAsync(u => u.IdUsuario == passwordDTO.IdUsuario);

            if (usuario == null)
                throw new Exception(UsuarioCredencialErrors.UsuarioNoExistente);

            // Validar contraseña actual
            var esValido = _passwordService.VerifyPassword(
                passwordDTO.PasswordActual,
                usuario.PasswordHash,
                usuario.PasswordSalt
            );

            if (!esValido)
                throw new Exception(UsuarioCredencialErrors.PasswordIncorrecto);

            // Crear nuevo hash
            _passwordService.CreatePassword(
                passwordDTO.PasswordNueva,
                out byte[] hash,
                out byte[] salt
            );

            usuario.PasswordHash = hash;
            usuario.PasswordSalt = salt;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<DesbloqueoUsuarioDTO> UnlockUsuario(DesbloqueoUsuarioDTO usuarioDTO)
        {
            var data = _mapper.Map<UsuarioCredencial>(usuarioDTO);
            //Validar usuario
            var validarUsuario = await _context.Usuario.AnyAsync(u => u.IdUsuario == usuarioDTO.IdUsuario && u.Activo == true);
            if (!validarUsuario)
                throw new Exception(UsuarioCredencialErrors.UsuarioNoExistente);

            data.BloqueoTemporal = false;
            await _context.SaveChangesAsync();
            return _mapper.Map<DesbloqueoUsuarioDTO>(data);
        }

        public async Task<LoginResponseDTO> Login(LoginDTO login)
        {
            var validaractivo = await _context.Usuario
                .AnyAsync(u => u.UsuarioLogin == login.UsuarioLogin && u.Activo == false);

            if (validaractivo)
                throw new Exception(UsuarioCredencialErrors.CredencialesIncorrectas);

            var usuarioData = await (
                from u in _context.Usuario
                join c in _context.UsuarioCredencial
                    on u.IdUsuario equals c.IdUsuario
                where u.UsuarioLogin == login.UsuarioLogin && u.Activo
                select new
                {
                    Usuario = u,
                    Credencial = c
                }
            ).FirstOrDefaultAsync();

            if (usuarioData == null)
                throw new Exception(UsuarioCredencialErrors.CredencialesIncorrectas);

            var usuario = usuarioData.Usuario;
            var credencial = usuarioData.Credencial;

            if (credencial.BloqueoTemporal)
                throw new Exception(UsuarioCredencialErrors.Bloqueo);

            var loginValido = _passwordService.VerifyPassword(
                login.Password,
                credencial.PasswordHash,
                credencial.PasswordSalt
            );

            if (!loginValido)
            {
                credencial.IntentosFallidos++;

                if (credencial.IntentosFallidos > 5)
                {
                    credencial.BloqueoTemporal = true;
                    await _context.SaveChangesAsync();
                    throw new Exception(UsuarioCredencialErrors.BloqueoAutomatico);
                }

                await _context.SaveChangesAsync();
                throw new Exception(UsuarioCredencialErrors.CredencialesIncorrectas);
            }

            if (credencial.IntentosFallidos > 0)
            {
                credencial.IntentosFallidos = 0;
                await _context.SaveChangesAsync();
            }

            var roles = await _context.UsuarioRol
                .Where(ur => ur.IdUsuario == usuario.IdUsuario && ur.Activo && ur.Rol.Activo)
                .Select(ur => new LoginResponseRolesDTO
                {
                    IdRol = ur.IdRol,
                    Nombre = ur.Rol.Nombre,

                    Permisos = _context.RolPermiso
                        .Where(rp => rp.IdRol == ur.IdRol && rp.Activo && rp.Rol.Activo && rp.Permiso.Activo)
                        .Select(rp => new PermisoDTO
                        {
                            IdPermiso = rp.IdPermiso,
                            CodigoPermiso = rp.Permiso.CodigoPermiso,
                            Descripcion = rp.Permiso.Descripcion,
                            Modulo = rp.Permiso.Modulo,
                            Accion = rp.Permiso.Accion,
                            Referencia = rp.Permiso.Referencia
                        }).ToList()
                })
                .ToListAsync();

            return new LoginResponseDTO
            {
                Nombres = usuario.Nombres,
                Apellidos = usuario.Apellidos,
                UsuarioLogin = usuario.UsuarioLogin,
                Roles = roles
            };
        }
    }
}