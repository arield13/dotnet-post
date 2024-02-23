using Post.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var MyPolicy = "AllowAll";
builder.Services.AddCors(o => o.AddPolicy(MyPolicy, policy => {
    policy.AllowAnyHeader()
    .AllowAnyOrigin()
    .AllowAnyMethod();
}));

builder.Services.AddScoped<PostDataService>();

var app = builder.Build();
app.UseCors(MyPolicy);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
