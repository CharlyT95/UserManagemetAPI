using Aduanas.Aci.Seguridad.Api.Data;
using Aduanas.Aci.Seguridad.Api.DTOs.Auth;
using Aduanas.Aci.Seguridad.Api.DTOs.Rol;
using Aduanas.Aci.Seguridad.Api.DTOs.Usuario;
using Aduanas.Aci.Seguridad.Api.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Aduanas.Aci.Seguridad.Api.Services;

// ─────────────────────────────────────────────────────────────────────────────
// Interface
// ─────────────────────────────────────────────────────────────────────────────
public interface IAuthService
{
    Task<LoginResponseDTO?> LoginAsync(LoginRequestDTO request, string? ip);
    Task<LoginResponseDTO?> RefreshTokenAsync(RefreshTokenRequestDTO request);
    Task LogoutAsync(string refreshToken);
}

// ─────────────────────────────────────────────────────────────────────────────
// Implementation
// ─────────────────────────────────────────────────────────────────────────────
public class AuthService : IAuthService
{
    private readonly AppDbContext _db;
    private readonly JwtHelper _jwt;
    private readonly ITokenService _tokenService;

    public AuthService(AppDbContext db, JwtHelper jwt, ITokenService tokenService)
    {
        _db = db;
        _jwt = jwt;
        _tokenService = tokenService;
    }

    // ─────────────────────────────────────────────────────────────────────────
    // LOGIN
    // ─────────────────────────────────────────────────────────────────────────
    public async Task<LoginResponseDTO?> LoginAsync(LoginRequestDTO request, string? ip)
    {
        // 1. Buscar usuario activo por UsuarioLogin
        var usuario = await _db.Usuarios
            .FirstOrDefaultAsync(u => u.UsuarioLogin == request.NombreUsuario);

        if (usuario is null)
            return null;

        // 2. Obtener credencial del usuario
        var credencial = await _db.UsuarioCredenciales
            .FirstOrDefaultAsync(c => c.IdUsuario == usuario.IdUsuario);

        if (credencial is null)
            return null;

        // 3. Verificar si la cuenta está bloqueada
        if (credencial.BloqueoTemporal)
            return null;

        // 4. Verificar contraseña usando PBKDF2
        var passwordValida = PasswordHelper.VerificarPassword(
            password: request.Password,
            storedHash: credencial.PasswordHash,
            storedSalt: credencial.PasswordSalt,
            iteraciones: credencial.Iteraciones);

        if (!passwordValida)
        {
            // Incrementar intentos fallidos y bloquear si llega a 5
            credencial.IntentosFallidos++;

            if (credencial.IntentosFallidos >= 5)
                credencial.BloqueoTemporal = true;

            await _db.SaveChangesAsync();
            return null;
        }

        // 5. Resetear intentos fallidos al ingresar correctamente
        credencial.IntentosFallidos = 0;
        await _db.SaveChangesAsync();

        // 6. Obtener roles asignados al usuario
        var roles = await _db.UsuarioRoles
            .Include(ur => ur.Rol)
            .Where(ur => ur.IdUsuario == usuario.IdUsuario)
            .Select(ur => ur.Rol)
            .ToListAsync();

        // 7. Obtener permisos a través de los roles (sin duplicados)
        var idRoles = roles.Select(r => r!.IdRol).ToList();
        var permisos = await _db.RolPermisos
            .Include(rp => rp.Permiso)
            .Where(rp => idRoles.Contains(rp.IdRol))
            .Select(rp => rp.Permiso.CodigoPermiso)
            .Distinct()
            .ToListAsync();

        // 8. Generar AccessToken y RefreshToken
        var (accessToken, accessExp) = _jwt.GenerateAccessToken(
            idUsuario: usuario.IdUsuario,
            usuarioLogin: usuario.UsuarioLogin,
            roles: roles.Select(r => r!.Nombre),
            permisos: permisos);

        var (refreshToken, refreshExp) = _jwt.GenerateRefreshToken();

        // 9. Persistir el RefreshToken en base de datos
        await _tokenService.SaveRefreshTokenAsync(
            idUsuario: usuario.IdUsuario,
            token: refreshToken,
            expiration: refreshExp,
            ip: ip);

        // 10. Mapear Models → DTOs y retornar respuesta
        return new LoginResponseDTO
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            AccessTokenExpiration = accessExp,
            RefreshTokenExpiration = refreshExp,
            Usuario = MapToUsuarioInfoDTO(usuario, roles!, permisos)
        };
    }

    // ─────────────────────────────────────────────────────────────────────────
    // REFRESH TOKEN
    // ─────────────────────────────────────────────────────────────────────────
    public async Task<LoginResponseDTO?> RefreshTokenAsync(RefreshTokenRequestDTO request)
    {
        // 1. Validar el AccessToken aunque esté expirado
        var principal = _jwt.GetPrincipalFromExpiredToken(request.AccessToken);
        if (principal is null)
            return null;

        // 2. Buscar y validar el RefreshToken en BD (no revocado, no expirado)
        var storedToken = await _tokenService.GetValidRefreshTokenAsync(request.RefreshToken);
        if (storedToken is null)
            return null;

        // 3. Revocar el RefreshToken usado (rotación de tokens)
        await _tokenService.RevokeRefreshTokenAsync(request.RefreshToken);

        // 4. Obtener el usuario asociado al RefreshToken
        var usuario = await _db.Usuarios
            .FirstOrDefaultAsync(u => u.IdUsuario == storedToken.IdUsuario);

        if (usuario is null)
            return null;

        // 5. Recargar roles y permisos actualizados
        var roles = await _db.UsuarioRoles
            .Include(ur => ur.Rol)
            .Where(ur => ur.IdUsuario == usuario.IdUsuario)
            .Select(ur => ur.Rol)
            .ToListAsync();

        var idRoles = roles.Select(r => r!.IdRol).ToList();
        var permisos = await _db.RolPermisos
            .Include(rp => rp.Permiso)
            .Where(rp => idRoles.Contains(rp.IdRol))
            .Select(rp => rp.Permiso.CodigoPermiso)
            .Distinct()
            .ToListAsync();

        // 6. Generar nuevos tokens
        var (accessToken, accessExp) = _jwt.GenerateAccessToken(
            idUsuario: usuario.IdUsuario,
            usuarioLogin: usuario.UsuarioLogin,
            roles: roles.Select(r => r!.Nombre),
            permisos: permisos);

        var (refreshToken, refreshExp) = _jwt.GenerateRefreshToken();

        // 7. Guardar nuevo RefreshToken (el anterior ya fue revocado)
        await _tokenService.SaveRefreshTokenAsync(
            idUsuario: usuario.IdUsuario,
            token: refreshToken,
            expiration: refreshExp,
            ip: null);

        // 8. Mapear y retornar
        return new LoginResponseDTO
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            AccessTokenExpiration = accessExp,
            RefreshTokenExpiration = refreshExp,
            Usuario = MapToUsuarioInfoDTO(usuario, roles!, permisos)
        };
    }

    // ─────────────────────────────────────────────────────────────────────────
    // LOGOUT
    // ─────────────────────────────────────────────────────────────────────────
    public async Task LogoutAsync(string refreshToken)
        => await _tokenService.RevokeRefreshTokenAsync(refreshToken);

    // ─────────────────────────────────────────────────────────────────────────
    // MAPEO PRIVADO: Models → DTOs
    // ─────────────────────────────────────────────────────────────────────────
    private static UsuarioDTO MapToUsuarioInfoDTO(
        Usuario usuario,
        List<Rol> roles,
        List<string> permisos)
    {
        return new UsuarioDTO
        {
            IdUsuario = usuario.IdUsuario,
            UsuarioLogin = usuario.UsuarioLogin,
            Nombres = usuario.Nombres,
            Apellidos = usuario.Apellidos,
            CorreoElectronico = usuario.CorreoElectronico,

            // Model Rol → RolInfoDTO (nunca expone la entidad directamente)
            Roles = roles.Select(r => new RolDTO
            {
                IdRol = r.IdRol,
                Nombre = r.Nombre,
                Descripcion = r.Descripcion
            }).ToList(),

            Permisos = permisos
        };
    }
}