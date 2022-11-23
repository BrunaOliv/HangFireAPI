using Hangfire;
using Hangfire.Storage.SQLite;
using HangFireAPI.Service;
using HangfireBasicAuthenticationFilter;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IServiceManagement, ServiceManagement>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHangfire(configuration => configuration
            //UseSimpleAssemblyNameTypeSerializer()
            //.UseRecommendedSerializerSettings()
            .UseSQLiteStorage(builder.Configuration.GetConnectionString("DefautConnection")));


builder.Services.AddHangfireServer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHangfireDashboard("/hangfire", new DashboardOptions()
{
    DashboardTitle = "Hangfire Dashboard",
    Authorization = new[]{
    new HangfireCustomBasicAuthenticationFilter{
        User = builder.Configuration.GetSection("HangfireCredentials:UserName").Value,
        Pass = builder.Configuration.GetSection("HangfireCredentials:Password").Value
    } }
});
app.MapHangfireDashboard();

RecurringJob.AddOrUpdate<IServiceManagement>(x => x.SyncRecords(), "0 * * ? * *");

//BackgroundJob.Enqueue(() => Console.WriteLine("Bem vindo ao HingFire"));

//RecurringJob.AddOrUpdate(() => Console.WriteLine("Recurring Job"), Cron.Hourly);

//BackgroundJob.Schedule(() => Console.WriteLine("Delayed Job"),TimeSpan.FromDays(2));

//string jobId = BackgroundJob.Enqueue(() => Console.WriteLine("Tarefa pai"));
//BackgroundJob.ContinueJobWith(jobId, () => Console.WriteLine("Tarefa filha"));

app.UseAuthorization();

app.MapControllers();

app.Run();
