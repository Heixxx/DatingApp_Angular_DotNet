using System.Text;
using Api.Data;
using API.Data;
using API.Helpers;
using API.Interfaces;
using API.Middleware;
using API.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<DataContext>(opt => {
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddCors();
//builder.Services.AddTransient                      - Usługa z bardzo krótkim czasem życia.
//builder.Services.AddSingleton()                   - Usługa inicjowana przy 1 uruchomieniu i kasowana dopiero przy zamknięciu aplikacji.
builder.Services.AddScoped<ITokenServices, TokenService>();       //     -Usługa z dłuższą żywotnością.
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));              //Pobniera konfiguracje z innego pliku.
builder.Services.AddScoped<IPhotoService, PhotoService>();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)    //Autoryzacja za pomocą tokenu
    .AddJwtBearer(options =>{
        options.TokenValidationParameters = new TokenValidationParameters{     //Zasady jakie serwer powinien walidowac, ze ten token to odpowiedni token.
            ValidateIssuerSigningKey = true,                                  //Sprawdza, czy token zostal podpisany,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenKey"])),
            ValidateIssuer = false,                                                          // informacje zwrotne z naszym API serwera. 
            ValidateAudience = false,                           //Nie przekazalismy z naszym tokenem wiec wylaczamy.

        };
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle   --
// builder.Services.AddEndpointsApiExplorer();  --
// builder.Services.AddSwaggerGen();                          --ALSO SWAGGER

var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();             // - Uzywamy naszej kklasy posredniczacej do obsługi błędów.
// if (builder.Environment.IsDevelopment()){             - Pozwala na ukazanie błędów w odpowiednim trybie
//     app.UseDeveloperExceptionPage();
// }

// Configure the HTTP request pipeline.

// if (app.Environment.IsDevelopment())             -SWAGGER
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200", "https://localhost:5001"));
//Miedzy UseCors, a MapControllers uzywamy Middleware (.. .AddAuthentication)
app.UseAuthentication();                  //-Musi byc 'pierwsze'
app.UseAuthorization();                    //'Pyta', czy masz wazny token
app.MapControllers();                       // Jesli masz wazny token pozwala ci robic cos, co mozesz

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try{
    var context = services.GetRequiredService<DataContext>();
    await context.Database.MigrateAsync();                     //Tworzy nową bazę danych z danymi jeśli bazy danych nie ma.
    await Seed.SeedUsers(context);
}
catch(Exception ex){
    var logger = services.GetService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred during migration");
}


app.Run();

