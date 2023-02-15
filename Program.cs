using AspNetCoreRateLimit;
using dsf_api_template_net6.Filters;
using dsf_api_template_net6.Middleware;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.Reflection;

using Microsoft.EntityFrameworkCore;
using dsf_api_template_net6.Models;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

//Use ToDoItems In Memory Database Context
builder.Services.AddDbContext<TodoContext>(opt =>
    opt?.UseInMemoryDatabase("TodoListDb"));

//Use Contact Info In Memory Database Context
builder.Services.AddDbContext<ContactInfoContext>(opt =>
    opt?.UseInMemoryDatabase("ContactInfoDb"));


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "DSF API Template",
        Version = "v1",
        Description = ".NET Restful API template to be used by dev teams.",
        Contact = new OpenApiContact
        {
            Name = "DSF Tech Team",
            Email = "dsf-tech@dits.dmrid.gov.cy",
            Url = new Uri("https://dsf.dmrid.gov.cy/")
        },
    });
    //todo: enable annotations
    c.OperationFilter<AddHeaderParameterOperationFilter>();
    //c.OperationFilter<SecurityRequirementsOperationFilter>();
    //c.SchemaFilter<EnumSchemaFilter>();

    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
});

builder.Services
    .AddAuthentication("token")          
    .AddJwtBearer("token", options =>
    {
        // base-address of Identity Server
        options.Authority = builder.Configuration["IdentityServer:Authority"];
        options.TokenValidationParameters.ValidateAudience = false;

        options.TokenValidationParameters.ValidTypes = new[] { "at+jwt", "JWT" }; // currently CYLogin token type is JWT                    

        if (Boolean.Parse(builder.Configuration["Proxy:ProxyEnabled"]))
        {
            options.BackchannelHttpHandler = new HttpClientHandler
            {
                Proxy = new WebProxy(builder.Configuration["Proxy:ProxyAddress"])
            };
        }
    });

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Add versioning control
builder.Services.AddApiVersioning(config =>
{
    config.DefaultApiVersion = new ApiVersion(1, 0);
    config.AssumeDefaultVersionWhenUnspecified = true;
    config.ReportApiVersions = true;
});

builder.Services.AddSingleton<IApiKeyRepository, ApiKeyRepository>();

//Rate Limiting Section
//Rate Limit Configuration

// needed to load configuration from appsettings.json
builder.Services.AddOptions();
// needed to store rate limit counters and ip rules
builder.Services.AddMemoryCache();
//load general configuration from appsettings.json
builder.Services.Configure<ClientRateLimitOptions>(builder.Configuration.GetSection("ClientRateLimiting"));
//load client rules from appsettings.json
builder.Services.Configure<ClientRateLimitPolicies>(builder.Configuration.GetSection("ClientRateLimitPolicies"));

// inject counter and rules stores
builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
builder.Services.AddSingleton<IClientPolicyStore, MemoryCacheClientPolicyStore>();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
//

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//add Production Environment
else if (app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ApiKeyValidators>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
