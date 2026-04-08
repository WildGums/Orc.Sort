namespace Orc;

using Catel.Services;
using Catel.ThirdPartyNotices;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Core module which allows the registration of default services in the service collection.
/// </summary>
public static class OrcSortModule
{
    public static IServiceCollection AddOrcSort(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<ILanguageSource>(new LanguageResourceSource("Orc.Sort", "Orc.Sort.Properties", "Resources"));

        serviceCollection.AddSingleton<IThirdPartyNotice>((x) => new LibraryThirdPartyNotice("Orc.Sort", "https://github.com/wildgums/orc.sort"));
        serviceCollection.AddSingleton<IThirdPartyNotice>((x) => new ResourceBasedThirdPartyNotice("morelinq", "https://github.com/morelinq/MoreLINQ", "Orc.Sort", "Orc.Sort", "Resources.ThirdPartyNotices.morelinq.txt"));

        return serviceCollection;
    }
}
