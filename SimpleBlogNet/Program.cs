using Microsoft.Data.Sqlite;
using SimpleBlogNet.Database;
using System.Data.Common;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddTransient<DbConnection>((s) => new SqliteConnection(builder.Configuration["SQLite"]));
builder.Services.AddTransient<DatabaseBootstrap>();
builder.Services.AddScoped<DatabaseRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

await app.Services.GetRequiredService<DatabaseBootstrap>().VerifyAndBootstrapIfNecessaryAsync();

app.Run();
