using SharePointDemo.Utils;

namespace SharePointDemo;
public class GraphClientFactory : BinderBase<GraphServiceClient>
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "CodeQuality",
        "IDE0051:Remove unused private members",
        Justification = "An alternative method. Might be used in the future.")]
    private static GraphServiceClient GetClient()
    {
        var config = new ConfigurationBuilder()
            .AddEnvironmentVariables("SharePointDemo_")
            .AddUserSecrets<Program>()
            .Build();

        var tenantId = config.GetValue<string>("TenantId");
        var clientId = config.GetValue<string>("ClientId");
        var clientSecret = config.GetValue<string>("ClientSecret");

        var authProvider = new ClientSecretCredential(tenantId, clientId, clientSecret);

        var graphClient = new GraphServiceClient(authProvider);

        return graphClient;
    }

    private static GraphServiceClient GetDefaultClient()
    {
        // https://devblogs.microsoft.com/azure-sdk/authentication-and-the-azure-sdk/
        // https://docs.microsoft.com/en-us/dotnet/api/azure.identity.defaultazurecredential
        // https://blog.jongallant.com/2021/08/azure-identity-202/
        // https://github.com/Azure/azure-sdk-for-net/tree/main/sdk/identity/Azure.Identity/src/Credentials
        // export AZURE_CLIENT_ID=""
        // export AZURE_TENANT_ID=""
        // export AZURE_CLIENT_SECRET=""
        var auth = new DefaultAzureCredential(new DefaultAzureCredentialOptions
        {
            ExcludeInteractiveBrowserCredential = false,
        });

        var graphClient = new GraphServiceClient(auth);

        return graphClient;
    }

    protected override GraphServiceClient GetBoundValue(BindingContext bindingContext)
    {
        
        return GetClient();
    }
}
