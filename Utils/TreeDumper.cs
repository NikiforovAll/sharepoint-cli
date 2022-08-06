using System.Collections;
using System.Reflection;

namespace SharePointDemo.Utils;

public static class TreeDumper
{
    private const string emptyDisplayLabel = "<empty>";

    public static void DumpTree(this object obj, string? label)
    {
        var node = new Tree(label ?? ".").Style("red");

        DumpInternal(obj, node);

        AnsiConsole.Write(node);
    }

    private static void DumpInternal(object obj, IHasTreeNodes node)
    {
        if (obj is null)
        {
            return;
        }
        Type type = obj.GetType();

        PropertyInfo[] properties =
            (from property in type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
             where property.GetIndexParameters().Length == 0
                   && property.CanRead
             select property).ToArray();

        if (!properties.Any())
        {
            node.AddNode(obj.ToString()?.EscapeMarkup() ?? emptyDisplayLabel);
            return;
        }

        foreach (PropertyInfo pi in properties)
        {
            var v = pi.GetValue(obj, null);

            if (type.IsValueType || v is string)
            {
                node.AddNode(pi.Name).AddNode(v?.ToString() ?? emptyDisplayLabel);
            }
            else if (v is not null)
            {
                var n = node.AddNode($"{pi.Name}");

                DumpInternal(v, n);
            }
        }

        if (obj is IEnumerable collection && obj is not string)
        {
            foreach (var collectionItem in collection)
            {
                DumpInternal(collectionItem, node);
            }
        }
    }
}