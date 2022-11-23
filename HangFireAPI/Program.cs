using Hangfire;
using Hangfire.MemoryStorage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHangfire(opt =>
{
    opt.UseMemoryStorage();
});

builder.Services.AddHangfireServer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHangfireDashboard();
BackgroundJob.Enqueue(() => Console.WriteLine("Bem vindo ao HingFire"));

RecurringJob.AddOrUpdate(() => Console.WriteLine("Recurring Job"), Cron.Hourly);

BackgroundJob.Schedule(() => Console.WriteLine("Delayed Job"),
    TimeSpan.FromDays(2));

string jobId = BackgroundJob.Enqueue(() => Console.WriteLine("Tarefa pai"));
BackgroundJob.ContinueJobWith(jobId, () => Console.WriteLine("Tarefa filha"));

app.UseAuthorization();

app.MapControllers();

app.Run();
