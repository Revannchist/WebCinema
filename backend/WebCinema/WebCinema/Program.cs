
using Microsoft.EntityFrameworkCore;
using WebCinema.Interfaces;
using WebCinema.Services;

namespace WebCinema
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddScoped<ICountriesService,CountriesService>(); //za svaki par interface-service
            builder.Services.AddScoped<IGenresService, GenresService>();
            builder.Services.AddScoped<IUsersService, UsersService>();
            builder.Services.AddScoped<IDirectorsService,DirectorsService>();
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
            builder.Services.AddScoped<IRolesService,RoleService>();
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<WebCinemaDBContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddCors(options => options.AddPolicy(name: "AngularPolicy",
            policy =>
            {
                 policy.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
            }));    
            var app = builder.Build();



            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("AngularPolicy");

            app.UseAuthorization();

            app.UseRouting();

            app.MapControllers();

            app.Run();
        }
    }
}
