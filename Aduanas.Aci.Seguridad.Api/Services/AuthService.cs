using Aduanas.Aci.Seguridad.Api.Data;
using Aduanas.Aci.Seguridad.Api.DTOs.Auth;
using Aduanas.Aci.Seguridad.Api.DTOs.Permis;
using Aduanas.Aci.Seguridad.Api.DTOs.Rol;
using Aduanas.Aci.Seguridad.Api.DTOs.Usuario;
using Aduanas.Aci.Seguridad.Api.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Aduanas.Aci.Seguridad.Api.Services;

public interface IAuthService
{
    Task<LoginResponseDTO?> LoginAsync(LoginRequestDTO request, string? ip);
    Task<LoginResponseDTO?> RefreshTokenAsync(RefreshTokenRequestDTO request);
    Task LogoutAsync(string refreshToken);
}

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

 
    public async Task<LoginResponseDTO?> LoginAsync(LoginRequestDTO request, string? ip)
    {
        //Usuario activo
        var usuario = await _db.Usuario
            .FirstOrDefaultAsync(u => u.UsuarioLogin == request.NombreUsuario && u.Activo);

        if (usuario is null)
            return null;

        //Credenciales
        var credencial = await _db.UsuarioCredencial
            .FirstOrDefaultAsync(c => c.IdUsuario == usuario.IdUsuario);

        if (credencial is null || credencial.BloqueoTemporal)
            return null;

        //Validar password
        var passwordValida = PasswordHelper.VerificarPassword(
            password: request.Password,
            storedHash: credencial.PasswordHash,
            storedSalt: credencial.PasswordSalt,
            iteraciones: credencial.Iteraciones);

        if (!passwordValida)
        {
            credencial.IntentosFallidos++;

            if (credencial.IntentosFallidos >= 5)
                credencial.BloqueoTemporal = true;

            await _db.SaveChangesAsync();
            return null;
        }

        //Reset intentos
        if (credencial.IntentosFallidos > 0)
        {
            credencial.IntentosFallidos = 0;
            await _db.SaveChangesAsync();
        }

        //Roles + Permisos
        var roles = await GetRolesConPermisosAsync(usuario.IdUsuario);

        //JWT
        var (accessToken, accessExp) = _jwt.GenerateAccessToken(
            idUsuario: usuario.IdUsuario,
            usuarioLogin: usuario.UsuarioLogin,
            roles: roles.Select(r => r.Nombre),
            permisos: roles.SelectMany(r => r.Permisos)
                           .Select(p => p.CodigoPermiso)
                           .Distinct()
        );

        var (refreshToken, refreshExp) = _jwt.GenerateRefreshToken();

        // Guardar refresh token
        await _tokenService.SaveRefreshTokenAsync(
            idUsuario: usuario.IdUsuario,
            token: refreshToken,
            expiration: refreshExp,
            ip: ip
        );

        //Response
        return new LoginResponseDTO
        {
            AccessToken = accessToken,
            AccessTokenExpiration = accessExp,
            RefreshToken = refreshToken,
            RefreshTokenExpiration = refreshExp,

            Usuario = new UsuarioDTO
            {
                IdUsuario = usuario.IdUsuario,
                UsuarioLogin = usuario.UsuarioLogin,
                Nombres = usuario.Nombres,
                Apellidos = usuario.Apellidos,
                CorreoElectronico = usuario.CorreoElectronico,
                Roles = roles
            }
        };
    }


    public async Task<LoginResponseDTO?> RefreshTokenAsync(RefreshTokenRequestDTO request)
    {
        // 1. Validar access token (aunque esté expirado)
        var principal = _jwt.GetPrincipalFromExpiredToken(request.AccessToken);
        if (principal is null)
            return null;

        // 2. Validar refresh token en BD
        var storedToken = await _tokenService.GetValidRefreshTokenAsync(request.RefreshToken);
        if (storedToken is null)
            return null;

        // 🔒 3. Validación extra (RECOMENDADO)
        var userIdFromToken = principal.FindFirst("sub")?.Value;

        if (userIdFromToken is null ||
            userIdFromToken != storedToken.IdUsuario.ToString())
            return null;

        // 4. Revocar refresh token actual (rotación)
        await _tokenService.RevokeRefreshTokenAsync(request.RefreshToken);

        // 5. Obtener usuario activo
        var usuario = await _db.Usuario
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.IdUsuario == storedToken.IdUsuario && u.Activo);

        if (usuario is null)
            return null;

        // 6. Obtener roles + permisos (ACTIVOS)
        var roles = await GetRolesConPermisosAsync(usuario.IdUsuario);

        // 7. Generar nuevo access token
        var (accessToken, accessExp) = _jwt.GenerateAccessToken(
            idUsuario: usuario.IdUsuario,
            usuarioLogin: usuario.UsuarioLogin,
            roles: roles.Select(r => r.Nombre),
            permisos: roles.SelectMany(r => r.Permisos)
                           .Select(p => p.CodigoPermiso)
                           .Distinct()
        );

        // 8. Generar nuevo refresh token
        var (refreshToken, refreshExp) = _jwt.GenerateRefreshToken();

        // 9. Guardar nuevo refresh token
        await _tokenService.SaveRefreshTokenAsync(
            idUsuario: usuario.IdUsuario,
            token: refreshToken,
            expiration: refreshExp,
            ip: null
        );

        // 10. Response (igual que login)
        return new LoginResponseDTO
        {
            AccessToken = accessToken,
            AccessTokenExpiration = accessExp,
            RefreshToken = refreshToken,
            RefreshTokenExpiration = refreshExp,

            Usuario = new UsuarioDTO
            {
                IdUsuario = usuario.IdUsuario,
                UsuarioLogin = usuario.UsuarioLogin,
                Nombres = usuario.Nombres,
                Apellidos = usuario.Apellidos,
                CorreoElectronico = usuario.CorreoElectronico,
                Roles = roles
            }
        };
    }


    public async Task LogoutAsync(string refreshToken)
    {
        await _tokenService.RevokeRefreshTokenAsync(refreshToken);
    }


    private async Task<List<RolDTO>> GetRolesConPermisosAsync(int idUsuario)
    {
        return await _db.UsuarioRol
            .AsNoTracking()
            .Where(ur => ur.IdUsuario == idUsuario
                      && ur.Activo
                      && ur.Rol.Activo)
            .Select(ur => new RolDTO
            {
                IdRol = ur.Rol.IdRol,
                Nombre = ur.Rol.Nombre,
                Descripcion = ur.Rol.Descripcion,

                Permisos = _db.RolPermiso
                    .Where(rp => rp.IdRol == ur.IdRol
                              && rp.Activo
                              && rp.Rol.Activo
                              && rp.Permiso.Activo)
                    .Select(rp => new PermisoDTO
                    {
                        IdPermiso = rp.Permiso.IdPermiso,
                        CodigoPermiso = rp.Permiso.CodigoPermiso,
                        Descripcion = rp.Permiso.Descripcion,
                        Modulo = rp.Permiso.Modulo,
                        Accion = rp.Permiso.Accion,
                        Referencia = rp.Permiso.Referencia
                    }).ToList()
            })
            .ToListAsync();
    }
}