using SharePointDemo.Utils;
using System.Text.Json;

namespace SharePointDemo.Commands.Drives;

public class DriveCommand : Command
{
    public DriveCommand(string name = "drives", string? description = null)
        : base(name, description)
    {
    }
}

public static class DriveCommands
{
    public static DriveCommand AddDrives(this Command root)
    {
        var drives = new DriveCommand();

        var get = new Command("get");
        var driveId = new Option<string>("--drive-id") { IsRequired = true };
        get.AddOption(driveId);

        get.SetHandler(Get, driveId, new GraphClientFactory(), new OutputFormatter());

        var items = new Command("search-items");
        var searchTerm = new Option<string>("--term");
        items.AddOption(driveId);
        items.AddOption(searchTerm);
        items.SetHandler(Search, driveId, searchTerm, new GraphClientFactory());

        root.AddCommand(drives);
        drives.AddCommand(get);
        drives.AddCommand(items);

        return drives;
    }

    private static async Task Search(string driveId, string term, GraphServiceClient graphClient)
    {
        var q = graphClient.Drives[driveId].Root;

        if (!string.IsNullOrWhiteSpace(term))
        {
            q = q.ItemWithPath(term);
        }

        var items = await q
            .Request()
            .Expand("children")
            .GetAsync();

        PrintTree(items);
    }

    private static void PrintTree(DriveItem driveItem)
    {
        var root = new Tree(driveItem.Name).Style("red");

        ComposeTree(root, driveItem.Children ?? Enumerable.Empty<DriveItem>());

        AnsiConsole.Write(root);
    }

    private static void ComposeTree(IHasTreeNodes root, IEnumerable<DriveItem> driveItems)
    {
        foreach (var p in driveItems)
        {
            var node = root.AddNode($"{p.Name}[{p.Id}]".EscapeMarkup());
            if (p.Children is not null)
            {
                ComposeTree(node, p.Children);
            }
        }
    }

    private static async Task Get(
        string driveId,
        GraphServiceClient graphClient,
        OutputFormatter formatter)
    {
        var drive = await graphClient.Drives[driveId]
            .Request()
            .GetAsync();

        formatter.Print(drive);
    }
}