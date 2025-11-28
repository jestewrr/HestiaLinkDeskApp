using HestiaLink.Components;
using HestiaLink.Services; // Add this using statement

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// --- ADD THIS LINE ---
builder.Services.AddScoped<UserSession>(); 
// --------------------

var app = builder.Build();

// ... rest of the file stays the same ...
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();