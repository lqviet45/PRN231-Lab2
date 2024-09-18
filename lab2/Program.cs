using lab2;
using lab2.common;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Routing;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEntityFrameworkSqlite()
    .AddDbContext<BookStoreDbContext>();

builder.Services.AddControllers()
    .AddOData(op =>
    {
        op.Select().Filter().Count().OrderBy().Expand().SetMaxTop(100)
            .AddRouteComponents("odata", OdataConfig.GetEdmModel());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<BookStoreDbContext>();
    await dbContext.Database.MigrateAsync();
}

app.UseODataBatching();

app.UseRouting();

app.Use(next => context =>
{
    var endpoint = context.GetEndpoint();
    if (endpoint == null)
    {
        return next(context);
    }

    IEnumerable<string> templates;
    var metadata = endpoint
        .Metadata
        .GetMetadata<IODataRoutingMetadata>();

    if (metadata != null)
    {
        templates = metadata.Template.GetTemplates();
    }
    
    return next(context);
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();