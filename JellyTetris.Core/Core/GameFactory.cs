using DependencyInjection;
using JellyTetris.Utils;

namespace JellyTetris.Core;

public static class GameFactory
{
    public static IGame Make()
    {
        var container = DependencyContainerFactory.MakeLiteContainer();
        container.InitFromModules(new MainInjectModule());

        return container.Resolve<IGame>();
    }
}
