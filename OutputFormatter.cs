using System.Text.Json;

namespace SharePointDemo.Utils;
public class OutputFormatter : BinderBase<OutputFormatter>
{
    public OutputFormat Format { get; set; }

    public static Option<OutputFormat> FormatOption = new Option<OutputFormat>("--format");

    public void Print<T>(T value)
    {
        if (value is IEnumerable<Permission> permissions)
        {
            Print(permissions);
            return;
        }

        switch (Format)
        {
            case OutputFormat.Object:
                value.DumpToConsole();
                break;
            default:
                Console.WriteLine(JsonSerializer.Serialize(value, options: new JsonSerializerOptions()
                {
                    WriteIndented = true,
                }));
                break;
        }
    }

    public void Print(IEnumerable<Permission> permissions)
    {
        switch (Format)
        {
            case OutputFormat.Table:
                Utils.PrintTable(permissions);
                break;
            case OutputFormat.Tree:
                Utils.PrintTree(permissions);
                break;
            default:
                Console.WriteLine(JsonSerializer.Serialize(permissions, options: new JsonSerializerOptions()
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
