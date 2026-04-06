using Aduanas.Aci.Usuarios.Api.DTOs.UsuarioCredencial;
using Aduanas.Aci.Usuarios.Api.Errors.UsuarioCredencial;
using Aduanas.Aci.Usuarios.Api.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Data;
using UserManagementAPI.DTOs.Permiso;
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

        public async Task<CreateUsuarioCredencialDTO> CreateCredeciales(CreateUsuarioCredencialDTO uc)
        {
            var data = _mapper.Map<UsuarioCredencial>(uc);
            //Validar que el usuario exista
            var validarUsuario = await _context.UsuariosCredencial.AnyAsync(u => u.IdUsuario == uc.IdUSuario);
            if (!validarUsuario)
                throw new Exception(UsuarioCredencialErrors.UsuarioNoExistente);

            //Validar que el usuario ya posee credenciales
            var validarCredecialesExistentes = await _context.UsuariosCredencial.AnyAsync(ucl => ucl.IdUsuario == uc.IdUSuario);
            if (validarCredecialesExistentes)
                throw new Exception(UsuarioCredencialErrors.CredencialExistente);

            //Almacenar contraseña utilizando HMACSHA512
            _passwordService.CreatePassword(uc.Password, out string hash, out string salt);

            data.PasswordHash = hash;
            data.PasswordSalt = salt;
            data.Iteraciones = 1000;
            data.FechaUltimoCambios = DateTime.Now;
            data.IntentosFallidos = 0;
            data.BloqueoTemporal = false;

            _context.UsuariosCredencial.Add(data);
            await _context.SaveChangesAsync();
            return _mapper.Map<CreateUsuarioCredencialDTO>(data);

        }

        public async Task<CambiarPasswordDTO> ChangePassword(CambiarPasswordDTO passwordDTO)
        {
            var data = _mapper.Map<UsuarioCredencial>(passwordDTO);

            //Validar usuario
            var validarUsuario = await _context.UsuariosCredencial.FirstOrDefaultAsync(u => u.IdUsuario == passwordDTO.IdUsuario);
            if (validarUsuario == null)
                throw new Exception(UsuarioCredencialErrors.UsuarioNoExistente);

            //Validar contraseña actual
            var validarPasswordActual = _passwordService.VerifyPassword(passwordDTO.PasswordActual, validarUsuario.PasswordHash, validarUsuario.PasswordSalt);
            if (!validarPasswordActual)
                throw new Exception(UsuarioCredencialErrors.PasswordIncorrecto);

            _passwordService.CreatePassword(passwordDTO.PasswordNueva, out string hash, out string salt);

            data.PasswordHash = hash;
            data.PasswordSalt = salt;

            await _context.SaveChangesAsync();
            return _mapper.Map<CambiarPasswordDTO>(data);

        }

        public async Task<DesbloqueoUsuarioDTO> UnlockUsuario(DesbloqueoUsuarioDTO usuarioDTO)
        {
            var data = _mapper.Map<UsuarioCredencial>(usuarioDTO);
            //Validar usuario
            var validarUsuario = await _context.UsuariosCredencial.FirstOrDefaultAsync(u => u.IdUsuario == usuarioDTO.IdUsuario);
            if (validarUsuario == null)
                throw new Exception(UsuarioCredencialErrors.UsuarioNoExistente);

            data.BloqueoTemporal = false;
            await _context.SaveChangesAsync();
            return _mapper.Map<DesbloqueoUsuarioDTO>(data);
        }

        public async Task<bool> Login(LoginDTO login)
        {
            var data = _mapper.Map<UsuarioCredencial>(login);

            //Validar usuario
            var selectUsuario = _context.Usuario.Where(u => u.UsuarioLogin == login.UsuarioLogin).FirstOrDefaultAsync();
            if (selectUsuario == null)
                throw new Exception(UsuarioCredencialErrors.UsuarioNoExistente);

            var validarUsuarioLogin = await _context.UsuariosCredencial.FirstOrDefaultAsync(credencial => credencial.IdUsuario == selectUsuario.Result.IdUsuario);
            if (validarUsuarioLogin.BloqueoTemporal == true)
                throw new Exception(UsuarioCredencialErrors.Bloqueo);

            var validarLogin = _passwordService.VerifyPassword(login.Password, validarUsuarioLogin.PasswordHash, validarUsuarioLogin.PasswordSalt);
            if (!validarLogin)
            {
                validarUsuarioLogin.IntentosFallidos++;
                if(validarUsuarioLogin.IntentosFallidos > 3)
                    validarUsuarioLogin.BloqueoTemporal = true;

                await _context.SaveChangesAsync();
                throw new Exception(UsuarioCredencialErrors.CredencialesIncorrectas);
            }

            return true;



        }
    }
}
