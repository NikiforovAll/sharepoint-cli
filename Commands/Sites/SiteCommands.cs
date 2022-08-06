using ObjectDump.Extensions;
using SharePointDemo.Utils;

namespace SharePointDemo.Commands.Sites;

public class SiteCommand : Command
{
    public SiteCommand(string name = "sites", string? description = null)
        : base(name, description)
    {
    }
}

public static class SiteCommands
{
    public static SiteCommand AddSites(this Command root)
    {
        var sites = new SiteCommand("sites");

        var search = new Command("search");
        var hostName = new Option<string>("--host-name") { IsRequired = true };
        var siteName = new Option<string>("--site-name") { IsRequired = true };
        search.AddOption(hostName);
        search.AddOption(siteName);
        search.SetHandler(Search, hostName, siteName, new GraphClientFactory());

        var getSite = new Command("get");
        var siteId = new Option<string>("--site-id");
        getSite.AddOption(siteId);
        getSite.SetHandler(Get, siteId, new GraphClientFactory(), new OutputFormatter());

        root.AddCommand(sites);
        sites.AddCommand(getSite);
        sites.AddCommand(search);

        return sites;
    }

    private static async Task Get(string siteId, GraphServiceClient graphClient, OutputFormatter formatter)
    {
        var site = await graphClient.Sites[siteId]
            .Request().GetAsync();

        formatter.Print(site);
    }

    private static async Task Search(string hostName, string siteName, GraphServiceClient graphClient)
    {
        var sites = await graphClient.Sites[$"{hostName}:"].Sites[siteName]
            .Request().GetAsync();

        AnsiConsole.MarkupLine($"[underline grey]{sites.Name}[/]");
        AnsiConsole.MarkupLine($"[underline green]{sites.WebUrl}[/]");
        AnsiConsole.MarkupLine($"[underline yellow]{sites.Id}[/]");
    }
}
