using ObjectDump.Extensions;
using System.Text.Json;

namespace SharePointDemo.Commands.Sites;

public static class SiteDriveCommands
{
    public static SiteCommand AddSiteDrives(this SiteCommand root)
    {
        var drives = new Command("drives");
        var siteId = new Option<string>("--site-id") { IsRequired = true };

        var list = new Command("list");
        list.AddOption(siteId);
        list.SetHandler(List, siteId, new GraphClientFactory());

        var get = new Command("get");
        get.AddOption(siteId);
        var driveId = new Option<string>("--drive-id") { IsRequired = true };
        get.AddOption(driveId);

        get.SetHandler(Get, siteId, driveId, new GraphClientFactory());

        root.AddCommand(drives);
        drives.AddCommand(list);
        drives.AddCommand(get);

        return root;
    }

    private static async Task Get(string siteId, string driveId, GraphServiceClient graphClient)
    {
        var drive = await graphClient.Sites[siteId].Drives[driveId]
            .Request().GetAsync();

        Console.WriteLine(JsonSerializer.Serialize(drive, options: new JsonSerializerOptions()
        {
            WriteIndented = true,
        }));
    }

    private static async Task List(string siteId, GraphServiceClient graphClient)
    {
        var drives = await graphClient.Sites[siteId].Drives
            .Request()
            .GetAsync();

        PrintTable(drives);
    }

    private static void PrintTable(ICollectionPage<Drive> drives)
    {
        var table = new Table();

        table.AddColumn("Id");
        table.AddColumn("Name");
        table.AddColumn("Type");

        foreach (var d in drives)
        {
            table.AddRow(d.Id, d.Name, d.DriveType);
        }

        AnsiConsole.Write(table);
    }
}