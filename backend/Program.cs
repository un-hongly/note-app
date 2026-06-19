using Microsoft.OpenApi;
using NoteAppApi.Infrastructure.DependencyInjection;
using NoteAppApi.Infrastructure.Persistence;
using NoteAppApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// global Dapper config
Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

DbUpInitializer.Run(builder.Configuration.GetConnectionString("DefaultConnection"));
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
builder.Services.AddRateLimiting(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT token"
    });

    c.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
    });
});
builder.Services.AddJwtAuth(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthData();

// DI
builder.Services.AddServices();
builder.Services.AddRepositories();

var app = builder.Build();

// // Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.MapOpenApi();
// }

app.UsePathBase("/api");
app.UseCors();
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI(c => c.ConfigObject.AdditionalItems["persistAuthorization"] = true);
app.UseMiddleware<ExceptionMiddleware>();

app.Run();