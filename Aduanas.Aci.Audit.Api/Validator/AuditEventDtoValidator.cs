using Aduanas.Aci.Audit.Api.DTOs;
using FluentValidation;

namespace Aduanas.Aci.Audit.Api.Validator
{
    public class AuditEventDtoValidator : AbstractValidator<AuditEventDto>
    {
        private static readonly string[] _accionesValidas =
            { "CREACIÓN", "LECTURA", "MODIFICACIÓN", "ELIMINACIÓN", "INICIO DE SESIÓN", "CIERRE DE SESIÓN", "ERROR" };

        public AuditEventDtoValidator()
        {
            RuleFor(x => x.IdUsuario)
                .GreaterThan(0)
                .WithMessage("El Identificador de usuario debe de ser mayor a 0");

            RuleFor(x => x.Modulo)
                .NotEmpty()
                .WithMessage("El campo Modulo es requerido.")
                .MaximumLength(50)
                .WithMessage("Modulo no puede superar 50 caracteres.");

            RuleFor(x => x.Servicio)
                .NotEmpty()
                .WithMessage("El campo Servicio es requerido.")
                .MaximumLength(100)
                .WithMessage("Servicio no puede superar 100 caracteres.");

            RuleFor(x => x.TipoAccion)
                .NotEmpty()
                .WithMessage("El campo TipoAccion es requerido.")
                .Must(a => _accionesValidas.Contains(a.ToUpper()))
                .WithMessage($"TipoAccion debe ser uno de: {string.Join(", ", _accionesValidas)}.");

            RuleFor(x => x.Tabla)
                .NotEmpty()
                .WithMessage("El campo Tabla es requerido.")
                .MaximumLength(100)
                .WithMessage("Tabla no puede superar 100 caracteres.");

            RuleFor(x => x.IdRegistro)
                .NotEmpty()
                .WithMessage("El campo IdRegistro es requerido.")
                .MaximumLength(50)
                .WithMessage("RegistroId no puede superar 50 caracteres.");

            RuleFor(x => x.Peticion)
                .MaximumLength(2000)
                .WithMessage("Peticion no puede superar 2000 caracteres.")
                .When(x => x.Peticion is not null);

            RuleFor(x => x.Respuesta)
                .MaximumLength(2000)
                .WithMessage("Respuesta no puede superar 2000 caracteres.")
                .When(x => x.Respuesta is not null);

            RuleFor(x => x.Referencia)
                .MaximumLength(500)
                .WithMessage("Referencia no puede superar 500 caracteres.")
                .When(x => x.Referencia is not null);

            RuleFor(x => x.DireccionIP)
                .MaximumLength(45)
                .WithMessage("DireccionIP no puede superar 45 caracteres.")
                .When(x => x.DireccionIP is not null);
        }
    }
}
