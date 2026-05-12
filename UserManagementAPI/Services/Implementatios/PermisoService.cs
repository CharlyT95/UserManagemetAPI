using Aduanas.Aci.Usuarios.Api.Audit;
using Aduanas.Aci.Usuarios.Api.Errors.Permiso;
using Aduanas.Aci.Usuarios.Api.Errors.UsuarioRol;
using Aduanas.Aci.Usuarios.Api.Extensions;
using Aduanas.Aci.Usuarios.Api.Helpers;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
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
        private readonly AuditoriaClient _auditoria;
        private readonly IHttpContextAccessor _httpContext;
        private readonly JwtHelper _jwtHelper;

        public PermisoService(UserManagementDbContext context, IMapper mapper, AuditoriaClient auditoria, IHttpContextAccessor httpContext, JwtHelper jwtHelper)
        {
            _context = context;
            _mapper = mapper;
            _auditoria = auditoria;
            _httpContext = httpContext;
            _jwtHelper = jwtHelper;
        }

        public async Task<PermisoDTO> CreatePermisoAsync(CreatePermisoDTO permiso)
        {
            var data = _mapper.Map<Permiso>(permiso);
            var validarCodigo = await _context.Permiso.AnyAsync(p => p.CodigoPermiso == permiso.CodigoPermiso && p.Activo == true);

            if (validarCodigo)
                throw new Exception(PermisoErrors.CodigoPermisoDuplicado);

            //Auditoria
            data.FechaCreacion = DateTime.Now;

            _context.Permiso.Add(data);
            await _context.SaveChangesAsync();

            _auditoria.Registrar(new AuditEvent
            {
                IdUsuario = _jwtHelper.ObtenerUsuarioId(),
                Modulo = "GestionUsuarios",
                Servicio = "Permiso",
                TipoAccion = TipoAccionEnum.Create,
                Tabla = "Permiso",
                IdRegistro = data.IdPermiso.ToString(),
                ValorNuevo = JsonSerializer.Serialize(permiso),
                DireccionIP = _httpContext.HttpContext?.Connection.RemoteIpAddress?.ToString()
            });

            return _mapper.Map<PermisoDTO>(data);
        }

        public async Task<List<PermisoDTO>> GetPermisos()
        {
            var permisos = await _context.Permiso.Where(permiso => permiso.Activo == true).OrderBy(p => p.IdPermiso).ProjectTo<PermisoDTO>(_mapper.ConfigurationProvider).ToListAsync();
            return permisos;
        }

        public async Task<PermisoDTO> GetPermisoById(int id)
        {
            var permiso = await _context.Permiso.Where(p => p.IdPermiso == id && p.Activo == true).OrderBy(p => p.IdPermiso).ProjectTo<PermisoDTO>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
            return permiso;
        }

        public async Task<PermisoDTO> UpdatePermiso(UpdatePermisoDTO permiso)
        {
            var nombreNormalizado = permiso.CodigoPermiso.NormalizarTexto();

            var data = await _context.Permiso.FirstOrDefaultAsync(p => p.IdPermiso == permiso.IdPermiso && p.Activo);
            if (data == null)
                throw new Exception("Permiso no encontrado");

            var valorAnterior = JsonSerializer.Serialize(data);

            var validarCodigo = await _context.Permiso.AnyAsync(p => p.IdPermiso != permiso.IdPermiso && p.CodigoPermiso.Trim().Replace(" ", "").ToLower() == nombreNormalizado && p.Activo);
            if (validarCodigo)
                throw new Exception(PermisoErrors.CodigoPermisoDuplicado);

            _mapper.Map(permiso, data);

            //Auditoria
            data.FechaModificacion = DateTime.Now;

            _context.Permiso.Update(data);
            await _context.SaveChangesAsync();

            _auditoria.Registrar(new AuditEvent
            {
                IdUsuario = _jwtHelper.ObtenerUsuarioId(),
                Modulo = "GestionUsuarios",
                Servicio = "Permiso",
                TipoAccion = TipoAccionEnum.Update,
                Tabla = "Permiso",
                IdRegistro = data.IdPermiso.ToString(),
                ValorAnterior = JsonSerializer.Serialize(valorAnterior),
                ValorNuevo = JsonSerializer.Serialize(permiso),
                DireccionIP = _httpContext.HttpContext?.Connection.RemoteIpAddress?.ToString()
            });

            return _mapper.Map<PermisoDTO>(data);

        }

        public async Task<bool> DeactivatePermisoAsync(int idpermiso, bool activo)
        {
            if (idpermiso <= 0)
                throw new Exception(UsuarioRolErrors.UsuarioNull);

            var data = await _context.Permiso
                .FirstOrDefaultAsync(ur => ur.IdPermiso == idpermiso);

            if (data == null)
                throw new Exception(PermisoErrors.PermisoNoEncontrado);

            if (!data.Activo && activo == false)
                throw new Exception(PermisoErrors.PermisoInactivoBoolInactivo);

            if (data.Activo && activo == true)
                throw new Exception(PermisoErrors.PermisoActivoBoolActivo);

            //Auditoria
            data.Activo = activo;

            _auditoria.Registrar(new AuditEvent
            {
                IdUsuario = _jwtHelper.ObtenerUsuarioId(),
                Modulo = "GestionUsuarios",
                Servicio = "Permiso",
                TipoAccion = TipoAccionEnum.Update,
                Tabla = "Permiso",
                IdRegistro = data.IdPermiso.ToString(),
                ValorAnterior = JsonSerializer.Serialize(true),
                ValorNuevo = JsonSerializer.Serialize(false),
                DireccionIP = _httpContext.HttpContext?.Connection.RemoteIpAddress?.ToString()
            });

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
