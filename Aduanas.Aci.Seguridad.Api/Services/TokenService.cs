using Aduanas.Aci.Seguridad.Api.Data;
using Aduanas.Aci.Seguridad.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Aduanas.Aci.Seguridad.Api.Services;

public interface ITokenService
{
    Task<RefreshToken> SaveRefreshTokenAsync(int idUsuario, string token, DateTime expiration, string? ip);
    Task<RefreshToken?> GetValidRefreshTokenAsync(string token);
    Task RevokeRefreshTokenAsync(string token);
}

public class TokenService : ITokenService
{
    private readonly AppDbContext _db;

    public TokenService(AppDbContext db) => _db = db;

    public async Task<RefreshToken> SaveRefreshTokenAsync(
        int idUsuario, string token, DateTime expiration, string? ip)
    {
        var refreshToken = new RefreshToken
        {
            IdUsuario = idUsuario,
            Token = token,       
            TokenHashReemplazo = null,
            ExpiresAt = expiration,  
            CreatedAt = DateTime.UtcNow, 
            IsRevoked = false,
            FechaRevocado = null
        };

        _db.RefreshToken.Add(refreshToken);
        await _db.SaveChangesAsync();
        return refreshToken;
    }

    public async Task<RefreshToken?> GetValidRefreshTokenAsync(string token)
        => await _db.RefreshToken
            .AsNoTracking()
            .FirstOrDefaultAsync(r =>
                r.Token == token &&        
                !r.IsRevoked &&           
                r.ExpiresAt > DateTime.UtcNow); 

    public async Task RevokeRefreshTokenAsync(string token)
    {
        var rt = await _db.RefreshToken
            .FirstOrDefaultAsync(r => r.Token == token);

        if (rt is not null)
        {
            rt.IsRevoked = true;            
            rt.FechaRevocado = DateTime.UtcNow; 
            await _db.SaveChangesAsync();
        }
    }
}