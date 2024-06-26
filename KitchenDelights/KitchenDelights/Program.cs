using Data.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;
using Business.Interfaces;
using Business.Managers;
using Data.Interfaces;
using Data.Repositories;

internal class Program
{
    private static void Main(string[] args)
    {
        string _folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/");
        Directory.CreateDirectory(_folderPath);

        var builder = WebApplication.CreateBuilder(args);

        var myCORSPolicy = "_myCORSPolicy";
        builder.Services.AddCors(options => options.AddPolicy(
            name: myCORSPolicy,
            policy =>
            {
                policy.AllowAnyOrigin();
                policy.AllowAnyMethod();
                policy.AllowAnyHeader();
            }
            ));


        // Add services to the container.
        builder.Services.AddControllers().AddNewtonsoftJson(
            options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "KitchenDelightsAPI", Version = "v1" });
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
                     });
        });

        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        builder.Services.AddScoped<IUserManager, UserManager>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();

        builder.Services.AddScoped<ICategoryManager, CategoryManager>();
        builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

        builder.Services.AddScoped<INewsManager, NewsManager>();
        builder.Services.AddScoped<INewsRepository,  NewsRepository>();

        builder.Services.AddScoped<IBlogManager, BlogManager>();
        builder.Services.AddScoped<IBlogRepository, BlogRepository>();

        builder.Services.AddScoped<IRecipeManager, RecipeManager>();
        builder.Services.AddScoped<IRecipeRepository, RecipeRepository>();

        builder.Services.AddScoped<IBookmarkManager, BookmarkManager>();

        builder.Services.AddScoped<ICommentManager, CommentManager>();
        builder.Services.AddScoped<ICommentRepository, CommentRepository>();

        builder.Services.AddScoped<IRatingManager, RatingManager>();
        builder.Services.AddScoped<IRatingRepository, RatingRepository>();

        builder.Services.AddScoped<ICartManager, CartManager>();
        builder.Services.AddScoped<ICartRepository,  CartRepository>();

        builder.Services.AddScoped<IHistoryManager, HistoryManager>();
        builder.Services.AddScoped<IHistoryRepository, HistoryRepository>();

        builder.Services.AddScoped<IVoucherRepository, VoucherRepository>();

        builder.Services.AddScoped<IMenuManager, MenuManager>();
        builder.Services.AddScoped<IMenuRepository, MenuRepository>();

        builder.Services.AddScoped<IIngredientManager, IngredientManager>();
        builder.Services.AddScoped<IIngredientRepository, IngredientRepository>();

        builder.Services.AddScoped<ICountryManager, CountryManager>();
        builder.Services.AddScoped<ICountryRepository, CountryRepository>();

        builder.Services.AddScoped<IVerificationManager, VerificationManager>();
        builder.Services.AddScoped<IVerificationRepository, VerificationRepository>();

        builder.Services.AddScoped<IVoucherManager, VoucherManager>();
        builder.Services.AddScoped<IVoucherRepository, VoucherRepository>();

        builder.Services.AddScoped<IAdvertisementManager, AdvertisementManager>();
        builder.Services.AddScoped<IAdvertisementRepository, AdvertisementRepository>();

        builder.Services.AddScoped<IRoleManager, RoleManager>();
        builder.Services.AddScoped<IRoleRepository, RoleRepository>();

        builder.Services.AddScoped<IRecipeIngredientRepository, RecipeIngredientRepository>();

        builder.Services.AddScoped<IMarketplaceManager, MarketplaceManager>();
        builder.Services.AddScoped<IMarketplaceRepository, MarketplaceRepository>();

        builder.Services.AddScoped<IIngredientMarketplaceManager, IngredientMarketplaceManager>();
        builder.Services.AddScoped<IIngredientMarketplaceRepository, IngredientMarketplaceRepository>();

        builder.Services.AddDbContext<KitchenDelightsContext>(
                    option => option.UseSqlServer(
                        builder.Configuration.GetConnectionString("KitchenDelights")
                        )
                    );

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
                    .AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.SaveToken = true;
                        options.TokenValidationParameters = new TokenValidationParameters()
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidAudience = builder.Configuration["Jwt:Audience"],
                            ValidIssuer = builder.Configuration["Jwt:Issuer"],
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                            RoleClaimType = ClaimTypes.Role,
                        };
                    });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();

        }


        app.UseStaticFiles();

        app.UseCors(myCORSPolicy);

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}