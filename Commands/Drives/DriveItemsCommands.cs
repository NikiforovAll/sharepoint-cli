namespace SharePointDemo.Commands.Drives;

public static class DriveItemsCommands
{
    public static DriveCommand AddDriveItems(this DriveCommand root)
    {
        var drives = new Command("items");

        var get = new Command("get");
        var driveId = new Option<string>("--drive-id") { IsRequired = true };
        var itemId = new Option<string>("--item-id") { IsRequired = true };
        get.AddOption(driveId);
        get.AddOption(itemId);
        get.SetHandler(Get, driveId, itemId, new GraphClientFactory(), new OutputFormatter());

        root.AddCommand(drives);
        drives.AddCommand(get);

        return root;
    }

    private static async Task Get(
        string driveId,
        string itemId,
        GraphServiceClient graphClient,
        OutputFormatter formatter)
    {
        var item = await graphClient.Drives[driveId].Items[itemId]
            .Request()
            .Expand("children")
            .GetAsync();

        formatter.Print(item);
    }
}