
using ObjectDump.Extensions;
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

        get.SetHandler(Get, driveId, new GraphClientFactory());

        root.AddCommand(drives);
        drives.AddCommand(get);

        return drives;
    }

    private static async Task Get(string driveId, GraphServiceClient graphClient)
    {
        var drive = await graphClient.Drives[driveId]
            .Request().GetAsync();

        Console.WriteLine(JsonSerializer.Serialize(drive, options: new JsonSerializerOptions()
        {
            WriteIndented = true,
        }));
    }
}