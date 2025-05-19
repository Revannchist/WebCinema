using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using WebCinema.Interfaces;
using WebCinema.Services;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using Microsoft.OpenApi.Models;

namespace WebCinema
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Register application services
            builder.Services.AddScoped<ICountriesService, CountriesService>();
            builder.Services.AddScoped<IGenresService, GenresService>();
            builder.Services.AddScoped<IUsersService, UsersService>();
            builder.Services.AddScoped<IDirectorsService, DirectorsService>();
            builder.Services.AddScoped<IActorsService, ActorsService>();
            builder.Services.AddScoped<ICitiesService, CitiesService>();
            builder.Services.AddScoped<ITheatersService, TheatersService>();
            builder.Services.AddScoped<IHallsService, HallsService>();
            builder.Services.AddScoped<ISeatsService, SeatsService>();
            builder.Services.AddScoped<IMoviesService, MoviesService>();
            builder.Services.AddScoped<IShowTimesService, ShowTimesService>();
            builder.Services.AddScoped<IRatingsService, RatingsService>();
            builder.Services.AddScoped<IBookingsService, BookingsService>();
            builder.Services.AddScoped<IPaymentsService, PaymentsService>();
            builder.Services.AddScoped<IMoviesImageService, MoviePosterService>();
            builder.Services.AddScoped<IUsersImageService, UsersImageService>();
            builder.Services.AddScoped<IRolesService, RoleService>();

            //JWT Token za provjeru kad je admin prijavljen
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebCinema API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
            });

            //Controllers
            builder.Services.AddControllers();

            //Swagger support
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Configure database connection
            builder.Services.AddDbContext<WebCinemaDBContext>(o =>
                o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
            );

            //CORS policy for Angular frontend
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AngularPolicy",
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:4200", "https://localhost:4200")//Allow both HTTP & HTTPS
                              .AllowAnyMethod()
                              .AllowAnyHeader()
                              .AllowCredentials(); // Required for cookies, authentication headers, etc.
                    });
            });

            //WT Authentication Configuration
            var jwtSettings = builder.Configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));

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
                        IssuerSigningKey = key,
                        ClockSkew = TimeSpan.Zero // Removes default 5-minute clock skew

                        //,
                        //RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                        //NameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"

                        ,
                        RoleClaimType = ClaimTypes.Role,
                        NameClaimType = ClaimTypes.Name
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = context =>
                        {
                            Console.WriteLine("Token validated successfully");
                            var userClaims = context.Principal.Claims;
                            foreach (var claim in userClaims)
                            {
                                Console.WriteLine($"Claim: {claim.Type} = {claim.Value}");
                            }
                            return Task.CompletedTask;
                        },
                        OnChallenge = context =>
                        {
                            Console.WriteLine("Challenge issued. Authentication requirement not met.");
                            return Task.CompletedTask;
                        }
                    };
                });



            var app = builder.Build();

            //CORS before authentication & authorization
            app.UseCors("AngularPolicy");

            //Swagger
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            //Authentication & Authorization after CORS
            app.UseAuthentication();
            app.UseAuthorization();

            //Map API controllers
            app.MapControllers();

            app.Run();
        }
    }
}
