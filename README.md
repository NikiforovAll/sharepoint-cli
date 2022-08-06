# SharePointDemo CLI

```bash
$ dotnet run --
Required command was not provided.

Description:
  SharePoint CLI

Usage:
  SharePointDemo [command] [options]

Options:
  --version       Show version information
  -?, -h, --help  Show help and usage information

Commands:
  sites
  drives
```


It is possible to grant one service account "Sites.Selected" permission and grant an access to a specific site. By doing this, the granted application can access SharePoint DriveItems and grant permissions on per-item basis.

Ref: <https://devblogs.microsoft.com/microsoft365dev/controlling-app-access-on-specific-sharepoint-site-collections/>

Sharing could be achieved in various ways:

* Direct invite of a User
* Create and managed Share Link
* Delegated access via Group
* Delegated access via access to parent resource

Granting same permission could be safely performed, but side effects should be expected. For example, you can send invites multiple times, but only one permission record is maintained. Although, a user receives multiple emails. 