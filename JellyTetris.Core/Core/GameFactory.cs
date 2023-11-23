using DependencyInjection;
using JellyTetris.Utils;

namespace JellyTetris.Core;

public static class GameFactory
{
    public static IGame Make()
    {
        var container = DependencyContainerFactory.MakeLiteContainer();
        container.InitFromModules(new MainInjectModule());

        container.Resolve<IGameInitializer>().Init();

        return container.Resolve<IGame>();
    }
}
