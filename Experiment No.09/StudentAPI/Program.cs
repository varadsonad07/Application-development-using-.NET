var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// ✅ Swagger services
builder.Services.AddEndpointsApiExplorer();//enable controllers
builder.Services.AddSwaggerGen();//enable swagger


var app = builder.Build();

// ✅ Enable Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();//connnect controller to map

app.Run();