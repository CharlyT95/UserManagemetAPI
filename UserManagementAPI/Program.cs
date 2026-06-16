using Aduanas.Aci.Usuarios.Api.Services.Implementatios;
using Aduanas.Aci.Usuarios.Api.Services.Interfaces;
using Aduanas.Aci.Usuarios.Api.Audit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Channels;
using UserManagementAPI.Data;
using UserManagementAPI.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Aduanas.Aci.Usuarios.Api.Helpers;

var builder = WebApplication.CreateBuilder(args);


var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                                           Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!)),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirAngular", builder =>
    {
        builder.WithOrigins("http://localhost:4200") // Permite tu Angular local
               .AllowAnyMethod()                     // Permite GET, POST, PUT, DELETE
               .AllowAnyHeader();                    // Permite cualquier tipo de header
    });
});


builder.Services.AddControllers();
builder.Services.AddDbContext<UserManagementDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<PermisoService>();
builder.Services.AddScoped<RolService>();
builder.Services.AddScoped<UsuarioRolService>();
builder.Services.AddScoped<RolPermisoService>();
builder.Services.AddScoped<UsuarioCredencialService>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<JwtHelper>();
builder.Services.AddScoped<RegistroAuditoria>();



builder.Services.AddSingleton(
    Channel.CreateBounded<AuditEvent>(new BoundedChannelOptions(5000)
    {
        FullMode = BoundedChannelFullMode.DropOldest,
        SingleReader = true,
        SingleWriter = false
    })
);
//AUDITORÍA
builder.Services.AddHttpClient("AuditoriaClient", client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["ServiciosExternos:SeguridadApi"]!);
    client.Timeout = TimeSpan.FromSeconds(30);
});

builder.Services.AddSingleton<AuditoriaClient>();
builder.Services.AddHostedService<AuditDispatcherWorker>();
// ----------------------------------------------------------------

builder.Services.AddHttpContextAccessor();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errores = context.ModelState
            .Where(x => x.Value.Errors.Count > 0)
            .SelectMany(x => x.Value.Errors)
            .Select(x => x.ErrorMessage)
            .ToList();
        var response = new
        {
            success = false,
            message = string.Join(", ", errores),
            data = (object)null
        };
        return new BadRequestObjectResult(response);
    };
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();
//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("PermitirAngular");
app.MapControllers();
app.Run();