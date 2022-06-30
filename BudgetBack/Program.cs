using System.Text;
using BudgetBack;
using BudgetBack.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Swagger
builder.Services.AddSwaggerGen(c =>
{
      c.SwaggerDoc("v1", new OpenApiInfo { Title = "Portal do Cliente", Version = "v1"});
      c.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
      {
            Description = "JWT Autorization header using the berarer scheme",
            Name = "Autorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT"
      });
      c.AddSecurityRequirement(new OpenApiSecurityRequirement
      {
            {
                  new OpenApiSecurityScheme
                  {
                        Reference = new OpenApiReference{ Type = ReferenceType.SecurityScheme, Id = "bearer"}
                  },
                  new List<string>()
            }
      });
});

// Entityframework
var connectionString = builder.Configuration.GetConnectionString("sqlServerConnection");
builder.Services.AddDbContext<Context>(options => { options.UseSqlServer(connectionString); });
builder.Services.AddScoped<Context, Context>();

// JsonResult
builder.Services.AddMvc().AddNewtonsoftJson(opt =>
{
      opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
      opt.SerializerSettings.ContractResolver = new DefaultContractResolver();
});

// Autenticação
var key = Encoding.ASCII.GetBytes(Settings.Secret);

builder.Services.AddAuthentication(x =>
{
      x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
      x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
      x.RequireHttpsMetadata = false;
      x.SaveToken = true;
      x.TokenValidationParameters = new TokenValidationParameters
      {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
      };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
      app.UseDeveloperExceptionPage();
}
else
{
      app.UseExceptionHandler("/Home/Error");
      app.UseHsts();
}

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
      app.UseDeveloperExceptionPage();
}
else
{
      app.UseExceptionHandler("/Home/Error");
      app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
      app.UseDeveloperExceptionPage();
}
else
{
      app.UseExceptionHandler("/Home/Error");
      app.UseHsts();
}

app.MapControllers();

app.Run();