using TodoApi.StartUpConfig;

var builder = WebApplication.CreateBuilder(args);

builder.AddStandardServices();
builder.AddAuthenticationServices();
builder.AddCustomServices();
builder.AddHealthCheckServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health").AllowAnonymous();

app.Run();
