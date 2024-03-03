using Chargily.Pay.V2.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using static System.OperatingSystem;

namespace Chargily.Pay.V2.Testing;

public class BaseTest
{
  internal IChargilyPayClient _chargilyPayClient;

  [SetUp]
  public void Setup()
  {
    var configuration = new ConfigurationBuilder()
                       .AddEnvironmentVariables("CHARGILY_SECRET_KEY")
                       .Build();
    var apiSecret = IsWindows()
                      ? Environment.GetEnvironmentVariable("CHARGILY_SECRET_KEY", EnvironmentVariableTarget.User)
                      : configuration["CHARGILY_SECRET_KEY"]!;
    
    Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Verbose)
                .WriteTo.Console(theme: AnsiConsoleTheme.Code, restrictedToMinimumLevel: LogEventLevel.Debug)
                .WriteTo.NUnitOutput(restrictedToMinimumLevel: LogEventLevel.Debug)
                .CreateLogger();

    _chargilyPayClient = ChargilyPay.CreateResilientClient(config =>
                                                           {
                                                             config.IsLiveMode = false;
                                                             config.ApiSecretKey = apiSecret!;
                                                           },
                                                           log =>
                                                           {
                                                             log.AddSerilog();
                                                             log.SetMinimumLevel(LogLevel.Debug);
                                                           });
  }
  

  [TearDown]
  public void TearDown()
  {
    _chargilyPayClient?.Dispose();
  }
}