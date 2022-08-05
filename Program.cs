using SharePointDemo.Commands.Drives;
using SharePointDemo.Commands.Sites;

var root = new RootCommand("SharePoint CLI");

root
    .AddSites()
        .AddSitePermissions()
        .AddSiteDrives();

root
    .AddDrives()
        .AddDriveItems();

await root.InvokeAsync(args);