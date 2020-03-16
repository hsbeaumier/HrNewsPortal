using Microsoft.Extensions.Configuration;

namespace HrNewsPortal.Data.IntegrationTests
{
    public static class TestHelper
    {
        public static IConfigurationRoot InitializeConfiguration(string appSettingsJsonPath)
        {
            // User secret copied over from web project
            // to test project, and included below.
            // Deemed useful for integration tests.
            return new ConfigurationBuilder()
                .AddJsonFile(appSettingsJsonPath)
                .Build();
        }
    }
}
