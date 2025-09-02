using conagra_inventory_management_engine.Repositories;
using conagra_inventory_management_engine.Services;
using Supabase;
using DotNetEnv;

namespace conagra_inventory_management_engine;

public class Program
{
    public static void Main(string[] args)
    {
        // Load environment variables from env.local file
        Env.Load("env.local");

        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Configure Supabase client
        var supabaseUrl = Environment.GetEnvironmentVariable("SUPABASE_URL") ?? builder.Configuration["Supabase:Url"];
        var supabaseKey = Environment.GetEnvironmentVariable("SUPABASE_KEY") ?? builder.Configuration["Supabase:Key"];
        
        Console.WriteLine("------------------");
        Console.WriteLine(supabaseUrl);
        Console.WriteLine(supabaseKey);
        
        if (string.IsNullOrEmpty(supabaseUrl) || string.IsNullOrEmpty(supabaseKey))
        {
            throw new InvalidOperationException("Supabase URL and Key must be configured via environment variables SUPABASE_URL and SUPABASE_KEY");
        }

        builder.Services.AddSingleton<Supabase.Client>(provider => new Supabase.Client(supabaseUrl, supabaseKey));

        // Register repositories
        builder.Services.AddScoped<IProductRepository, ProductRepository>();
        builder.Services.AddScoped<IStoreRepository, StoreRepository>();
        builder.Services.AddScoped<IWarehouseRepository, WarehouseRepository>();
        builder.Services.AddScoped<IStoreInventoryRepository, StoreInventoryRepository>();
        builder.Services.AddScoped<IInventoryThresholdRepository, InventoryThresholdRepository>();

        // Register services
        builder.Services.AddScoped<IStoresService, StoresService>();
        builder.Services.AddScoped<IProductsService, ProductsService>();
        builder.Services.AddScoped<IWarehouseService, WarehouseService>();
        builder.Services.AddScoped<IStoreInventoryService, StoreInventoryService>();
        builder.Services.AddScoped<IInventoryThresholdsService, InventoryThresholdsService>();

        // Add CORS configuration
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAngularApp", policy =>
            {
                policy.WithOrigins("http://localhost:4200", "https://your-app-name.netlify.app")
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseCors("AllowAngularApp");

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}