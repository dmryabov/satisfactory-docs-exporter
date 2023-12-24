using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using ApplicationException = SatisfactoryDocsExporter.Exceptions.ApplicationException;

namespace SatisfactoryDocsExporter.Console;

public static class ConsoleApplication
{
    public static int Invoke(string[] args)
    {
        Log.Logger = new LoggerConfiguration().WriteTo.Console(theme: AnsiConsoleTheme.Literate, standardErrorFromLevel: LogEventLevel.Verbose).CreateLogger();

        var cmd = new CommandLineBuilder(ExportDocsCommand.Create());

        cmd.UseExceptionHandler(HandleException);
        cmd.UseHelp();

        return cmd.Build().Invoke(args);
    }

    private static void HandleException(Exception exception, InvocationContext context)
    {
        if (exception is ApplicationException)
            Log.Fatal(exception.InnerException, exception.Message);
        else
            Log.Fatal(exception, "Internal application error.");

        context.ExitCode = 1;
    }
}