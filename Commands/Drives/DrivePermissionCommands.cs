using ObjectDump.Extensions;
using SharePointDemo.Utils;

namespace SharePointDemo.Commands.Drives;

public static class DrivePermissionCommands
{
    public static DriveCommand AddDrivePermissions(this DriveCommand root)
    {
        var perm = new Command("perm");
        var driveId = new Option<string>("--drive-id") { IsRequired = true };
        var itemId = new Option<string>("--item-id") { IsRequired = true };

        var list = new Command("list");
        list.AddOption(driveId);
        list.AddOption(itemId);
        list.SetHandler(Search, driveId, itemId, new GraphClientFactory(), new OutputFormatter());

        var get = new Command("get");
        get.AddOption(driveId);
        get.AddOption(itemId);

        var permissionId = new Option<string>("--permission-id") { IsRequired = true };
        get.AddOption(permissionId);

        get.SetHandler(Get, driveId, itemId, permissionId, new GraphClientFactory(), new OutputFormatter());

        root.AddCommand(perm);
        perm.AddCommand(list);
        perm.AddCommand(get);

        return root;
    }

    private static async Task Get(
        string driveId,
        string itemId,
        string permissionId,
        GraphServiceClient graphClient,
        OutputFormatter formatter)
    {
        var site = await graphClient.Drives[driveId].Items[itemId].Permissions[permissionId]
            .Request().GetAsync();

        formatter.Print(site);
    }

    private static async Task Search(
        string driveId,
        string itemId,
        GraphServiceClient graphClient,
        OutputFormatter formatter)
    {
        var permissions = await graphClient.Drives[driveId].Items[itemId].Permissions
            .Request()
            .GetAsync();

        formatter.Print(permissions);
    }
}