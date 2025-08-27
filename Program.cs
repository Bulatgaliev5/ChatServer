using ChatServer.Hubs;

var builder = WebApplication.CreateBuilder(args);

// -----------------
// ��������� �������
// -----------------
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();

// CORS ��� ���������� ������� / WPF
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMobileClient", policy =>
    {
        policy.SetIsOriginAllowed(_ => true)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// -----------------
// ��������� Kestrel �� Render
// -----------------
builder.WebHost.ConfigureKestrel(options =>
{
    // Render ����� ���� ����� ���������� ��������� PORT
    var portStr = Environment.GetEnvironmentVariable("PORT");
    int port = 5000;
    if (!string.IsNullOrEmpty(portStr) && int.TryParse(portStr, out var p))
        port = p;

    options.ListenAnyIP(port);
});

// -----------------
// ������ ����������
// -----------------
var app = builder.Build();

// -----------------
// Middleware
// -----------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowMobileClient");

// -----------------
// �������� � ����
// -----------------
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<ChatHub>("/chathub");

// -----------------
// ������
// -----------------
app.Run();
