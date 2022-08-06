using System.Text.Json;

namespace SharePointDemo.Utils;
public class OutputFormatter : BinderBase<OutputFormatter>
{
    public OutputFormat Format { get; set; }

    public static Option<OutputFormat> FormatOption = new Option<OutputFormat>("--format");

    public void Print<T>(T value)
    {
        if (value is IEnumerable<Permission> permissions && Format == OutputFormat.Table)
        {
            Utils.PrintTable(permissions);
            return;
        }

        switch (Format)
        {
            case OutputFormat.Object:
                value.DumpToConsole();
                break;
            case OutputFormat.Tree:
                value!.DumpTree(default);
                break;
            default:
                Console.WriteLine(JsonSerializer.Serialize(value, options: new JsonSerializerOptions()
                {
                    WriteIndented = true,
                }));
                break;
        }
    }

    protected override OutputFormatter GetBoundValue(BindingContext bindingContext)
    {
        var format = bindingContext.ParseResult.GetValueForOption(FormatOption);

        Format = format;

        return this;
    }
}

public enum OutputFormat
{
    Object,
    Json,
    Table,
    Tree
}
