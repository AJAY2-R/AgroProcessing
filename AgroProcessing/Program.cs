using AgroProcessing.Data;
using AgroProcessing.Services;
using AgroProcessing.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure API controllers with JSON options
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "AgroProcessing API",
        Version = "v1",
        Description = "Complete API for Agricultural Processing Management System",
        Contact = new OpenApiContact
        {
            Name = "AgroProcessing Team",
            Email = "support@agroprocessing.com"
        }
    });

    // Add XML comments if you want (optional)
    // var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    // var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    // c.IncludeXmlComments(xmlPath);

    // Group endpoints by tags
    c.TagActionsBy(api =>
    {
        if (api.GroupName != null)
        {
            return new[] { api.GroupName };
        }

        var controllerName = api.ActionDescriptor.RouteValues["controller"];
        return new[] { controllerName ?? "Unknown" };
    });

    c.DocInclusionPredicate((name, api) => true);
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register application services
builder.Services.AddScoped<IMasterDataService, MasterDataService>();
builder.Services.AddScoped<IPurchaseService, PurchaseService>();
builder.Services.AddScoped<IPurchasePaymentService, PurchasePaymentService>();
builder.Services.AddScoped<IRawInventoryService, RawInventoryService>();
builder.Services.AddScoped<IProcessingRunService, ProcessingRunService>();
builder.Services.AddScoped<IFinishedInventoryService, FinishedInventoryService>();
builder.Services.AddScoped<ISalesService, SalesService>();
builder.Services.AddScoped<ISalesPaymentService, SalesPaymentService>();
builder.Services.AddScoped<ITransportationService, TransportationService>();
builder.Services.AddScoped<IWorkerPaymentService, WorkerPaymentService>();
builder.Services.AddScoped<IReportService, ReportService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "AgroProcessing API v1");
        c.RoutePrefix = "swagger";
        c.DocumentTitle = "AgroProcessing API Documentation";
        c.DefaultModelsExpandDepth(2);
        c.DefaultModelExpandDepth(2);
        c.DisplayRequestDuration();
        c.EnableFilter();
        c.EnableTryItOutByDefault();
    });
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
