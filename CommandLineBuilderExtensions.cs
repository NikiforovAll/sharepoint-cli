using System.CommandLine.Builder;

namespace SharePointDemo;
public static class CommandLineBuilderExtensions
{
    public static CommandLineBuilder UseCustomExceptionHandler(this CommandLineBuilder builder)
    {
        builder.AddMiddleware(async (context, next) =>
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                var settings = new ExceptionSettings
                {
                    Format = ExceptionFormats.ShortenPaths |
                        ExceptionFormats.ShortenTypes |
                        ExceptionFormats.ShortenMethods |
                        ExceptionFormats.ShowLinks,
                    Style = new ExceptionStyle
                    {
                        Exception = new Style().Foreground(Color.Grey),
                        Message = new Style().Foreground(Color.White),
                        NonEmphasized = new Style().Foreground(Color.Cornsilk1),
                        Parenthesis = new Style().Foreground(Color.Cornsilk1),
                        Method = new Style().Foreground(Color.Red),
                        ParameterName = new Style().Foreground(Color.Cornsilk1),
                        ParameterType = new Style().Foreground(Color.Red),
                        Path = new Style().Foreground(Color.Red),
                        LineNumber = new Style().Foreground(Color.Cornsilk1),
                    }
                };

                AnsiConsole.WriteException(ex, settings);
            }
        });

        return builder;
    }
}
