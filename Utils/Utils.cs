using System.Collections;

namespace SharePointDemo.Utils;

public static class Utils
{
    public static void PrintTable(IEnumerable<Permission> permissions)
    {
        var table = new Table();

        table.AddColumn("Permission ID");
        table.AddColumn("Identity");
        table.AddColumn(new TableColumn("Extra").Centered());
        table.AddColumn(new TableColumn("Type").Centered());

        foreach (var permission in permissions)
        {
            if (permission.GrantedToIdentitiesV2 is not null)
            {
                foreach (var identity in permission.GrantedToIdentitiesV2)
                {
                    if (identity.Application is not null)
                    {
                        table.AddRow(
                            permission.Id,
                            identity.Application.DisplayName,
                            identity.Application.Id,
                            identity.ODataType);
                    }
                    else
                    {
                        table.AddRow(
                            permission.Id,
                            identity.SiteUser?.LoginName ?? string.Empty,
                            identity.SiteUser?.DisplayName ?? string.Empty,
                            identity.SiteUser?.ODataType ?? string.Empty);
                    }
                }
            }
            else if (permission.Invitation is var invitation && invitation is not null)
            {
                table.AddRow(
                    permission.Id,
                    invitation.Email,
                    invitation.InvitedBy.User.DisplayName,
                    invitation.ODataType);
            }
        }

        AnsiConsole.Write(table);
    }

    public static void PrintTree(IEnumerable permissions)
    {
        var rule = new Rule();
        foreach (var p in permissions)
        {
            AnsiConsole.Write(rule);
            p.DumpTree(default);
        }
    }
}
