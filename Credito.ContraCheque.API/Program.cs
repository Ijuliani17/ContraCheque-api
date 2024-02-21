using Credito.ContraCheque.API;
using Serilog;
using Serilog.Events;
using Serilog.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Logging
    .ClearProviders()
    .AddSerilog(new LoggerConfiguration()
       .MinimumLevel.Override("Microsoft.AspNetCore",
           (LogEventLevel)Enum.Parse(typeof(LogEventLevel), builder.Configuration["LOGLEVEL"]))
       .Filter.ByExcluding(Matching.FromSource("Microsoft.AspNetCore.StaticFiles"))
       .WriteTo.Async(wt =>
           wt.Console(
               outputTemplate:
               "[{Timestamp:dd/MM/yyyy HH:mm:ss} {Level:u3}] {Message:lj} {NewLine}{Exception}"))
       .CreateLogger());

builder.ConfigurarApi();

var app = builder.Build();

#if DEBUG
    app.UseSwagger();
    app.UseSwaggerUI();
#endif

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
