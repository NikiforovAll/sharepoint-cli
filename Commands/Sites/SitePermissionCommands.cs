using ObjectDump.Extensions;

namespace SharePointDemo.Commands.Sites;

public static class SitePermissionCommands
{
    public static SiteCommand AddSitePermissions(this SiteCommand root)
    {
        var perm = new Command("perm");
        var siteId = new Option<string>("--site-id") { IsRequired = true };
        var display = new Option<string>("--display", getDefaultValue: () => "table")
            .FromAmong("tree", "table");

        var list = new Command("list");
        list.AddOption(siteId);
        list.AddOption(display);
        list.SetHandler(Search, siteId, display, new GraphClientFactory());

        var get = new Command("get");
        get.AddOption(siteId);
        var permissionId = new Option<string>("--permission-id") { IsRequired = true };
        get.AddOption(permissionId);

        get.SetHandler(Get, siteId, permissionId, new GraphClientFactory());

        root.AddCommand(perm);
        perm.AddCommand(list);
        perm.AddCommand(get);

        return root;
    }

    private static async Task Get(string siteId, string permissionId, GraphServiceClient graphClient)
    {
        var site = await graphClient.Sites[siteId].Permissions[permissionId]
            .Request().GetAsync();

        site.DumpToConsole();
    }

    private static async Task Search(string siteId, string displayMode, GraphServiceClient graphClient)
    {
        var permissions = await graphClient.Sites[siteId].Permissions
            .Request()
            .GetAsync();

        switch (displayMode)
        {
            case "table":
                PrintTable(permissions);
                break;
            case "tree":
                PrintTree(permissions);
                break;
            default:
                break;
        }
    }

    private static void PrintTable(ICollectionPage<Permission> permissions)
    {
        var table = new Table();

        table.AddColumn("Permission ID");
        table.AddColumn("Application");
        table.AddColumn(new TableColumn("Id").Centered());

        var ps = permissions
            .SelectMany(p => p.GrantedToIdentitiesV2.Select(pp => new { pp.Application, p.Id }));

        foreach (var p in ps)
        {
            table.AddRow(p.Id, p.Application.DisplayName, p.Application.Id);
        }

        AnsiConsole.Write(table);
    }

    private static void PrintTree(ICollectionPage<Permission> permissions)
    {
        var root = new Tree(".").Style("red");

        foreach (var p in permissions)
        {
            var node = root.AddNode(p.Id);

            foreach (var a in p.GrantedToIdentitiesV2.Select(i => i.Application))
            {
                node.AddNode(a.DisplayName);
                node.AddNode(a.Id);
            }
        }

        AnsiConsole.Write(root);
    }
}