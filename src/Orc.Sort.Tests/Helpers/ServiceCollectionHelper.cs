namespace Orc.Sort.Tests
{
    using Catel;
    using Microsoft.Extensions.DependencyInjection;
    using Orc.Sort;

    internal static class ServiceCollectionHelper
    {
        public static IServiceCollection CreateServiceCollection()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddLogging();
            serviceCollection.AddCatelCore();
            serviceCollection.AddOrcSort();

            return serviceCollection;
        }
    }
}
