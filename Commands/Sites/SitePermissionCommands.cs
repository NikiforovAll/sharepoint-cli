using ObjectDump.Extensions;
using SharePointDemo.Utils;

namespace SharePointDemo.Commands.Sites;

public static class SitePermissionCommands
{
    public static SiteCommand AddSitePermissions(this SiteCommand root)
    {
        var perm = new Command("perm");
        var siteId = new Option<string>("--site-id") { IsRequired = true };

        var list = new Command("list");
        list.AddOption(siteId);
        list.SetHandler(Search, siteId, new GraphClientFactory(), new OutputFormatter());

        var get = new Command("get");
        get.AddOption(siteId);
        var permissionId = new Option<string>("--permission-id") { IsRequired = true };
        get.AddOption(permissionId);

        get.SetHandler(Get, siteId, permissionId, new GraphClientFactory(), new OutputFormatter());

        root.AddCommand(perm);
        perm.AddCommand(list);
        perm.AddCommand(get);

        return root;
    }

    private static async Task Get(
        string siteId,
        string permissionId,
        GraphServiceClient graphClient,
        OutputFormatter formatter)
    {
        var site = await graphClient.Sites[siteId].Permissions[permissionId]
            .Request().GetAsync();

        formatter.Print(site);
    }

    private static async Task Search(
        string siteId,
        GraphServiceClient graphClient,
        OutputFormatter formatter)
    {
        var permissions = await graphClient.Sites[siteId].Permissions
            .Request()
            .GetAsync();

        formatter.Print(permissions);
    }
}