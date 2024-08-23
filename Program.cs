using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using static PracticalAssessmentAPI.Controllers.ContactController;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson(options => options.UseMemberCasing());

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost44318",
        builder =>
        {
            builder.WithOrigins("https://localhost:44318")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

// Configure JWT authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "https://localhost:7145",
        ValidAudience = "https://localhost:44318",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("HeinrichLambrechtsCoOPAssessment247"))
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Use CORS
app.UseCors("AllowLocalhost44318");

// Add your custom middleware
app.UseMiddleware<AuthMiddleware>();

// Use Authentication and Authorization
app.UseAuthentication(); // Ensure this comes before UseAuthorization
app.UseAuthorization();

// Configure endpoint routing
app.MapControllers();

app.Run();
