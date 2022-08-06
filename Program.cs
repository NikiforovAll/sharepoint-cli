using SharePointDemo;
using SharePointDemo.Commands.Drives;
using SharePointDemo.Commands.Sites;

var root = new RootCommand("SharePoint CLI");

root.AddGlobalOption(OutputFormatter.FormatOption);

root
    .AddSites()
        .AddSitePermissions()
        .AddSiteDrives();

root
    .AddDrives()
        .AddDriveItems()
        .AddDrivePermissions();

var builder = new CommandLineBuilder(root)
    .UseCustomExceptionHandler()
    .UseDefaults();

var parser = builder.Build();

await parser.InvokeAsync(args);
