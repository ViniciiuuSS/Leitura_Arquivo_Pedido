var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWebView2", builder =>
    {
        builder.WithOrigins("http://app.local") // Permite apenas a origem do WebView2
               .AllowAnyMethod() // Permite todos os m�todos (GET, PUT, etc.)
               .AllowAnyHeader() // Permite todos os cabe�alhos
               .AllowCredentials(); // Se necess�rio, permite cookies ou credenciais
    });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowWebView2");
app.UseAuthorization();

app.MapControllers();

app.Run();
